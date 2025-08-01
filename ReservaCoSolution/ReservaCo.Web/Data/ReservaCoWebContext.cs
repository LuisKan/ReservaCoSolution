// ************************************************************************
// Proyecto 02 
// Aguilar Verónica, Guerrero Luis
// Fecha de realización: 21/07/2025 
// Fecha de entrega: 03/08/2025  
// Resultados:
// - Esta clase define el contexto de Entity Framework para la capa Web, permitiendo acceso y persistencia de datos desde modelos de presentación.
// - Expone DbSet para entidades como Usuario, Rol, Espacio y Reserva, conectando formularios y vistas con la base de datos.
// - Utiliza la cadena de conexión 'ReservaCoWebContext' definida en el archivo de configuración web.config.
// ************************************************************************

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ReservaCo.Web.Data
{
    // Contexto de base de datos para la capa Web del proyecto ReservaCo.
    public class ReservaCoWebContext : DbContext
    {
        // Constructor que inicializa el contexto utilizando la cadena de conexión especificada.
        public ReservaCoWebContext() : base("name=ReservaCoWebContext")
        {
        }

        // DbSet que representa la tabla de usuarios basada en el modelo Web.
        public System.Data.Entity.DbSet<ReservaCo.Web.Models.Usuario> Usuarios { get; set; }

        // DbSet que representa la tabla de roles basada en el modelo Web.
        public System.Data.Entity.DbSet<ReservaCo.Web.Models.Rol> Rols { get; set; }

        // DbSet que representa la tabla de espacios basada en el modelo Web.
        public System.Data.Entity.DbSet<ReservaCo.Web.Models.Espacio> Espacios { get; set; }

        // DbSet que representa la tabla de reservas basada en el modelo Web.
        public System.Data.Entity.DbSet<ReservaCo.Web.Models.Reserva> Reservas { get; set; }
    }
}
