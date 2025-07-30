using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservaCo.Domain.Entities
{
    public class Reserva
    {
        public int Id { get; set; }
        public EstadoReserva Estado { get; set; }
        public DateTime FechaCreacion { get; set; }

        // Relaciones
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public int EspacioId { get; set; }
        public Espacio Espacio { get; set; }

        // Horario (composición)
        public DateTime Fecha { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
    }


}

