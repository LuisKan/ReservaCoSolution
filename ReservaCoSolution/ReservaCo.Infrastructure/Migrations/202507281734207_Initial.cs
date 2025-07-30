namespace ReservaCo.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Inicial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Espacios",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Nombre = c.String(),
                    Tipo = c.String(),
                    Ubicacion = c.String(),
                    Capacidad = c.Int(nullable: false),
                    FechaCreacion = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Reservas",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Estado = c.Int(nullable: false),
                    FechaCreacion = c.DateTime(nullable: false),
                    UsuarioId = c.Int(nullable: false),
                    EspacioId = c.Int(nullable: false),
                    Fecha = c.DateTime(nullable: false),
                    HoraInicio = c.Time(nullable: false, precision: 7),
                    HoraFin = c.Time(nullable: false, precision: 7),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Espacios", t => t.EspacioId, cascadeDelete: true)
                .ForeignKey("dbo.Usuarios", t => t.UsuarioId, cascadeDelete: true)
                .Index(t => t.UsuarioId)
                .Index(t => t.EspacioId);

            CreateTable(
                "dbo.Usuarios",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Nombre = c.String(),
                    Apellido = c.String(),
                    Email = c.String(),
                    Contrasenia = c.String(),
                    FechaCreacion = c.DateTime(nullable: false),
                    RolId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rols", t => t.RolId, cascadeDelete: true)
                .Index(t => t.RolId);

            CreateTable(
                "dbo.Rols",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Nombre = c.String(),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
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