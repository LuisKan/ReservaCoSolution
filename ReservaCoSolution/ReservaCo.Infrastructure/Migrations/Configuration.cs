// ************************************************************************
// Proyecto 02 
// Aguilar Verónica, Guerrero Luis
// Fecha de realización: 21/07/2025 
// Fecha de entrega: 03/08/2025  
// Resultados:
// - Esta clase configura el comportamiento de las migraciones en Entity Framework para el contexto ReservaCoDbContext.
// - Permite definir si las migraciones automáticas están habilitadas y proporciona un método para insertar datos semilla.
// - Facilita la evolución del esquema de base de datos durante el desarrollo.
// ************************************************************************

namespace ReservaCo.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    // Clase de configuración para las migraciones de Entity Framework.
    internal sealed class Configuration : DbMigrationsConfiguration<ReservaCo.Infrastructure.Context.ReservaCoDbContext>
    {
        // Constructor que desactiva las migraciones automáticas por seguridad y control.
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        // Método que se ejecuta automáticamente después de aplicar la última migración.
        protected override void Seed(ReservaCo.Infrastructure.Context.ReservaCoDbContext context)
        {
            // Aquí se pueden insertar datos iniciales en la base de datos después de una migración.

            // Ejemplo de uso recomendado para evitar duplicados:
            // context.Usuarios.AddOrUpdate(u => u.Email, new Usuario { ... });

            // Actualmente no se han definido datos semilla en esta implementación.
        }
    }
}
