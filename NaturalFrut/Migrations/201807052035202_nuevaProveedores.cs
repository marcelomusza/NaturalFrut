namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nuevaProveedores : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Proveedores", "CondicionIVAId", "dbo.CondicionIVAs");
            DropIndex("dbo.Proveedores", new[] { "CondicionIVAId" });
            AddColumn("dbo.Proveedores", "Contacto", c => c.String(nullable: false));
            AddColumn("dbo.Proveedores", "TelefonoOficina", c => c.Int(nullable: false));
            AddColumn("dbo.Proveedores", "TelefonoOtros", c => c.Int(nullable: false));
            DropColumn("dbo.Proveedores", "RazonSocial");
            DropColumn("dbo.Proveedores", "Saldo");
            DropColumn("dbo.Proveedores", "Provincia");
            DropColumn("dbo.Proveedores", "TelefonoNegocio");
            DropColumn("dbo.Proveedores", "CondicionIVAId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Proveedores", "CondicionIVAId", c => c.Int(nullable: false));
            AddColumn("dbo.Proveedores", "TelefonoNegocio", c => c.Int(nullable: false));
            AddColumn("dbo.Proveedores", "Provincia", c => c.String(nullable: false));
            AddColumn("dbo.Proveedores", "Saldo", c => c.Double(nullable: false));
            AddColumn("dbo.Proveedores", "RazonSocial", c => c.String(nullable: false));
            DropColumn("dbo.Proveedores", "TelefonoOtros");
            DropColumn("dbo.Proveedores", "TelefonoOficina");
            DropColumn("dbo.Proveedores", "Contacto");
            CreateIndex("dbo.Proveedores", "CondicionIVAId");
            AddForeignKey("dbo.Proveedores", "CondicionIVAId", "dbo.CondicionIVAs", "ID", cascadeDelete: true);
        }
    }
}
