// ************************************************************************
// Proyecto 02 
// Aguilar Verónica, Guerrero Luis
// Fecha de realización: 21/07/2025 
// Fecha de entrega: 03/08/2025  
// Resultados:
// - Esta clase representa el servicio de aplicación para la entidad 'Rol'.
// - Encapsula la lógica de negocio y persistencia necesaria para crear, consultar, actualizar y eliminar roles.
// - Utiliza las reglas de validación y normalización de la capa de dominio para garantizar integridad.
// ************************************************************************

using System;
using System.Linq;
using ReservaCo.Domain.Entities;
using ReservaCo.Domain.Business;
using ReservaCo.Infrastructure.Context;
using System.Collections.Generic;

namespace ReservaCo.Application.Service
{
    // Servicio de aplicación encargado de la gestión de roles.
    public class RolService
    {
        // Contexto de base de datos para acceder a las entidades persistidas.
        private readonly ReservaCoDbContext _context;

        // Constructor que recibe el contexto mediante inyección de dependencias.
        public RolService(ReservaCoDbContext context)
        {
            _context = context;
        }

        // Método que retorna todos los roles registrados en la base de datos.
        public List<Rol> ObtenerRoles()
        {
            return _context.Roles.ToList();
        }

        // Método que obtiene un rol específico según su identificador.
        public Rol ObtenerRolPorId(int id)
        {
            return _context.Roles.Find(id);
        }

        // Método que guarda un nuevo rol o actualiza uno existente.
        public void GuardarRol(Rol rol)
        {
            // Aplica las reglas de validación definidas en la capa de dominio.
            RolRules.Validar(rol);

            // Elimina espacios y estandariza el nombre del rol.
            RolRules.NormalizarNombre(rol);

            // Verifica si ya existe otro rol con el mismo nombre (ignorando el actual).
            var existentes = _context.Roles
                .Where(r => r.Nombre == rol.Nombre && r.Id != rol.Id)
                .ToList();

            RolRules.ValidarNombreUnico(rol, existentes);

            // Si es un rol nuevo, se agrega. Si ya existe, se marca como modificado.
            if (rol.Id == 0)
                _context.Roles.Add(rol);
            else
                _context.Entry(rol).State = System.Data.Entity.EntityState.Modified;

            // Persiste los cambios en la base de datos.
            _context.SaveChanges();
        }

        // Método que elimina un rol según su identificador.
        public void EliminarRol(int id)
        {
            // Busca el rol en la base de datos.
            var rol = _context.Roles.Find(id);

            // Si existe, lo elimina y guarda los cambios.
            if (rol != null)
            {
                _context.Roles.Remove(rol);
                _context.SaveChanges();
            }
        }
    }
}
