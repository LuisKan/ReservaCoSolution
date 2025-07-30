using System;
using System.Linq;
using System.Web.Http;
using ReservaCo.Application.Services;
using ReservaCo.Infrastructure.Context;

//------//
using System.IO;
using System.Net.Http;
using System.Net;
using iTextSharp.text;
using iTextSharp.text.pdf;
//------//

// Evita conflicto con la entidad
using EntidadUsuario = ReservaCo.Domain.Entities.Usuario;
using ModeloUsuario = ReservaCo.Web.Models.Usuario;

namespace ReservaCo.Web.Controllers
{
    [RoutePrefix("api/usuarios")]
    public class UsuariosController : ApiController
    {
        private readonly UsuarioService _usuarioService;


        public UsuariosController()
        {
            var context = new ReservaCoDbContext();
            _usuarioService = new UsuarioService(context);
        }

        // GET: api/usuarios
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
                Rol = u.Rol?.Nombre, // ✅ Obtiene solo el nombre
                FechaCreacion = u.FechaCreacion
            }).ToList();

            return Ok(modelos);
        }

        // GET: api/usuarios/5
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

        //--------------------------//

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

                // Título
                var titulo = new Paragraph("Lista de Usuarios")
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20f,
                    Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16)
                };
                doc.Add(titulo);

                // Tabla con 4 columnas
                PdfPTable tabla = new PdfPTable(4)
                {
                    WidthPercentage = 100
                };

                // Encabezados
                tabla.AddCell("Nombre");
                tabla.AddCell("Apellido");
                tabla.AddCell("Correo");
                tabla.AddCell("Rol");

                // Cuerpo
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

    }
}
