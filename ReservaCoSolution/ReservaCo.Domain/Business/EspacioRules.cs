using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReservaCo.Domain.Entities;

namespace ReservaCo.Domain.Business
{
    public static class EspacioRules
    {
        public static void Validar(Espacio espacio)
        {
            if (espacio == null)
                throw new ArgumentNullException("El espacio no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(espacio.Nombre))
                throw new ArgumentException("El nombre del espacio es obligatorio.");

            if (espacio.Capacidad <= 0)
                throw new ArgumentException("La capacidad del espacio debe ser mayor a cero.");

            if (string.IsNullOrWhiteSpace(espacio.Tipo))
                throw new ArgumentException("El tipo del espacio es obligatorio.");

            if (string.IsNullOrWhiteSpace(espacio.Ubicacion))
                throw new ArgumentException("La ubicación del espacio es obligatoria.");
        }

        public static void ValidarNombreUnico(Espacio nuevoEspacio, List<Espacio> existentes)
        {
            if (existentes.Any(e => e.Nombre.Trim().ToLower() == nuevoEspacio.Nombre.Trim().ToLower()))
                throw new InvalidOperationException("Ya existe un espacio con el mismo nombre.");
        }

        public static void ValidarCapacidad(Espacio espacio)
        {
            if (espacio.Capacidad <= 0)
                throw new ArgumentException("La capacidad debe ser mayor que cero.");
        }

        public static void FormatearNombre(Espacio espacio)
        {
            if (!string.IsNullOrWhiteSpace(espacio.Nombre))
                espacio.Nombre = espacio.Nombre.Trim();
        }
    }
}

