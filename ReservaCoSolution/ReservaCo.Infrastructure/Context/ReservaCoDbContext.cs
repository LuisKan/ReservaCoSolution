using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ReservaCo.Domain.Entities;

namespace ReservaCo.Infrastructure.Context
{
    public class ReservaCoDbContext : DbContext
    {
        public ReservaCoDbContext() : base("name=ReservaCoConnection")
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Espacio> Espacios { get; set; }
        public DbSet<Reserva> Reservas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

