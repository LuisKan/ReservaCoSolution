using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservaCo.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Contrasenia { get; set; }
        public DateTime FechaCreacion { get; set; }

        // Relaciones
        public int RolId { get; set; }
        public Rol Rol { get; set; }

        public ICollection<Reserva> Reservas { get; set; }
    }
}

