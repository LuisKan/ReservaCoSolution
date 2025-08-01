// ************************************************************************
// Proyecto 02 
// Aguilar Verónica, Guerrero Luis
// Fecha de realización: 21/07/2025 
// Fecha de entrega: 03/08/2025  
// Resultados:
// - Este servicio gestiona la lógica de aplicación para la entidad 'Reserva'.
// - Permite crear, validar, modificar, eliminar, aprobar o rechazar reservas de espacios.
// - Integra reglas para control de horarios superpuestos, disponibilidad y estados de reserva.
// ************************************************************************

using System;
using System.Linq;
using ReservaCo.Domain.Entities;
using ReservaCo.Domain.Business;
using ReservaCo.Infrastructure.Context;
using System.Collections.Generic;
using System.Data.Entity;

namespace ReservaCo.Application.Services
{
    // Servicio de aplicación que maneja la lógica relacionada con reservas de espacios.
    public class ReservaService
    {
        // Contexto de acceso a base de datos.
        private readonly ReservaCoDbContext _context;

        // Constructor que recibe el contexto mediante inyección de dependencias.
        public ReservaService(ReservaCoDbContext context)
        {
            _context = context;
        }

        // Retorna todas las reservas, incluyendo la información del usuario y el espacio reservado.
        public List<Reserva> ObtenerTodasLasReservas()
        {
            return _context.Reservas
                .Include(r => r.Usuario) // Carga relacionada con Usuario
                .Include(r => r.Espacio) // Carga relacionada con Espacio
                .ToList();
        }

        // Obtiene una reserva específica por su identificador, incluyendo relaciones.
        public Reserva ObtenerReservaPorId(int id)
        {
            return _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Espacio)
                .FirstOrDefault(r => r.Id == id);
        }

        // Crea o actualiza una reserva después de realizar validaciones.
        public void GuardarReserva(Reserva reserva)
        {
            // Valida que la fecha no sea anterior a la actual.
            ReservaRules.ValidarFecha(reserva.Fecha);

            // Valida que la hora de inicio sea menor que la de fin.
            ReservaRules.ValidarHoras(reserva.HoraInicio, reserva.HoraFin);

            // Verifica si existe traslape de horarios en el mismo espacio y fecha.
            var existeTraslape = _context.Reservas.Any(r =>
                r.Espacio.Id == reserva.Espacio.Id &&
                r.Fecha == reserva.Fecha &&
                r.Id != reserva.Id && // Evita comparar contra sí mismo
                (
                    (reserva.HoraInicio >= r.HoraInicio && reserva.HoraInicio < r.HoraFin) || // Comienza durante otra
                    (reserva.HoraFin > r.HoraInicio && reserva.HoraFin <= r.HoraFin) ||       // Termina durante otra
                    (reserva.HoraInicio <= r.HoraInicio && reserva.HoraFin >= r.HoraFin)      // Cubre completamente otra
                )
            );

            // Valida que el horario esté disponible (sin traslapes).
            ReservaRules.ValidarDisponibilidad(!existeTraslape);

            // Establece estado por defecto si aún no ha sido definido.
            ReservaRules.EstablecerEstadoPorDefecto(reserva);

            // Agrega o actualiza la reserva según su ID.
            if (reserva.Id == 0)
                _context.Reservas.Add(reserva);
            else
                _context.Entry(reserva).State = System.Data.Entity.EntityState.Modified;

            // Persiste los cambios en la base de datos.
            _context.SaveChanges();
        }

        // Elimina una reserva según su ID si existe en la base de datos.
        public void EliminarReserva(int id)
        {
            var reserva = _context.Reservas.Find(id);
            if (reserva != null)
            {
                _context.Reservas.Remove(reserva);
                _context.SaveChanges();
            }
        }

        // Verifica si un espacio está disponible para una fecha y horario específicos.
        public bool ValidarDisponibilidad(DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin, int espacioId)
        {
            return !_context.Reservas.Any(r =>
                r.EspacioId == espacioId &&
                r.Fecha == fecha &&
                (
                    (horaInicio >= r.HoraInicio && horaInicio < r.HoraFin) ||
                    (horaFin > r.HoraInicio && horaFin <= r.HoraFin)
                )
            );
        }

        // Cambia el estado de la reserva a "Aprobada" si está pendiente.
        public void AprobarReserva(int id)
        {
            var reserva = _context.Reservas.Find(id);
            if (reserva != null)
            {
                ReservaRules.Aprobar(reserva);
                _context.SaveChanges();
            }
        }

        // Cambia el estado de la reserva a "Rechazada" si está pendiente.
        public void RechazarReserva(int id)
        {
            var reserva = _context.Reservas.Find(id);
            if (reserva != null)
            {
                ReservaRules.Rechazar(reserva);
                _context.SaveChanges();
            }
        }
    }
}
