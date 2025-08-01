// ************************************************************************
// Proyecto 02 
// Aguilar Verónica, Guerrero Luis
// Fecha de realización: 21/07/2025 
// Fecha de entrega: 03/08/2025  
// Resultados:
// - Este modelo representa la entidad 'Rol' en la capa de presentación (Web).
// - Se utiliza para la validación de datos en formularios y vistas del frontend.
// - Incluye anotaciones de validación como [Required] y [StringLength] para garantizar entrada válida del usuario.
// ************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ReservaCo.Web.Models
{
    // Modelo de vista utilizado para capturar y validar datos de roles en el frontend.
    public class Rol
    {
        // Identificador único del rol (clave primaria).
        [Key]
        public int ID_Rol { get; set; }

        // Nombre del rol (obligatorio y con longitud máxima de 50 caracteres).
        [Required(ErrorMessage = "Debe ingresar el nombre del rol")]
        [StringLength(50)]
        public string Nombre { get; set; } // Ej: Profesor, Administrador, Coordinador
    }
}
