// ************************************************************************
// Proyecto 02 
// Aguilar Verónica, Guerrero Luis
// Fecha de realización: 21/07/2025 
// Fecha de entrega: 03/08/2025  
// Resultados:
// - Este controlador proporciona una API RESTful para gestionar usuarios del sistema.
// - Permite operaciones CRUD, autenticación (login) y exportación de datos en PDF.
// - Utiliza el servicio de aplicación 'UsuarioService' y el helper 'UsuarioSesion' para manejar el acceso y sesión.
// ************************************************************************

using System;
using System.Linq;
using System.Web.Http;
using ReservaCo.Application.Services;
using ReservaCo.Infrastructure.Context;
using ReservaCo.Web.Models;
using ReservaCo.Web.Utils;

// PDF exportación
using System.IO;
using System.Net.Http;
using System.Net;
using iTextSharp.text;
using iTextSharp.text.pdf;

// Aliases para evitar ambigüedad entre modelo de dominio y modelo de vista
using EntidadUsuario = ReservaCo.Domain.Entities.Usuario;
using ModeloUsuario = ReservaCo.Web.Models.Usuario;

namespace ReservaCo.Web.Controllers
{
    // Prefijo para todas las rutas de este controlador: api/usuarios
    [RoutePrefix("api/usuarios")]
    public class UsuariosController : ApiController
    {
        // Servicio de aplicación para manejar la lógica de usuarios
        private readonly UsuarioService _usuarioService;

        // Constructor que instancia el contexto y el servicio
        public UsuariosController()
        {
            var context = new ReservaCoDbContext();
            _usuarioService = new UsuarioService(context);
        }

        // GET: api/usuarios
        // Devuelve todos los usuarios registrados
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            var usuarios = _usuarioService.ObtenerUsuarios();

            var modelos = usuarios.Select(u => new ModeloUsuario
            {
                ID_Usuario = u.Id,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Correo = u.Email,
                Contraseña = u.Contrasenia,
                Rol = u.Rol?.Nombre,
                FechaCreacion = u.FechaCreacion
            }).ToList();

            return Ok(modelos);
        }

        // GET: api/usuarios/5
        // Devuelve un usuario específico por ID
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var usuario = _usuarioService.ObtenerUsuarioPorId(id);
            if (usuario == null)
                return NotFound();

            var model = new ModeloUsuario
            {
                ID_Usuario = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Correo = usuario.Email,
                Contraseña = usuario.Contrasenia,
                Rol = usuario.Rol?.Nombre,
                FechaCreacion = usuario.FechaCreacion
            };

            return Ok(model);
        }

        // POST: api/usuarios
        // Crea un nuevo usuario
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post(ModeloUsuario model)
        {
            var rolEntidad = _usuarioService.ObtenerRolPorNombre(model.Rol);
            if (rolEntidad == null)
                return BadRequest("Rol no válido");

            var entidad = new EntidadUsuario
            {
                Nombre = model.Nombre,
                Apellido = model.Apellido,
                Contrasenia = model.Contraseña,
                Rol = rolEntidad,
                FechaCreacion = DateTime.Now
            };

            try
            {
                _usuarioService.CambiarCorreo(entidad, model.Correo);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            _usuarioService.GuardarUsuario(entidad);
            model.ID_Usuario = entidad.Id;
            model.FechaCreacion = entidad.FechaCreacion;

            return Ok(model);
        }

        // PUT: api/usuarios/5
        // Actualiza los datos de un usuario existente
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Put(int id, ModeloUsuario model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = _usuarioService.ObtenerUsuarioPorId(id);
            if (usuario == null)
                return NotFound();

            var rolEntity = _usuarioService.ObtenerRolPorNombre(model.Rol);
            if (rolEntity == null)
                return BadRequest("Rol no válido.");

            usuario.Nombre = model.Nombre;
            usuario.Apellido = model.Apellido;
            usuario.Contrasenia = model.Contraseña;
            usuario.Rol = rolEntity;

            try
            {
                _usuarioService.CambiarCorreo(usuario, model.Correo);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            _usuarioService.GuardarUsuario(usuario);

            return Ok(model);
        }

        // DELETE: api/usuarios/5
        // Elimina un usuario por ID
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            var usuario = _usuarioService.ObtenerUsuarioPorId(id);
            if (usuario == null)
                return NotFound();

            _usuarioService.EliminarUsuario(id);
            return Ok();
        }

        // GET: api/usuarios/exportar
        // Exporta la lista de usuarios a un archivo PDF
        [HttpGet]
        [Route("exportar")]
        public HttpResponseMessage ExportarUsuariosPDF()
        {
            var usuarios = _usuarioService.ObtenerUsuarios();

            using (var ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 50, 50, 25, 25);
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();

                var titulo = new Paragraph("Lista de Usuarios")
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20f,
                    Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16)
                };
                doc.Add(titulo);

                PdfPTable tabla = new PdfPTable(4)
                {
                    WidthPercentage = 100
                };

                tabla.AddCell("Nombre");
                tabla.AddCell("Apellido");
                tabla.AddCell("Correo");
                tabla.AddCell("Rol");

                foreach (var u in usuarios)
                {
                    tabla.AddCell(u.Nombre);
                    tabla.AddCell(u.Apellido);
                    tabla.AddCell(u.Email);
                    tabla.AddCell(u.Rol?.Nombre ?? "-");
                }

                doc.Add(tabla);
                doc.Close();

                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(ms.ToArray())
                };
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = "usuarios.pdf"
                };
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");

                return response;
            }
        }

        // POST: api/usuarios/login
        // Autentica al usuario por correo y contraseña
        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login(LoginRequest request)
        {
            var usuario = _usuarioService.ObtenerUsuarios()
                .FirstOrDefault(u => u.Email.ToLower() == request.Correo.ToLower() &&
                                     u.Contrasenia == request.Contraseña);

            if (usuario == null)
                return Unauthorized();

            // Guarda en sesión (solo en contexto no concurrente)
            UsuarioSesion.UsuarioActual = usuario;

            return Ok(new
            {
                Mensaje = "Inicio de sesión exitoso",
                Usuario = new
                {
                    usuario.Id,
                    usuario.Nombre,
                    usuario.Apellido,
                    usuario.Email,
                    Rol = usuario.Rol?.Nombre
                }
            });
        }

        // GET: api/usuarios/actual
        // Retorna el usuario actualmente autenticado en la sesión
        [HttpGet]
        [Route("actual")]
        public IHttpActionResult ObtenerUsuarioActual()
        {
            if (UsuarioSesion.UsuarioActual == null)
                return Unauthorized();

            var u = UsuarioSesion.UsuarioActual;
            return Ok(new
            {
                u.Id,
                u.Nombre,
                u.Apellido,
                u.Email,
                Rol = u.Rol?.Nombre
            });
        }
    }
}
