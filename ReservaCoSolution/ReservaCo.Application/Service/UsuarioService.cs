using System;
using System.Linq;
using ReservaCo.Domain.Entities;
using ReservaCo.Domain.Business;
using ReservaCo.Infrastructure.Context;
using System.Collections.Generic;
using System.Data.Entity;

namespace ReservaCo.Application.Services
{
    public class UsuarioService
    {
        private readonly ReservaCoDbContext _context;

        public UsuarioService(ReservaCoDbContext context)
        {
            _context = context;
        }


        public List<Usuario> ObtenerUsuarios()
        {
            return _context.Usuarios
        .Include(u => u.Rol) // 👈 esto es clave
        .ToList();
        }

        public Usuario ObtenerUsuarioPorId(int id)
        {
            return _context.Usuarios
        .Include(u => u.Rol)
        .FirstOrDefault(u => u.Id == id);
        }

        public void GuardarUsuario(Usuario usuario)
        {
            var usuarios = _context.Usuarios.ToList();
            UsuarioRules.ValidarCorreoUnico(usuario.Email, usuarios, usuario.Id == 0 ? null : (int?)usuario.Id);

            if (usuario.Id == 0)
                _context.Usuarios.Add(usuario);
            else
                _context.Entry(usuario).State = EntityState.Modified;

            _context.SaveChanges();
        }


        public void EliminarUsuario(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                _context.SaveChanges();
            }
        }

        public Rol ObtenerRolPorNombre(string nombre)
        {
            return _context.Roles.FirstOrDefault(r => r.Nombre == nombre);
        }

        public void CambiarCorreo(Usuario usuario, string nuevoCorreo)
        {
            UsuarioRules.CambiarCorreo(usuario, nuevoCorreo);
        }


    }
}
