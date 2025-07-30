using System;
using System.Linq;
using ReservaCo.Domain.Entities;
using ReservaCo.Domain.Business;
using ReservaCo.Infrastructure.Context;
using System.Collections.Generic;

namespace ReservaCo.Application.Service
{
    public class RolService
    {

        private readonly ReservaCoDbContext _context;

        public RolService(ReservaCoDbContext context)
        {
            _context = context;
        }

        public List<Rol> ObtenerRoles()
        {
            return _context.Roles.ToList();
        }

        public Rol ObtenerRolPorId(int id)
        {
            return _context.Roles.Find(id);
        }

        public void GuardarRol(Rol rol)
        {

            RolRules.Validar(rol);
            RolRules.NormalizarNombre(rol);


            var existentes = _context.Roles
                .Where(r => r.Nombre == rol.Nombre && r.Id != rol.Id)
                .ToList();

            RolRules.ValidarNombreUnico(rol, existentes);

            if (rol.Id == 0)
                _context.Roles.Add(rol);
            else
                _context.Entry(rol).State = System.Data.Entity.EntityState.Modified;

            _context.SaveChanges();
        }

        public void EliminarRol(int id)
        {
            var rol = _context.Roles.Find(id);
            if (rol != null)
            {
                _context.Roles.Remove(rol);
                _context.SaveChanges();
            }
        }

        

    }
}
