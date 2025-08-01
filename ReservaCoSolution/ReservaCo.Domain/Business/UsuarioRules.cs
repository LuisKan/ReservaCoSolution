// ************************************************************************
// Proyecto 02 
// Aguilar Verónica, Guerrero Luis
// Fecha de realización: 21/07/2025 
// Fecha de entrega: 03/08/2025  
// Resultados:
// - Esta clase estática contiene reglas de negocio relacionadas con la entidad 'Usuario'.
// - Permite actualizar y validar correos electrónicos, garantizando su unicidad y formato adecuado.
// - Ayuda a mantener la consistencia de datos y evitar conflictos durante el registro o edición de usuarios.
// ************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReservaCo.Domain.Entities;

namespace ReservaCo.Domain.Business
{
    // Clase que agrupa las validaciones y transformaciones relacionadas con el usuario.
    public static class UsuarioRules
    {
        // Método que permite cambiar el correo de un usuario, aplicando formato.
        public static void CambiarCorreo(Usuario usuario, string nuevoCorreo)
        {
            // Valida que el nuevo correo no esté vacío ni solo en blanco.
            if (string.IsNullOrWhiteSpace(nuevoCorreo))
                throw new ArgumentException("El correo no puede estar vacío.");

            // Normaliza el correo: elimina espacios y lo convierte a minúsculas.
            usuario.Email = nuevoCorreo.Trim().ToLower();
        }

        // Verifica que el correo electrónico no esté ya registrado por otro usuario.
        public static void ValidarCorreoUnico(string correo, List<Usuario> usuariosExistentes, int? idExistente = null)
        {
            // Valida que el correo proporcionado no sea vacío.
            if (string.IsNullOrWhiteSpace(correo))
                throw new ArgumentException("El correo no puede estar vacío.");

            // Normaliza el correo: elimina espacios y lo convierte a minúsculas.
            var correoNormalizado = correo.Trim().ToLower();

            // Busca si ya existe un usuario con el mismo correo, excluyendo un ID opcional (útil para edición).
            var existe = usuariosExistentes
                .Any(u => u.Email.ToLower() == correoNormalizado && (idExistente == null || u.Id != idExistente));

            // Lanza excepción si se encuentra duplicado.
            if (existe)
                throw new InvalidOperationException("Ya existe un usuario con ese correo.");
        }
    }
}
