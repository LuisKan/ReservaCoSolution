// ************************************************************************
// Proyecto 02 
// Aguilar Verónica, Guerrero Luis
// Fecha de realización: 21/07/2025 
// Fecha de entrega: 03/08/2025  
// Resultados:
// - Este modelo representa la estructura de datos utilizada para realizar una solicitud de inicio de sesión.
// - Captura las credenciales básicas del usuario: correo y contraseña.
// - Se emplea como DTO en peticiones HTTP desde el frontend hacia los controladores de autenticación.
// ************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReservaCo.Web.Models
{
    // Modelo que encapsula los datos enviados por el usuario para iniciar sesión.
    public class LoginRequest
    {
        // Correo electrónico ingresado por el usuario.
        public string Correo { get; set; }

        // Contraseña ingresada por el usuario.
        public string Contraseña { get; set; }
    }
}
