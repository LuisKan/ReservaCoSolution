// ************************************************************************
// Proyecto 02 
// Aguilar Verónica, Guerrero Luis
// Fecha de realización: 21/07/2025 
// Fecha de entrega: 03/08/2025  
// Resultados:
// - Este modelo representa la entidad 'Espacio' en la capa Web (presentación).
// - Permite capturar y validar datos de espacios físicos a través de formularios.
// - Usa anotaciones de data annotations para garantizar que los campos requeridos y los valores ingresados sean válidos.
// ************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ReservaCo.Domain.Entities;

namespace ReservaCo.Web.Models
{
    // Modelo de vista que representa un espacio (aula, laboratorio, etc.) para la capa web.
    public class Espacio
    {
        // Identificador único del espacio (clave primaria).
        [Key]
        public int ID_Espacio { get; set; }

        // Nombre del espacio, obligatorio para identificarlo (ej. Aula 101).
        [Required(ErrorMessage = "El nombre del espacio es obligatorio.")]
        public string Nombre { get; set; }

        // Tipo de espacio, requerido para clasificarlo (ej. Aula, Laboratorio).
        [Required(ErrorMessage = "El tipo de espacio es obligatorio.")]
        public string Tipo { get; set; }

        // Ubicación física del espacio, también obligatoria.
        [Required(ErrorMessage = "La ubicación es obligatoria.")]
        public string Ubicacion { get; set; }

        // Capacidad máxima permitida del espacio, con validación de rango mínimo.
        [Required(ErrorMessage = "La capacidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La capacidad debe ser mayor que 0.")]
        public int Capacidad { get; set; }

        // Fecha en la que se creó o registró el espacio en el sistema.
        public DateTime FechaCreacion { get; set; }
    }
}
