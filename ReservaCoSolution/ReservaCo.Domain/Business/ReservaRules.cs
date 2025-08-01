// ************************************************************************
// Proyecto 02 
// Aguilar Verónica, Guerrero Luis
// Fecha de realización: 21/07/2025 
// Fecha de entrega: 03/08/2025  
// Resultados:
// - Esta clase contiene las reglas de negocio relacionadas con la gestión de reservas.
// - Permite aprobar o rechazar solicitudes, validar fechas y horarios, verificar disponibilidad y establecer estado por defecto.
// - Mejora la consistencia del sistema al aplicar lógicas de control centralizadas y reutilizables.
// ************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReservaCo.Domain.Entities;

namespace ReservaCo.Domain.Business
{
    // Clase que agrupa reglas de negocio asociadas a la entidad Reserva.
    public static class ReservaRules
    {
        // Método para aprobar una reserva solo si se encuentra en estado Pendiente.
        public static void Aprobar(Reserva reserva)
        {
            // Verifica que la reserva esté en estado Pendiente antes de aprobarla.
            if (reserva.Estado != EstadoReserva.Pendiente)
                throw new InvalidOperationException("Solo se puede aprobar una reserva pendiente.");

            // Cambia el estado a Aprobada.
            reserva.Estado = EstadoReserva.Aprobada;
        }

        // Método para rechazar una reserva solo si se encuentra en estado Pendiente.
        public static void Rechazar(Reserva reserva)
        {
            // Verifica que la reserva esté en estado Pendiente antes de rechazarla.
            if (reserva.Estado != EstadoReserva.Pendiente)
                throw new InvalidOperationException("Solo se puede rechazar una reserva pendiente.");

            // Cambia el estado a Rechazada.
            reserva.Estado = EstadoReserva.Rechazada;
        }

        // Valida que la hora de inicio sea anterior a la hora de fin.
        public static void ValidarHoras(TimeSpan horaInicio, TimeSpan horaFin)
        {
            if (horaInicio >= horaFin)
                throw new ArgumentException("La hora de inicio debe ser anterior a la hora de fin.");
        }

        // Valida que la fecha ingresada no sea una fecha pasada.
        public static void ValidarFecha(DateTime fecha)
        {
            if (fecha.Date < DateTime.Today)
                throw new ArgumentException("No se puede reservar en fechas pasadas.");
        }

        // Valida si el espacio está disponible para el horario solicitado.
        public static void ValidarDisponibilidad(bool estaDisponible)
        {
            if (!estaDisponible)
                throw new InvalidOperationException("El horario seleccionado no está disponible. Ya existe una reserva que se cruza con este horario.");
        }

        // Establece el estado Pendiente como valor por defecto si no se ha definido uno.
        public static void EstablecerEstadoPorDefecto(Reserva reserva)
        {
            if (reserva.Estado == 0) // Estado no definido
                reserva.Estado = EstadoReserva.Pendiente;
        }
    }
}
