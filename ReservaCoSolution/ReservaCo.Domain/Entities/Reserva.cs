// ************************************************************************
// Proyecto 02 
// Aguilar Verónica, Guerrero Luis
// Fecha de realización: 21/07/2025 
// Fecha de entrega: 03/08/2025  
// Resultados:
// - Esta clase representa la entidad 'Reserva', la cual registra las solicitudes de uso de espacios.
// - Almacena información como estado de aprobación, fecha de creación, usuario solicitante, espacio solicitado y horario reservado.
// - Establece relaciones con las entidades Usuario y Espacio, además de permitir gestión horaria mediante composición.
// ************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservaCo.Domain.Entities
{
    // Clase que representa una solicitud de reserva de espacio realizada por un usuario.
    public class Reserva
    {
        // Identificador único de la reserva.
        public int Id { get; set; }

        // Estado actual de la reserva (Pendiente, Aprobada o Rechazada).
        public EstadoReserva Estado { get; set; }

        // Fecha en la que se registró la solicitud de reserva.
        public DateTime FechaCreacion { get; set; }

        // Identificador del usuario que realizó la reserva (clave foránea).
        public int UsuarioId { get; set; }

        // Objeto de navegación que representa al usuario solicitante.
        public Usuario Usuario { get; set; }

        // Identificador del espacio que se desea reservar (clave foránea).
        public int EspacioId { get; set; }

        // Objeto de navegación que representa el espacio solicitado.
        public Espacio Espacio { get; set; }

        // Fecha específica en la que se desea usar el espacio.
        public DateTime Fecha { get; set; }

        // Hora de inicio de uso del espacio.
        public TimeSpan HoraInicio { get; set; }

        // Hora de finalización del uso del espacio.
        public TimeSpan HoraFin { get; set; }
    }
}
