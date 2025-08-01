// ************************************************************************
// Proyecto 02 
// Aguilar Verónica, Guerrero Luis
// Fecha de realización: 21/07/2025 
// Fecha de entrega: 03/08/2025  
// Resultados:
// - Esta clase representa la entidad de dominio 'Rol', que define los tipos de roles disponibles en el sistema.
// - Cada rol tiene un identificador único y un nombre descriptivo (por ejemplo: Profesor, Administrador, Coordinador).
// - Se utiliza para la gestión de permisos y control de acceso dentro de la aplicación.
// ************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservaCo.Domain.Entities
{
    // Clase que define la estructura de la entidad Rol en la capa de dominio.
    public class Rol
    {
        // Propiedad que representa el identificador único del rol.
        public int Id { get; set; }

        // Propiedad que almacena el nombre del rol (ej. Profesor, Administrador, Coordinador).
        public string Nombre { get; set; }
    }
}
