// ************************************************************************
// Proyecto 02 
// Aguilar Verónica, Guerrero Luis
// Fecha de realización: 21/07/2025 
// Fecha de entrega: 03/08/2025  
// Resultados:
// - Este modelo representa la entidad 'Usuario' en la capa Web para la interacción con vistas o formularios.
// - Incluye validaciones mediante anotaciones de data annotations para garantizar consistencia y formato correcto de los datos ingresados.
// - Se utiliza para registrar, visualizar o editar usuarios dentro del sistema desde el frontend.
// ************************************************************************

using System;
using System.ComponentModel.DataAnnotations;

namespace ReservaCo.Web.Models
{
    // Modelo de vista utilizado para representar un usuario en formularios y vistas del frontend.
    public class Usuario
    {
        // Identificador único del usuario (clave primaria).
        [Key]
        public int ID_Usuario { get; set; }

        // Nombre del usuario. Campo obligatorio.
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        // Apellido del usuario. Campo obligatorio.
        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string Apellido { get; set; }

        // Correo electrónico del usuario. Campo obligatorio y validado como formato de email.
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido.")]
        public string Correo { get; set; }

        // Contraseña del usuario. Campo obligatorio.
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Contraseña { get; set; }

        // Rol asignado al usuario. Campo obligatorio (ej. Profesor, Administrador).
        [Required(ErrorMessage = "El rol es obligatorio.")]
        public string Rol { get; set; }

        // Fecha en la que se registró el usuario en el sistema.
        public DateTime FechaCreacion { get; set; }
    }
}
