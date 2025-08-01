using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReservaCo.Domain.Entities;

namespace ReservaCo.Web.Utils
{
    public static class UsuarioSesion
    {
        public static Usuario UsuarioActual { get; set; }
    }
}