using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace ReservaCo.Web.Models
{
    public class Reserva
    {
        [Key]
        public int ID_Reserva { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un estado")]
        public string Estado { get; set; } // "Pendiente", "Aprobada", "Rechazada"

        [DataType(DataType.Date)]
        public DateTime FechaCreacion { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un usuario")]
        public int ID_Usuario { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un espacio")]
        public int ID_Espacio { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una fecha")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Debe ingresar la hora de inicio")]
        [DataType(DataType.Time)]
        public TimeSpan HoraInicio { get; set; }

        [Required(ErrorMessage = "Debe ingresar la hora de fin")]
        [DataType(DataType.Time)]
        public TimeSpan HoraFin { get; set; }

        // Propiedades auxiliares opcionales para mostrar nombres en vistas
        public string NombreUsuario { get; set; }
        public string NombreEspacio { get; set; }
    }
}
