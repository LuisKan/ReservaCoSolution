using System;
using System.Linq;
using ReservaCo.Domain.Entities;
using ReservaCo.Domain.Business; 
using ReservaCo.Infrastructure.Context;
using System.Collections.Generic;
using System.Data.Entity;



namespace ReservaCo.Application.Services
{
    public class ReservaService
    {
        private readonly ReservaCoDbContext _context;

        public ReservaService(ReservaCoDbContext context)
        {
            _context = context;
        }

        public List<Reserva> ObtenerTodasLasReservas()
        {
            return _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Espacio)
                .ToList();
        }


        public Reserva ObtenerReservaPorId(int id)
        {
            return _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Espacio)
                .FirstOrDefault(r => r.Id == id);
        }

        public void GuardarReserva(Reserva reserva)
        {
            ReservaRules.ValidarFecha(reserva.Fecha);
            ReservaRules.ValidarHoras(reserva.HoraInicio, reserva.HoraFin);

            // Verificar si ya existe una reserva en el mismo espacio, fecha y horario superpuesto
            var existeTraslape = _context.Reservas.Any(r =>
                r.Espacio.Id == reserva.Espacio.Id &&
                r.Fecha == reserva.Fecha &&
                r.Id != reserva.Id && // evitar compararse consigo mismo
                (
                    (reserva.HoraInicio >= r.HoraInicio && reserva.HoraInicio < r.HoraFin) || // empieza durante otra reserva
                    (reserva.HoraFin > r.HoraInicio && reserva.HoraFin <= r.HoraFin) ||       // termina durante otra reserva
                    (reserva.HoraInicio <= r.HoraInicio && reserva.HoraFin >= r.HoraFin)      // cubre completamente otra reserva
                )
            );

            ReservaRules.ValidarDisponibilidad(!existeTraslape);
            ReservaRules.EstablecerEstadoPorDefecto(reserva);

            if (reserva.Id == 0)
                _context.Reservas.Add(reserva);
            else
                _context.Entry(reserva).State = System.Data.Entity.EntityState.Modified;

            _context.SaveChanges();
        }


        public void EliminarReserva(int id)
        {
            var reserva = _context.Reservas.Find(id);
            if (reserva != null)
            {
                _context.Reservas.Remove(reserva);
                _context.SaveChanges();
            }
        }


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

        public void AprobarReserva(int id)
        {
            var reserva = _context.Reservas.Find(id);
            if (reserva != null)
            {
                ReservaRules.Aprobar(reserva); // ← Aplicar lógica de dominio
                _context.SaveChanges();
            }
        }

        public void RechazarReserva(int id)
        {
            var reserva = _context.Reservas.Find(id);
            if (reserva != null)
            {
                ReservaRules.Rechazar(reserva); // ← Aplicar lógica de dominio
                _context.SaveChanges();
            }
        }

    }
}
