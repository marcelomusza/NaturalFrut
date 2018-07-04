namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Model_ListaDePrecios : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ListaDePrecios",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        ProductoID = c.Int(nullable: false),
                        TipoDeUnidadID = c.Int(nullable: false),
                        ClienteID = c.Int(nullable: false),
                        Precio = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Clientes", t => t.ClienteID, cascadeDelete: true)
                .ForeignKey("dbo.Productos", t => t.ProductoID, cascadeDelete: true)
                .ForeignKey("dbo.TiposDeUnidad", t => t.TipoDeUnidadID, cascadeDelete: true)
                .Index(t => t.ProductoID)
                .Index(t => t.TipoDeUnidadID)
                .Index(t => t.ClienteID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ListaDePrecios", "TipoDeUnidadID", "dbo.TiposDeUnidad");
            DropForeignKey("dbo.ListaDePrecios", "ProductoID", "dbo.Productos");
            DropForeignKey("dbo.ListaDePrecios", "ClienteID", "dbo.Clientes");
            DropIndex("dbo.ListaDePrecios", new[] { "ClienteID" });
            DropIndex("dbo.ListaDePrecios", new[] { "TipoDeUnidadID" });
            DropIndex("dbo.ListaDePrecios", new[] { "ProductoID" });
            DropTable("dbo.ListaDePrecios");
        }
    }
}
