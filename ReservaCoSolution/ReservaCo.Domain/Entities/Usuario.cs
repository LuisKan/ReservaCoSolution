// ************************************************************************
// Proyecto 02 
// Aguilar Verónica, Guerrero Luis
// Fecha de realización: 21/07/2025 
// Fecha de entrega: 03/08/2025  
// Resultados:
// - Esta clase define la entidad de dominio 'Usuario', la cual representa a cada persona registrada en el sistema.
// - Contiene información personal y credenciales de acceso como nombre, correo y contraseña.
// - Establece la relación con el rol que posee y las reservas realizadas por el usuario.
// ************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservaCo.Domain.Entities
{
    // Clase que representa un usuario del sistema (profesor, administrador, etc.)
    public class Usuario
    {
        // Identificador único del usuario.
        public int Id { get; set; }

        // Nombre de pila del usuario.
        public string Nombre { get; set; }

        // Apellido del usuario.
        public string Apellido { get; set; }

        // Correo electrónico del usuario. Sirve como credencial de acceso.
        public string Email { get; set; }

        // Contraseña cifrada o en texto plano (recomendado usar hash en producción).
        public string Contrasenia { get; set; }

        // Fecha en la que fue creado el usuario dentro del sistema.
        public DateTime FechaCreacion { get; set; }

        // Identificador del rol que tiene asignado el usuario (relación foránea).
        public int RolId { get; set; }

        // Objeto de navegación que representa el rol del usuario.
        public Rol Rol { get; set; }

        // Colección de reservas que ha realizado este usuario.
        public ICollection<Reserva> Reservas { get; set; }
    }
}
