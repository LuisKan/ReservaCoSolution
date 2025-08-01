// ************************************************************************
// Proyecto 02 
// Aguilar Verónica, Guerrero Luis
// Fecha de realización: 21/07/2025 
// Fecha de entrega: 03/08/2025  
// Resultados:
// - Esta clase implementa la lógica de aplicación para la gestión de espacios (aulas, laboratorios, etc.).
// - Permite registrar, consultar y eliminar espacios, validando su información mediante reglas de negocio.
// - Garantiza la integridad de datos como nombre único, capacidad válida y fechas de creación.
// ************************************************************************

using System;
using System.Linq;
using ReservaCo.Domain.Entities;
using ReservaCo.Domain.Business;
using ReservaCo.Infrastructure.Context;
using System.Collections.Generic;

namespace ReservaCo.Application.Service
{
    // Servicio de aplicación para manejar operaciones relacionadas con espacios físicos.
    public class EspacioService
    {
        // Contexto de base de datos para acceder a la información persistida.
        private readonly ReservaCoDbContext _context;

        // Constructor que recibe el contexto mediante inyección de dependencias.
        public EspacioService(ReservaCoDbContext context)
        {
            _context = context;
        }

        // Método que obtiene la lista completa de espacios registrados.
        public List<Espacio> ObtenerEspacios()
        {
            return _context.Espacios.ToList();
        }

        // Método que guarda un nuevo espacio o actualiza uno existente.
        public void GuardarEspacio(Espacio espacio)
        {
            // Obtiene la lista de espacios con el mismo nombre (excepto el actual) para verificar duplicados.
            var existentes = _context.Espacios
                .Where(e => e.Nombre == espacio.Nombre && e.Id != espacio.Id)
                .ToList();

            // Valida los datos del espacio usando las reglas de dominio.
            EspacioRules.Validar(espacio);
            EspacioRules.FormatearNombre(espacio);
            EspacioRules.ValidarCapacidad(espacio);
            EspacioRules.ValidarNombreUnico(espacio, existentes);

            // Si es un espacio nuevo (Id = 0), lo agrega con fecha de creación actual.
            if (espacio.Id == 0)
            {
                if (espacio.FechaCreacion == DateTime.MinValue)
                    espacio.FechaCreacion = DateTime.Now;

                _context.Espacios.Add(espacio);
            }
            else
            {
                // Si ya existe, se marca como modificado para actualizarlo.
                _context.Entry(espacio).State = System.Data.Entity.EntityState.Modified;
            }

            // Guarda los cambios en la base de datos.
            _context.SaveChanges();
        }

        // Método que elimina un espacio por su identificador.
        public void EliminarEspacio(int id)
        {
            // Busca el espacio en la base de datos.
            var espacio = _context.Espacios.Find(id);

            // Si existe, se elimina y se persiste el cambio.
            if (espacio != null)
            {
                _context.Espacios.Remove(espacio);
                _context.SaveChanges();
            }
        }
    }
}
