// ************************************************************************
// Proyecto 02 
// Aguilar Verónica, Guerrero Luis
// Fecha de realización: 21/07/2025 
// Fecha de entrega: 03/08/2025  
// Resultados:
// - Este modelo representa una reserva de espacio en la capa Web, utilizado para interacción con vistas o formularios.
// - Valida datos como usuario, espacio, fecha, horario y estado, garantizando su integridad antes de enviarse al backend.
// - Incluye propiedades auxiliares para facilitar la visualización de datos relacionados en la interfaz.
// ************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ReservaCo.Web.Models
{
    // Modelo de vista utilizado para representar una reserva desde la capa de presentación.
    public class Reserva
    {
        // Identificador único de la reserva (clave primaria).
        [Key]
        public int ID_Reserva { get; set; }

        // Estado actual de la reserva. Debe seleccionarse entre valores válidos ("Pendiente", "Aprobada", "Rechazada").
        [Required(ErrorMessage = "Debe seleccionar un estado")]
        public string Estado { get; set; }

        // Fecha en la que fue creada la reserva. Mostrada como tipo fecha en formularios.
        [DataType(DataType.Date)]
        public DateTime FechaCreacion { get; set; }

        // Identificador del usuario que realiza la reserva. Campo obligatorio.
        [Required(ErrorMessage = "Debe seleccionar un usuario")]
        public int ID_Usuario { get; set; }

        // Identificador del espacio que se desea reservar. Campo obligatorio.
        [Required(ErrorMessage = "Debe seleccionar un espacio")]
        public int ID_Espacio { get; set; }

        // Fecha específica en la que se quiere utilizar el espacio. Campo obligatorio.
        [Required(ErrorMessage = "Debe seleccionar una fecha")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        // Hora de inicio de uso del espacio. Campo obligatorio y validado como hora.
        [Required(ErrorMessage = "Debe ingresar la hora de inicio")]
        [DataType(DataType.Time)]
        public TimeSpan HoraInicio { get; set; }

        // Hora de fin del uso del espacio. Campo obligatorio y validado como hora.
        [Required(ErrorMessage = "Debe ingresar la hora de fin")]
        [DataType(DataType.Time)]
        public TimeSpan HoraFin { get; set; }

        // Propiedad auxiliar para mostrar el nombre del usuario en la vista.
        public string NombreUsuario { get; set; }

        // Propiedad auxiliar para mostrar el nombre del espacio en la vista.
        public string NombreEspacio { get; set; }
    }
}
