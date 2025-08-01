// ************************************************************************
// Proyecto 02 
// Aguilar Verónica, Guerrero Luis
// Fecha de realización: 21/07/2025 
// Fecha de entrega: 03/08/2025  
// Resultados:
// - Esta clase estática encapsula las reglas de negocio asociadas a la entidad 'Rol'.
// - Se incluyen validaciones estructurales, normalización del nombre y control de duplicados en la creación o edición de roles.
// - Permite separar la lógica de validación del resto del sistema, manteniendo el principio de responsabilidad única.
// ************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReservaCo.Domain.Entities;

namespace ReservaCo.Domain.Business
{
    // Clase estática que contiene reglas de validación y normalización para la entidad Rol.
    public static class RolRules
    {
        // Método para validar que un objeto Rol cumpla con las reglas básicas.
        public static void Validar(Rol rol)
        {
            // Verifica que el objeto rol no sea nulo.
            if (rol == null)
                throw new ArgumentNullException("El rol no puede ser nulo.");

            // Verifica que el nombre no esté vacío o compuesto solo de espacios.
            if (string.IsNullOrWhiteSpace(rol.Nombre))
                throw new ArgumentException("El nombre del rol es obligatorio.");

            // Verifica que el nombre del rol no exceda los 50 caracteres permitidos.
            if (rol.Nombre.Length > 50)
                throw new ArgumentException("El nombre del rol no puede superar los 50 caracteres.");
        }

        // Método para eliminar espacios innecesarios en el nombre del rol.
        public static void NormalizarNombre(Rol rol)
        {
            // Verifica que el nombre no esté vacío antes de aplicar la normalización.
            if (string.IsNullOrWhiteSpace(rol.Nombre))
                throw new ArgumentException("El nombre del rol no puede estar vacío.");

            // Elimina espacios al inicio y final del nombre.
            rol.Nombre = rol.Nombre.Trim();
        }

        // Método para asegurar que el nombre del nuevo rol no se repita entre los ya existentes.
        public static void ValidarNombreUnico(Rol nuevoRol, List<Rol> existentes)
        {
            // Compara ignorando mayúsculas/minúsculas si existe otro rol con el mismo nombre.
            if (existentes.Any(r => r.Nombre.Equals(nuevoRol.Nombre, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Ya existe un rol con el mismo nombre.");
        }
    }
}
