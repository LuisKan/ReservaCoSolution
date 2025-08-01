// ************************************************************************
// Proyecto 02 
// Aguilar Verónica, Guerrero Luis
// Fecha de realización: 21/07/2025 
// Fecha de entrega: 03/08/2025  
// Resultados:
// - Este archivo define la clase `ReservaCoDbContext`, que representa el contexto principal de Entity Framework para la base de datos.
// - Expone las entidades del dominio como conjuntos (`DbSet`) para su persistencia y consultas.
// - Configura la conexión a la base de datos mediante la cadena de conexión 'ReservaCoConnection'.
// ************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ReservaCo.Domain.Entities;

namespace ReservaCo.Infrastructure.Context
{
    // Clase que representa el contexto de base de datos del sistema usando Entity Framework.
    public class ReservaCoDbContext : DbContext
    {
        // Constructor que inicializa el contexto usando la cadena de conexión especificada en el archivo de configuración.
        public ReservaCoDbContext() : base("name=ReservaCoConnection")
        {
        }

        // Representa la tabla de usuarios en la base de datos.
        public DbSet<Usuario> Usuarios { get; set; }

        // Representa la tabla de roles en la base de datos.
        public DbSet<Rol> Roles { get; set; }

        // Representa la tabla de espacios en la base de datos.
        public DbSet<Espacio> Espacios { get; set; }

        // Representa la tabla de reservas en la base de datos.
        public DbSet<Reserva> Reservas { get; set; }

        // Método para configurar relaciones y convenciones personalizadas usando Fluent API.
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Llama a la implementación base sin sobreescribir ninguna convención.
            base.OnModelCreating(modelBuilder);
        }
    }
}
