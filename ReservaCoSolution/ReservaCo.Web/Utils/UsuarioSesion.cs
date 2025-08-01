// ************************************************************************
// Proyecto 02 
// Aguilar Verónica, Guerrero Luis
// Fecha de realización: 21/07/2025 
// Fecha de entrega: 03/08/2025  
// Resultados:
// - Esta clase estática representa una utilidad para almacenar el usuario actualmente autenticado en la sesión del sistema.
// - Facilita el acceso global a la información del usuario durante el ciclo de vida de una sesión web.
// - Su implementación es sencilla pero debe usarse con precaución en entornos concurrentes o multiusuario.
// ************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReservaCo.Domain.Entities;

namespace ReservaCo.Web.Utils
{
    // Clase estática que almacena el usuario autenticado actualmente en sesión.
    public static class UsuarioSesion
    {
        // Propiedad que contiene el objeto Usuario autenticado en la sesión actual.
        public static Usuario UsuarioActual { get; set; }
    }
}
