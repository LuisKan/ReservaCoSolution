// ************************************************************************
// Proyecto 02 
// Aguilar Verónica, Guerrero Luis
// Fecha de realización: 21/07/2025 
// Fecha de entrega: 03/08/2025  
// Resultados:
// - Esta clase define reglas de validación y normalización para la entidad 'Espacio'.
// - Verifica que los datos esenciales como nombre, capacidad, tipo y ubicación sean válidos.
// - Permite controlar duplicados, asegurar integridad en la capacidad y estandarizar nombres.
// ************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReservaCo.Domain.Entities;

namespace ReservaCo.Domain.Business
{
    // Clase estática que agrupa las reglas de negocio para la entidad Espacio.
    public static class EspacioRules
    {
        // Método que valida la integridad básica de los datos de un espacio.
        public static void Validar(Espacio espacio)
        {
            // Verifica que el objeto no sea nulo.
            if (espacio == null)
                throw new ArgumentNullException("El espacio no puede ser nulo.");

            // Valida que el nombre no esté vacío ni en blanco.
            if (string.IsNullOrWhiteSpace(espacio.Nombre))
                throw new ArgumentException("El nombre del espacio es obligatorio.");

            // Valida que la capacidad sea un número mayor a cero.
            if (espacio.Capacidad <= 0)
                throw new ArgumentException("La capacidad del espacio debe ser mayor a cero.");

            // Valida que el tipo del espacio haya sido definido.
            if (string.IsNullOrWhiteSpace(espacio.Tipo))
                throw new ArgumentException("El tipo del espacio es obligatorio.");

            // Verifica que se haya proporcionado una ubicación válida.
            if (string.IsNullOrWhiteSpace(espacio.Ubicacion))
                throw new ArgumentException("La ubicación del espacio es obligatoria.");
        }

        // Verifica que el nombre del nuevo espacio no se repita en la lista existente.
        public static void ValidarNombreUnico(Espacio nuevoEspacio, List<Espacio> existentes)
        {
            // Compara los nombres ignorando mayúsculas/minúsculas y espacios.
            if (existentes.Any(e => e.Nombre.Trim().ToLower() == nuevoEspacio.Nombre.Trim().ToLower()))
                throw new InvalidOperationException("Ya existe un espacio con el mismo nombre.");
        }

        // Valida específicamente que la capacidad del espacio sea válida (> 0).
        public static void ValidarCapacidad(Espacio espacio)
        {
            if (espacio.Capacidad <= 0)
                throw new ArgumentException("La capacidad debe ser mayor que cero.");
        }

        // Elimina espacios innecesarios del nombre del espacio para mantener consistencia.
        public static void FormatearNombre(Espacio espacio)
        {
            if (!string.IsNullOrWhiteSpace(espacio.Nombre))
                espacio.Nombre = espacio.Nombre.Trim();
        }
    }
}
