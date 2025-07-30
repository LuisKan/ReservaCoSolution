using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReservaCo.Domain.Entities;

namespace ReservaCo.Domain.Business
{
    public static class RolRules
    {
        public static void Validar(Rol rol)
        {
            if (rol == null)
                throw new ArgumentNullException("El rol no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(rol.Nombre))
                throw new ArgumentException("El nombre del rol es obligatorio.");

            if (rol.Nombre.Length > 50)
                throw new ArgumentException("El nombre del rol no puede superar los 50 caracteres.");
        }

        public static void NormalizarNombre(Rol rol)
        {
            if (string.IsNullOrWhiteSpace(rol.Nombre))
                throw new ArgumentException("El nombre del rol no puede estar vacío.");

            rol.Nombre = rol.Nombre.Trim();
        }

        // Valida que no haya duplicados en los nombres (exceptuando el mismo ID)
        public static void ValidarNombreUnico(Rol nuevoRol, List<Rol> existentes)
        {
            if (existentes.Any(r => r.Nombre.Equals(nuevoRol.Nombre, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Ya existe un rol con el mismo nombre.");
        }
    }
}

