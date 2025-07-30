using System.Linq;
using System.Web.Http;
using ReservaCo.Application.Service;
using ReservaCo.Application.Services;
using ReservaCo.Infrastructure.Context;

// Evita conflicto con la entidad del dominio
using EntidadRol = ReservaCo.Domain.Entities.Rol;
using ModeloRol = ReservaCo.Web.Models.Rol;


using System.IO;
using System.Net.Http;
using System.Net;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ReservaCo.Web.Controllers
{
    [RoutePrefix("api/rols")]
    public class RolsController : ApiController
    {
        private readonly RolService _rolService;

        public RolsController()
        {
            var context = new ReservaCoDbContext();
           _rolService = new RolService(context);
        }

        // GET: api/rols
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            var roles =_rolService.ObtenerRoles();
            var modelos = roles.Select(r => new ModeloRol
            {
                ID_Rol = r.Id,
                Nombre = r.Nombre
            }).ToList();

            return Ok(modelos);
        }

        // GET: api/rols/5
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var rol =_rolService.ObtenerRolPorId(id);
            if (rol == null)
                return NotFound();

            var model = new ModeloRol
            {
                ID_Rol = rol.Id,
                Nombre = rol.Nombre
            };

            return Ok(model);
        }

        // POST: api/rols
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post(ModeloRol model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entidad = new EntidadRol
            {
                Nombre = model.Nombre
            };

           _rolService.GuardarRol(entidad);
            model.ID_Rol = entidad.Id;

            return Ok(model);
        }

        // PUT: api/rols/5
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Put(int id, ModeloRol model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var rol =_rolService.ObtenerRolPorId(id);
            if (rol == null)
                return NotFound();

            rol.Nombre = model.Nombre;
           _rolService.GuardarRol(rol);

            return Ok(model);
        }

        // DELETE: api/rols/5
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            var rol =_rolService.ObtenerRolPorId(id);
            if (rol == null)
                return NotFound();

           _rolService.EliminarRol(id);
            return Ok();
        }

        // GET: api/rols/exportar
        [HttpGet]
        [Route("exportar")]
        public HttpResponseMessage ExportarRolesPDF()
        {
            var roles = _rolService.ObtenerRoles();

            using (var ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 50, 50, 25, 25);
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();

                // Título
                var titulo = new Paragraph("Lista de Roles")
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20f,
                    Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16)
                };
                doc.Add(titulo);

                // Tabla con 2 columnas
                PdfPTable tabla = new PdfPTable(2)
                {
                    WidthPercentage = 100
                };

                // Encabezados
                tabla.AddCell("ID");
                tabla.AddCell("Nombre");

                // Cuerpo
                foreach (var r in roles)
                {
                    tabla.AddCell(r.Id.ToString());
                    tabla.AddCell(r.Nombre);
                }

                doc.Add(tabla);
                doc.Close();

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
