using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReservaCo.Domain.Entities;

namespace ReservaCo.Domain.Business
{
    public static class ReservaRules
    {
        public static void Aprobar(Reserva reserva)
        {
            if (reserva.Estado != EstadoReserva.Pendiente)
                throw new InvalidOperationException("Solo se puede aprobar una reserva pendiente.");

            reserva.Estado = EstadoReserva.Aprobada;
        }

        public static void Rechazar(Reserva reserva)
        {
            if (reserva.Estado != EstadoReserva.Pendiente)
                throw new InvalidOperationException("Solo se puede rechazar una reserva pendiente.");

            reserva.Estado = EstadoReserva.Rechazada;
        }

        public static void ValidarHoras(TimeSpan horaInicio, TimeSpan horaFin)
        {
            if (horaInicio >= horaFin)
                throw new ArgumentException("La hora de inicio debe ser anterior a la hora de fin.");
        }


        public static void ValidarFecha(DateTime fecha)
        {
            if (fecha.Date < DateTime.Today)
                throw new ArgumentException("No se puede reservar en fechas pasadas.");
        }

        public static void ValidarDisponibilidad(bool estaDisponible)
        {
            if (!estaDisponible)
                throw new InvalidOperationException("El horario seleccionado no está disponible. Ya existe una reserva que se cruza con este horario.");
        }


        public static void EstablecerEstadoPorDefecto(Reserva reserva)
        {
            if (reserva.Estado == 0) // Estado no definido
                reserva.Estado = EstadoReserva.Pendiente;
        }
    }
}


