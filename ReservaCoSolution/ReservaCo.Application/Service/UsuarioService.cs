// ************************************************************************
// Proyecto 02 
// Aguilar Verónica, Guerrero Luis
// Fecha de realización: 21/07/2025 
// Fecha de entrega: 03/08/2025  
// Resultados:
// - Este servicio gestiona la lógica de aplicación para la entidad 'Usuario'.
// - Permite registrar, consultar, actualizar y eliminar usuarios, incluyendo la asignación de roles y validación de correos.
// - Utiliza reglas de negocio para asegurar unicidad de correos y normalización de datos.
// ************************************************************************

using System;
using System.Linq;
using ReservaCo.Domain.Entities;
using ReservaCo.Domain.Business;
using ReservaCo.Infrastructure.Context;
using System.Collections.Generic;
using System.Data.Entity;

namespace ReservaCo.Application.Services
{
    // Servicio de aplicación que encapsula la lógica de negocio y acceso a datos para los usuarios.
    public class UsuarioService
    {
        // Contexto de base de datos.
        private readonly ReservaCoDbContext _context;

        // Constructor que inyecta el contexto para acceso a datos.
        public UsuarioService(ReservaCoDbContext context)
        {
            _context = context;
        }

        // Retorna todos los usuarios con sus roles asociados.
        public List<Usuario> ObtenerUsuarios()
        {
            return _context.Usuarios
                .Include(u => u.Rol) // Carga el objeto Rol relacionado a cada usuario (carga ansiosa).
                .ToList();
        }

        // Retorna un usuario específico por su ID, incluyendo su rol.
        public Usuario ObtenerUsuarioPorId(int id)
        {
            return _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefault(u => u.Id == id);
        }

        // Guarda o actualiza un usuario en la base de datos.
        public void GuardarUsuario(Usuario usuario)
        {
            // Obtiene todos los usuarios para validar si el correo es único.
            var usuarios = _context.Usuarios.ToList();

            // Aplica la regla de validación de unicidad de correo.
            UsuarioRules.ValidarCorreoUnico(usuario.Email, usuarios, usuario.Id == 0 ? null : (int?)usuario.Id);

            // Si es un nuevo usuario, lo agrega. Si no, lo actualiza.
            if (usuario.Id == 0)
                _context.Usuarios.Add(usuario);
            else
                _context.Entry(usuario).State = EntityState.Modified;

            // Guarda los cambios en la base de datos.
            _context.SaveChanges();
        }

        // Elimina un usuario existente por su ID.
        public void EliminarUsuario(int id)
        {
            // Busca el usuario en la base de datos.
            var usuario = _context.Usuarios.Find(id);

            // Si lo encuentra, lo elimina.
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                _context.SaveChanges();
            }
        }

        // Obtiene un rol según su nombre (por ejemplo: "Profesor", "Administrador").
        public Rol ObtenerRolPorNombre(string nombre)
        {
            return _context.Roles.FirstOrDefault(r => r.Nombre == nombre);
        }

        // Cambia el correo de un usuario aplicando reglas de normalización.
        public void CambiarCorreo(Usuario usuario, string nuevoCorreo)
        {
            UsuarioRules.CambiarCorreo(usuario, nuevoCorreo);
        }
    }
}
