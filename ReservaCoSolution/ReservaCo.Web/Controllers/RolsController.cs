// ************************************************************************
// Proyecto 02 
// Aguilar Verónica, Guerrero Luis
// Fecha de realización: 21/07/2025 
// Fecha de entrega: 03/08/2025  
// Resultados:
// - Este controlador expone una API RESTful para gestionar roles en el sistema.
// - Permite operaciones CRUD completas (crear, consultar, editar y eliminar roles).
// - Incluye una funcionalidad adicional para exportar los datos de roles a PDF mediante iTextSharp.
// ************************************************************************

using System.Linq;
using System.Web.Http;
using ReservaCo.Application.Service;
using ReservaCo.Application.Services;
using ReservaCo.Infrastructure.Context;

// Evita conflicto entre entidad de dominio y modelo de vista
using EntidadRol = ReservaCo.Domain.Entities.Rol;
using ModeloRol = ReservaCo.Web.Models.Rol;

using System.IO;
using System.Net.Http;
using System.Net;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ReservaCo.Web.Controllers
{
    // Define el prefijo base para todas las rutas de este controlador: api/rols
    [RoutePrefix("api/rols")]
    public class RolsController : ApiController
    {
        // Servicio de aplicación encargado de gestionar roles.
        private readonly RolService _rolService;

        // Constructor que inicializa el servicio con un nuevo contexto de base de datos.
        public RolsController()
        {
            var context = new ReservaCoDbContext();
            _rolService = new RolService(context);
        }

        // GET: api/rols
        // Devuelve la lista de todos los roles.
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            var roles = _rolService.ObtenerRoles();

            // Mapea las entidades a modelos de vista.
            var modelos = roles.Select(r => new ModeloRol
            {
                ID_Rol = r.Id,
                Nombre = r.Nombre
            }).ToList();

            return Ok(modelos);
        }

        // GET: api/rols/5
        // Devuelve un rol específico por su ID.
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var rol = _rolService.ObtenerRolPorId(id);
            if (rol == null)
                return NotFound();

            // Mapea la entidad a modelo.
            var model = new ModeloRol
            {
                ID_Rol = rol.Id,
                Nombre = rol.Nombre
            };

            return Ok(model);
        }

        // POST: api/rols
        // Crea un nuevo rol.
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post(ModeloRol model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Mapea modelo a entidad.
            var entidad = new EntidadRol
            {
                Nombre = model.Nombre
            };

            _rolService.GuardarRol(entidad);

            // Retorna el ID asignado tras guardar.
            model.ID_Rol = entidad.Id;

            return Ok(model);
        }

        // PUT: api/rols/5
        // Actualiza un rol existente por ID.
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Put(int id, ModeloRol model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var rol = _rolService.ObtenerRolPorId(id);
            if (rol == null)
                return NotFound();

            // Aplica cambios sobre la entidad.
            rol.Nombre = model.Nombre;
            _rolService.GuardarRol(rol);

            return Ok(model);
        }

        // DELETE: api/rols/5
        // Elimina un rol por su ID.
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            var rol = _rolService.ObtenerRolPorId(id);
            if (rol == null)
                return NotFound();

            _rolService.EliminarRol(id);
            return Ok();
        }

        // GET: api/rols/exportar
        // Exporta todos los roles a un archivo PDF.
        [HttpGet]
        [Route("exportar")]
        public HttpResponseMessage ExportarRolesPDF()
        {
            var roles = _rolService.ObtenerRoles();

            using (var ms = new MemoryStream())
            {
                // Configura el documento PDF (tamaño A4 y márgenes).
                Document doc = new Document(PageSize.A4, 50, 50, 25, 25);
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();

                // Título del documento.
                var titulo = new Paragraph("Lista de Roles")
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20f,
                    Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16)
                };
                doc.Add(titulo);

                // Crea una tabla con 2 columnas.
                PdfPTable tabla = new PdfPTable(2)
                {
                    WidthPercentage = 100
                };

                // Agrega los encabezados de la tabla.
                tabla.AddCell("ID");
                tabla.AddCell("Nombre");

                // Agrega cada fila de datos.
                foreach (var r in roles)
                {
                    tabla.AddCell(r.Id.ToString());
                    tabla.AddCell(r.Nombre);
                }

                // Agrega la tabla al documento y lo cierra.
                doc.Add(tabla);
                doc.Close();

                // Devuelve el PDF como archivo adjunto.
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(ms.ToArray())
                };
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = "roles.pdf"
                };
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");

                return response;
            }
        }
    }
}
