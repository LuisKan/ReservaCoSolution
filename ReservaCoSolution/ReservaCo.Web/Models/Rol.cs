using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace ReservaCo.Web.Models
{
    public class Rol
    {
        [Key]
        public int ID_Rol { get; set; }

        [Required(ErrorMessage = "Debe ingresar el nombre del rol")]
        [StringLength(50)]
        public string Nombre { get; set; } // Ej: Profesor, Administrador, Coordinador
    }
}
