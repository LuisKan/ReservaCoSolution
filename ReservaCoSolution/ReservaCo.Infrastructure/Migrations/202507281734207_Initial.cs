// ************************************************************************
// Proyecto 02 
// Aguilar Verónica, Guerrero Luis
// Fecha de realización: 21/07/2025 
// Fecha de entrega: 03/08/2025  
// Resultados:
// - Esta clase representa la primera migración del sistema mediante Entity Framework.
// - Crea las tablas principales: Rols, Usuarios, Espacios y Reservas, incluyendo sus relaciones y restricciones.
// - Se establece la integridad referencial con claves foráneas y eliminación en cascada.
// ************************************************************************

namespace ReservaCo.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    // Clase parcial que representa la migración inicial del esquema de base de datos.
    public partial class Inicial : DbMigration
    {
        // Método que define los pasos necesarios para aplicar esta migración (creación de tablas y relaciones).
        public override void Up()
        {
            // Crea la tabla 'Espacios' con sus columnas y clave primaria.
            CreateTable(
                "dbo.Espacios",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true), // Clave primaria autoincremental
                    Nombre = c.String(),                         // Nombre del espacio
                    Tipo = c.String(),                           // Tipo (Aula, Laboratorio, etc.)
                    Ubicacion = c.String(),                      // Ubicación física
                    Capacidad = c.Int(nullable: false),          // Capacidad máxima
                    FechaCreacion = c.DateTime(nullable: false), // Fecha de registro
                })
                .PrimaryKey(t => t.Id); // Define la columna Id como clave primaria

            // Crea la tabla 'Reservas' con campos de estado, horario y relaciones.
            CreateTable(
                "dbo.Reservas",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),     // Clave primaria autoincremental
                    Estado = c.Int(nullable: false),                 // Estado de la reserva (enum int)
                    FechaCreacion = c.DateTime(nullable: false),     // Fecha de creación
                    UsuarioId = c.Int(nullable: false),              // Relación con Usuario
                    EspacioId = c.Int(nullable: false),              // Relación con Espacio
                    Fecha = c.DateTime(nullable: false),             // Fecha solicitada
                    HoraInicio = c.Time(nullable: false, precision: 7), // Hora inicio
                    HoraFin = c.Time(nullable: false, precision: 7),   // Hora fin
                })
                .PrimaryKey(t => t.Id) // Define Id como clave primaria
                .ForeignKey("dbo.Espacios", t => t.EspacioId, cascadeDelete: true) // FK con Espacios
                .ForeignKey("dbo.Usuarios", t => t.UsuarioId, cascadeDelete: true) // FK con Usuarios
                .Index(t => t.UsuarioId)  // Índice para búsqueda rápida por Usuario
                .Index(t => t.EspacioId); // Índice para búsqueda rápida por Espacio

            // Crea la tabla 'Usuarios' con datos personales y relación con rol.
            CreateTable(
                "dbo.Usuarios",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),      // Clave primaria
                    Nombre = c.String(),                              // Nombre del usuario
                    Apellido = c.String(),                            // Apellido
                    Email = c.String(),                               // Correo electrónico
                    Contrasenia = c.String(),                         // Contraseña
                    FechaCreacion = c.DateTime(nullable: false),      // Fecha de registro
                    RolId = c.Int(nullable: false),                   // Relación con rol
                })
                .PrimaryKey(t => t.Id) // Clave primaria
                .ForeignKey("dbo.Rols", t => t.RolId, cascadeDelete: true) // FK con Rols
                .Index(t => t.RolId);  // Índice por RolId

            // Crea la tabla 'Rols' con identificador y nombre.
            CreateTable(
                "dbo.Rols",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true), // Clave primaria
                    Nombre = c.String(),                         // Nombre del rol
                })
                .PrimaryKey(t => t.Id); // Define Id como clave primaria
        }

        // Método que revierte los cambios aplicados por la migración (elimina las tablas y relaciones).
        public override void Down()
        {
            // Elimina la clave foránea y tabla en orden inverso a la creación para evitar conflictos.
            DropForeignKey("dbo.Usuarios", "RolId", "dbo.Rols");
            DropForeignKey("dbo.Reservas", "UsuarioId", "dbo.Usuarios");
            DropForeignKey("dbo.Reservas", "EspacioId", "dbo.Espacios");
            DropIndex("dbo.Usuarios", new[] { "RolId" });
            DropIndex("dbo.Reservas", new[] { "EspacioId" });
            DropIndex("dbo.Reservas", new[] { "UsuarioId" });
            DropTable("dbo.Rols");
            DropTable("dbo.Usuarios");
            DropTable("dbo.Reservas");
            DropTable("dbo.Espacios");
        }
    }
}
