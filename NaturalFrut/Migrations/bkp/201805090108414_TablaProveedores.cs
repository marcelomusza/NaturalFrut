namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TablaProveedores : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Proveedores",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        RazonSocial = c.String(nullable: false),
                        Saldo = c.Double(nullable: true),
                        Cuit = c.String(nullable: false),
                        Iibb = c.String(nullable: false),
                        Direccion = c.String(nullable: false),
                        Provincia = c.String(nullable: false),
                        Localidad = c.String(nullable: false),
                        TelefonoNegocio = c.Int(nullable: false),
                        TelefonoCelular = c.Int(nullable: false),
                        Email = c.String(nullable: false),
                        CondicionIVAId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CondicionIVAs", t => t.CondicionIVAId, cascadeDelete: true)
                .Index(t => t.CondicionIVAId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Proveedores", "CondicionIVAId", "dbo.CondicionIVAs");
            DropIndex("dbo.Proveedores", new[] { "CondicionIVAId" });
            DropTable("dbo.Proveedores");
        }
    }
}
