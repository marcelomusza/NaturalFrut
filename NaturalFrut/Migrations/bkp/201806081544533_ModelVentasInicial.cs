namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelVentasInicial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ventas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        Impreso = c.Boolean(nullable: false),
                        NoConcretado = c.Boolean(nullable: false),
                        EntregaEfectivo = c.Boolean(nullable: false),
                        Descuento = c.Int(nullable: false),
                        ClienteID = c.Int(nullable: false),
                        VendedorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Clientes", t => t.ClienteID, cascadeDelete: true)
                .ForeignKey("dbo.Vendedor", t => t.VendedorID, cascadeDelete: true)
                .Index(t => t.ClienteID)
                .Index(t => t.VendedorID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ventas", "VendedorID", "dbo.Vendedor");
            DropForeignKey("dbo.Ventas", "ClienteID", "dbo.Clientes");
            DropIndex("dbo.Ventas", new[] { "VendedorID" });
            DropIndex("dbo.Ventas", new[] { "ClienteID" });
            DropTable("dbo.Ventas");
        }
    }
}
