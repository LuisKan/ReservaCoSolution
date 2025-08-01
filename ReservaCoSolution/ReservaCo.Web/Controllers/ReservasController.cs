using System;
using System.Linq;
using System.Web.Http;
using ReservaCo.Application.Service;
using ReservaCo.Application.Services;
using ReservaCo.Domain.Entities;
using ReservaCo.Infrastructure.Context;

// Evita conflictos
using EntidadReserva = ReservaCo.Domain.Entities.Reserva;
using ModeloReserva = ReservaCo.Web.Models.Reserva;



using System.IO;
using System.Net.Http;
using System.Net;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data.Entity;
using System.Collections.Generic;

namespace ReservaCo.Web.Controllers
{
    [RoutePrefix("api/reservas")]
    public class ReservasController : ApiController
    {
        private readonly ReservaService _reservaService;
        private readonly UsuarioService _usuarioService;
        private readonly EspacioService _espacioService;

        public ReservasController()
        {
            var context = new ReservaCoDbContext();
            _reservaService = new ReservaService(context);
            _usuarioService = new UsuarioService(context);
            _espacioService = new EspacioService(context);
        }


        // GET: api/reservas
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            var reservas = _reservaService.ObtenerTodasLasReservas();

            foreach (var r in reservas)
            {
                Console.WriteLine($"Reserva #{r.Id} - {r.Fecha.ToShortDateString()} - Usuario: {(r.Usuario?.Nombre ?? "NULL")} - Espacio: {(r.Espacio?.Nombre ?? "NULL")}");
            }

            var modelos = reservas.Select(r => new ModeloReserva
            {
                ID_Reserva = r.Id,
                Estado = r.Estado.ToString(),
                FechaCreacion = r.FechaCreacion,
                ID_Usuario = r.Usuario?.Id ?? 0,
                ID_Espacio = r.Espacio?.Id ?? 0,
                Fecha = r.Fecha,
                HoraInicio = r.HoraInicio,
                HoraFin = r.HoraFin,
                NombreUsuario = r.Usuario?.Nombre,
                NombreEspacio = r.Espacio?.Nombre
            }).ToList();

            return Ok(modelos);
        }

        // GET: api/reservas/5
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var r = _reservaService.ObtenerReservaPorId(id);
            if (r == null)
                return NotFound();

            var model = new ModeloReserva
            {
                ID_Reserva = r.Id,
                Estado = r.Estado.ToString(),
                FechaCreacion = r.FechaCreacion,
                ID_Usuario = r.Usuario?.Id ?? 0,
                ID_Espacio = r.Espacio?.Id ?? 0,
                Fecha = r.Fecha,
                HoraInicio = r.HoraInicio,
                HoraFin = r.HoraFin,
                NombreUsuario = r.Usuario?.Nombre,
                NombreEspacio = r.Espacio?.Nombre
            };

            return Ok(model);
        }

        // POST: api/reservas
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post(ModeloReserva model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = _usuarioService.ObtenerUsuarioPorId(model.ID_Usuario);
            var espacio = _espacioService.ObtenerEspacios().FirstOrDefault(e => e.Id == model.ID_Espacio);

            if (usuario == null) return BadRequest("El usuario no existe.");
            if (espacio == null) return BadRequest("El espacio no existe.");

            var disponible = _reservaService.ValidarDisponibilidad(model.Fecha, model.HoraInicio, model.HoraFin, espacio.Id);
            if (!disponible)
                return BadRequest("El espacio no está disponible en ese horario.");

            var reserva = new EntidadReserva
            {
                Estado = EstadoReserva.Pendiente,
                FechaCreacion = DateTime.Now,
                Usuario = usuario,
                Espacio = espacio,
                Fecha = model.Fecha,
                HoraInicio = model.HoraInicio,
                HoraFin = model.HoraFin
            };

            _reservaService.GuardarReserva(reserva);

            model.ID_Reserva = reserva.Id;
            model.FechaCreacion = reserva.FechaCreacion;
            model.NombreUsuario = usuario.Nombre;
            model.NombreEspacio = espacio.Nombre;

            return Ok(model);
        }

        // PUT: api/reservas/5
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Put(int id, ModeloReserva model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reserva = _reservaService.ObtenerReservaPorId(id);
            if (reserva == null)
                return NotFound();

            var usuario = _usuarioService.ObtenerUsuarioPorId(model.ID_Usuario);
            var espacio = _espacioService.ObtenerEspacios().FirstOrDefault(e => e.Id == model.ID_Espacio);
            if (usuario == null || espacio == null)
                return BadRequest("Usuario o espacio inválido.");

            reserva.Usuario = usuario;
            reserva.Espacio = espacio;
            reserva.Fecha = model.Fecha;
            reserva.HoraInicio = model.HoraInicio;
            reserva.HoraFin = model.HoraFin;
            if (!Enum.TryParse(model.Estado, out EstadoReserva estadoEnum))
                return BadRequest("Estado no válido.");
            reserva.Estado = estadoEnum;


            _reservaService.GuardarReserva(reserva);
            return Ok(model);
        }

        // DELETE: api/reservas/5
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            var reserva = _reservaService.ObtenerReservaPorId(id);
            if (reserva == null)
                return NotFound();

            _reservaService.EliminarReserva(id);
            return Ok();
        }

        // POST: api/reservas/{id}/aprobar
        [HttpPost]
        [Route("{id:int}/aprobar")]
        public IHttpActionResult Aprobar(int id)
        {
            _reservaService.AprobarReserva(id);
            return Ok();
        }

        // POST: api/reservas/{id}/rechazar
        [HttpPost]
        [Route("{id:int}/rechazar")]
        public IHttpActionResult Rechazar(int id)
        {
            _reservaService.RechazarReserva(id);
            return Ok();
        }

        //exportar
        [HttpGet]
        [Route("exportar")]
        public HttpResponseMessage ExportarReservasPDF(string rol)
        {
            // Solo permitir acceso si el rol es Administrador
            if (rol == null || rol.ToLower() != "administrador")
            {
                var responseUnauthorized = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent("No tiene permisos para exportar reservas.")
                };
                return responseUnauthorized;
            }

            var reservas = _reservaService.ObtenerTodasLasReservas();

            using (var ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 50, 50, 25, 25);
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();

                // Título
                var titulo = new Paragraph("Lista de Reservas")
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20f,
                    Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16)
                };
                doc.Add(titulo);

                // Tabla
                PdfPTable tabla = new PdfPTable(6)
                {
                    WidthPercentage = 100
                };

                tabla.AddCell("Fecha");
                tabla.AddCell("Hora Inicio");
                tabla.AddCell("Hora Fin");
                tabla.AddCell("Espacio");
                tabla.AddCell("Usuario");
                tabla.AddCell("Estado");

                foreach (var r in reservas)
                {
                    tabla.AddCell(r.Fecha.ToShortDateString());
                    tabla.AddCell(r.HoraInicio.ToString(@"hh\:mm"));
                    tabla.AddCell(r.HoraFin.ToString(@"hh\:mm"));
                    tabla.AddCell(r.Espacio?.Nombre ?? "-");
                    tabla.AddCell(r.Usuario?.Nombre ?? "-");
                    tabla.AddCell(r.Estado.ToString());
                }

                doc.Add(tabla);
                doc.Close();

                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(ms.ToArray())
                };
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = "reservas.pdf"
                };
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");

                return response;
            }
        }




        ///api/Reservas/filtrar?periodo=semana
        [HttpGet]
        [Route("filtrar")]
        public IHttpActionResult FiltrarReservasPorPeriodo(string periodo = "dia")
        {
            var reservas = _reservaService.ObtenerTodasLasReservas()
                .ToList();

            if (!reservas.Any())
                return NotFound();

            IEnumerable<IGrouping<string, Reserva>> agrupadas;

            switch (periodo.ToLower())
            {
                case "semana":
                    agrupadas = reservas
                        .GroupBy(r => System.Globalization.CultureInfo.CurrentCulture.Calendar
                            .GetWeekOfYear(r.Fecha, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString());
                    break;

                case "mes":
                    agrupadas = reservas
                        .GroupBy(r => r.Fecha.ToString("yyyy-MM"));
                    break;

                case "dia":
                default:
                    agrupadas = reservas
                        .GroupBy(r => r.Fecha.ToString("yyyy-MM-dd"));
                    break;
            }

            var resultado = agrupadas.Select(g => new
            {
                Agrupado = g.Key,
                TotalReservas = g.Count(),
                Reservas = g.Select(r => new
                {
                    r.Id,
                    r.Fecha,
                    HoraInicio = r.HoraInicio.ToString(@"hh\:mm"),
                    HoraFin = r.HoraFin.ToString(@"hh\:mm"),
                    Espacio = r.Espacio?.Nombre,
                    Usuario = r.Usuario?.Nombre,
                    Estado = r.Estado.ToString()
                }).ToList()
            });

            return Ok(resultado);
        }




        [HttpGet]
        [Route("filtrar-avanzado")]
        public IHttpActionResult FiltrarAvanzado(string rol, int? usuarioId = null, string tipoEspacio = null, string estado = null)
        {
            // Validar rol correctamente (con coincidencia flexible)
            if (!string.Equals(rol, "Administrador", StringComparison.InvariantCultureIgnoreCase) &&
                !string.Equals(rol, "Coordinador", StringComparison.InvariantCultureIgnoreCase))
            {
                return Unauthorized();
            }

            var reservas = _reservaService.ObtenerTodasLasReservas().AsQueryable();

            if (usuarioId.HasValue)
                reservas = reservas.Where(r => r.Usuario.Id == usuarioId);

            if (!string.IsNullOrEmpty(tipoEspacio))
                reservas = reservas.Where(r => r.Espacio.Tipo.ToLower() == tipoEspacio.ToLower());

            if (!string.IsNullOrEmpty(estado) && Enum.TryParse(estado, true, out EstadoReserva estadoEnum))
                reservas = reservas.Where(r => r.Estado == estadoEnum);

            // Convertir a memoria y luego hacer la proyección
            var resultado = reservas
                .ToList()
                .Select(r => new ModeloReserva
                {
                    ID_Reserva = r.Id,
                    Estado = r.Estado.ToString(),
                    FechaCreacion = r.FechaCreacion,
                    ID_Usuario = r.Usuario?.Id ?? 0,
                    ID_Espacio = r.Espacio?.Id ?? 0,
                    Fecha = r.Fecha,
                    HoraInicio = r.HoraInicio,
                    HoraFin = r.HoraFin,
                    NombreUsuario = r.Usuario?.Nombre,
                    NombreEspacio = r.Espacio?.Nombre
                })
                .ToList();

            return Ok(resultado);
        }


        //GET /api/reservas/filtrar-avanzado?rol=admin&usuarioId=3&tipoEspacio=Auditorio&estado=Aprobada



        // GET: api/reservas/usuario/3
        [HttpGet]
        [Route("usuario/{id:int}")]
        public IHttpActionResult ObtenerReservasPorUsuario(int id)
        {
            var usuario = _usuarioService.ObtenerUsuarioPorId(id);
            if (usuario == null)
                return NotFound();

            var reservas = _reservaService.ObtenerTodasLasReservas()
                .Where(r => r.Usuario != null && r.Usuario.Id == id)
                .ToList();

            var modelos = reservas.Select(r => new ModeloReserva
            {
                ID_Reserva = r.Id,
                Estado = r.Estado.ToString(),
                FechaCreacion = r.FechaCreacion,
                ID_Usuario = r.Usuario?.Id ?? 0,
                ID_Espacio = r.Espacio?.Id ?? 0,
                Fecha = r.Fecha,
                HoraInicio = r.HoraInicio,
                HoraFin = r.HoraFin,
                NombreUsuario = r.Usuario?.Nombre,
                NombreEspacio = r.Espacio?.Nombre
            }).ToList();

            return Ok(modelos);
        }


        [HttpGet]
        [Route("historial/usuario/{id:int}")]
        public IHttpActionResult HistorialPorUsuario(int id)
        {
            var usuario = _usuarioService.ObtenerUsuarioPorId(id);
            if (usuario == null)
                return NotFound();

            var reservas = _reservaService.ObtenerTodasLasReservas()
                .Where(r => r.Usuario != null && r.Usuario.Id == id)
                .ToList();

            var resultado = reservas.Select(r => new ModeloReserva
            {
                ID_Reserva = r.Id,
                Estado = r.Estado.ToString(),
                FechaCreacion = r.FechaCreacion,
                ID_Usuario = r.Usuario?.Id ?? 0,
                ID_Espacio = r.Espacio?.Id ?? 0,
                Fecha = r.Fecha,
                HoraInicio = r.HoraInicio,
                HoraFin = r.HoraFin,
                NombreUsuario = r.Usuario?.Nombre,
                NombreEspacio = r.Espacio?.Nombre
            }).ToList();

            return Ok(resultado);
        }

        [HttpGet]
        [Route("historial/espacio/{id:int}")]
        public IHttpActionResult HistorialPorEspacio(int id)
        {
            var espacio = _espacioService.ObtenerEspacios().FirstOrDefault(e => e.Id == id);
            if (espacio == null)
                return NotFound();

            var reservas = _reservaService.ObtenerTodasLasReservas()
                .Where(r => r.Espacio != null && r.Espacio.Id == id)
                .ToList();

            var resultado = reservas.Select(r => new ModeloReserva
            {
                ID_Reserva = r.Id,
                Estado = r.Estado.ToString(),
                FechaCreacion = r.FechaCreacion,
                ID_Usuario = r.Usuario?.Id ?? 0,
                ID_Espacio = r.Espacio?.Id ?? 0,
                Fecha = r.Fecha,
                HoraInicio = r.HoraInicio,
                HoraFin = r.HoraFin,
                NombreUsuario = r.Usuario?.Nombre,
                NombreEspacio = r.Espacio?.Nombre
            }).ToList();

            return Ok(resultado);
        }





    }
}
