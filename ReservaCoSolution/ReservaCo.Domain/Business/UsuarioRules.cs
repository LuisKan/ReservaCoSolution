using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReservaCo.Domain.Entities;

namespace ReservaCo.Domain.Business
{
    public static class UsuarioRules
    {
        public static void CambiarCorreo(Usuario usuario, string nuevoCorreo)
        {
            if (string.IsNullOrWhiteSpace(nuevoCorreo))
                throw new ArgumentException("El correo no puede estar vacío.");

            usuario.Email = nuevoCorreo.Trim().ToLower();
        }

        public static void ValidarCorreoUnico(string correo, List<Usuario> usuariosExistentes, int? idExistente = null)
        {
            if (string.IsNullOrWhiteSpace(correo))
                throw new ArgumentException("El correo no puede estar vacío.");

            var correoNormalizado = correo.Trim().ToLower();

            var existe = usuariosExistentes
                .Any(u => u.Email.ToLower() == correoNormalizado && (idExistente == null || u.Id != idExistente));

            if (existe)
                throw new InvalidOperationException("Ya existe un usuario con ese correo.");
        }


    }
}