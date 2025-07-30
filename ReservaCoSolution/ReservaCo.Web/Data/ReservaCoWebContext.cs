using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ReservaCo.Web.Data
{
    public class ReservaCoWebContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public ReservaCoWebContext() : base("name=ReservaCoWebContext")
        {
        }

        public System.Data.Entity.DbSet<ReservaCo.Web.Models.Usuario> Usuarios { get; set; }

        public System.Data.Entity.DbSet<ReservaCo.Web.Models.Rol> Rols { get; set; }

        public System.Data.Entity.DbSet<ReservaCo.Web.Models.Espacio> Espacios { get; set; }

        public System.Data.Entity.DbSet<ReservaCo.Web.Models.Reserva> Reservas { get; set; }
    }
}
