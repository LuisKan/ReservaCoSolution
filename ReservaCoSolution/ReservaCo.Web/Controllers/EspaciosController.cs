// ************************************************************************
// Proyecto 02 
// Aguilar Verónica, Guerrero Luis
// Fecha de realización: 21/07/2025 
// Fecha de entrega: 03/08/2025  
// Resultados:
// - Este controlador gestiona operaciones CRUD para espacios físicos dentro del sistema.
// - Expone rutas RESTful para registrar, consultar, editar y eliminar espacios, además de exportarlos en formato PDF.
// - Utiliza el servicio de aplicación 'EspacioService' para mantener la separación de responsabilidades.
// ************************************************************************

using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ReservaCo.Application.Service;
using ReservaCo.Application.Services;
using ReservaCo.Infrastructure.Context;

// Alias para diferenciar modelo de dominio y modelo de vista
using EntidadEspacio = ReservaCo.Domain.Entities.Espacio;
using ModeloEspacio = ReservaCo.Web.Models.Espacio;

using System.IO;
using System.Net.Http;
using System.Net;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ReservaCo.Web.Controllers
{
    // Define el prefijo base para todas las rutas de este controlador: api/espacios
    [RoutePrefix("api/espacios")]
    public class EspaciosController : ApiController
    {
        // Servicio de aplicación para gestión de espacios
        private readonly EspacioService _espacioService;

        // Constructor que inicializa el servicio con un contexto nuevo
        public EspaciosController()
        {
            var context = new ReservaCoDbContext();
            _espacioService = new EspacioService(context);
        }

        // GET: api/espacios
        // Devuelve todos los espacios registrados
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            var espacios = _espacioService.ObtenerEspacios();

            // Mapea entidades a modelos de vista
            var modelos = espacios.Select(e => new ModeloEspacio
            {
                ID_Espacio = e.Id,
                Nombre = e.Nombre,
                Tipo = e.Tipo,
                Ubicacion = e.Ubicacion,
                Capacidad = e.Capacidad
            }).ToList();

            return Ok(modelos);
        }

        // GET: api/espacios/5
        // Devuelve un espacio específico por su ID
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var espacio = _espacioService.ObtenerEspacios()
                .FirstOrDefault(e => e.Id == id);

            if (espacio == null)
                return NotFound();

            var model = new ModeloEspacio
            {
                ID_Espacio = espacio.Id,
                Nombre = espacio.Nombre,
                Tipo = espacio.Tipo,
                Ubicacion = espacio.Ubicacion,
                Capacidad = espacio.Capacidad
            };

            return Ok(model);
        }

        // POST: api/espacios
        // Crea un nuevo espacio
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post(ModeloEspacio model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entidad = new EntidadEspacio
            {
                Nombre = model.Nombre,
                Tipo = model.Tipo,
                Ubicacion = model.Ubicacion,
                Capacidad = model.Capacidad
            };

            _espacioService.GuardarEspacio(entidad);
            model.ID_Espacio = entidad.Id;

            return Ok(model);
        }

        // PUT: api/espacios/5
        // Actualiza los datos de un espacio existente
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Put(int id, ModeloEspacio model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var espacio = _espacioService.ObtenerEspacios()
                .FirstOrDefault(e => e.Id == id);

            if (espacio == null)
                return NotFound();

            // Actualiza propiedades
            espacio.Nombre = model.Nombre;
            espacio.Tipo = model.Tipo;
            espacio.Ubicacion = model.Ubicacion;
            espacio.Capacidad = model.Capacidad;

            _espacioService.GuardarEspacio(espacio);

            return Ok(model);
        }

        // DELETE: api/espacios/5
        // Elimina un espacio por su ID
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            var espacio = _espacioService.ObtenerEspacios()
                .FirstOrDefault(e => e.Id == id);

            if (espacio == null)
                return NotFound();

            _espacioService.EliminarEspacio(id);

            return Ok();
        }

        // GET: api/espacios/exportar
        // Exporta los espacios registrados en formato PDF
        [HttpGet]
        [Route("exportar")]
        public HttpResponseMessage ExportarEspaciosPDF()
        {
            var espacios = _espacioService.ObtenerEspacios();

            using (var ms = new MemoryStream())
            {
                // Crea y configura el documento PDF
                Document doc = new Document(PageSize.A4, 50, 50, 25, 25);
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();

                // Agrega título al documento
                var titulo = new Paragraph("Lista de Espacios")
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20f,
                    Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16)
                };
                doc.Add(titulo);

                // Crea una tabla con 4 columnas
                PdfPTable tabla = new PdfPTable(4)
                {
                    WidthPercentage = 100
                };

                // Encabezados de la tabla
                tabla.AddCell("Nombre");
                tabla.AddCell("Tipo");
                tabla.AddCell("Ubicación");
                tabla.AddCell("Capacidad");

                // Agrega cada espacio como fila en la tabla
                foreach (var e in espacios)
                {
                    tabla.AddCell(e.Nombre);
                    tabla.AddCell(e.Tipo);
                    tabla.AddCell(e.Ubicacion);
                    tabla.AddCell(e.Capacidad.ToString());
                }

                // Agrega tabla al documento y cierra
                doc.Add(tabla);
                doc.Close();

                // Devuelve el archivo PDF como respuesta HTTP
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(ms.ToArray())
                };
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = "espacios.pdf"
                };
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");

                return response;
            }
        }
    }
}
