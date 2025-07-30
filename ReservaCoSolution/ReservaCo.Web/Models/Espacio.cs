using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.ComponentModel.DataAnnotations;
using ReservaCo.Domain.Entities;
namespace ReservaCo.Web.Models
{
    public class Espacio
    {
        [Key]
        public int ID_Espacio { get; set; }

        [Required(ErrorMessage = "El nombre del espacio es obligatorio.")]
        public string Nombre { get; set; }  // Ej: Aula 101

        [Required(ErrorMessage = "El tipo de espacio es obligatorio.")]
        public string Tipo { get; set; }  // Ej: Aula, Laboratorio

        [Required(ErrorMessage = "La ubicación es obligatoria.")]
        public string Ubicacion { get; set; }

        [Required(ErrorMessage = "La capacidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La capacidad debe ser mayor que 0.")]
        public int Capacidad { get; set; }

        public DateTime FechaCreacion { get; set; }


        
    }
}
