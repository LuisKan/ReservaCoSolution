using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservaCo.Domain.Entities
{
    public class Espacio
    {
        public int Id { get; set; }
        public string Nombre { get; set; }  // Aula, Laboratorio, etc.
        public string Tipo { get; set; }
        public string Ubicacion { get; set; }
        public int Capacidad { get; set; }
        public DateTime FechaCreacion { get; set; }

        public ICollection<Reserva> Reservas { get; set; }
    }
}

