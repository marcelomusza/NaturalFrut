namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Model_Refactoring_Fix_Lista_ListaPrecios : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProductosXLista", "ClienteID", "dbo.Clientes");
            DropForeignKey("dbo.ProductosXLista", "ListaDePreciosID", "dbo.ListaDePrecios");
            DropForeignKey("dbo.ProductosXLista", "ProductoID", "dbo.Productos");
            DropForeignKey("dbo.ProductosXLista", "TipoDeUnidadID", "dbo.TiposDeUnidad");
            DropIndex("dbo.ProductosXLista", new[] { "ListaDePreciosID" });
            DropIndex("dbo.ProductosXLista", new[] { "ProductoID" });
            DropIndex("dbo.ProductosXLista", new[] { "TipoDeUnidadID" });
            DropIndex("dbo.ProductosXLista", new[] { "ClienteID" });
            CreateTable(
                "dbo.ListaPrecios",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ListaID = c.Int(nullable: false),
                        ProductoID = c.Int(nullable: false),
                        PrecioXKG = c.Single(nullable: false),
                        PrecioXBultoCerrado = c.Single(nullable: false),
                        KGBultoCerrado = c.Single(nullable: false),
                        PrecioXUnidad = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Listas", t => t.ListaID, cascadeDelete: true)
                .ForeignKey("dbo.Productos", t => t.ProductoID, cascadeDelete: true)
                .Index(t => t.ListaID)
                .Index(t => t.ProductoID);
            
            CreateTable(
                "dbo.Listas",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Nombre = c.String(nullable: false),
                        ClienteID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Clientes", t => t.ID)
                .Index(t => t.ID);
            
            AddColumn("dbo.Clientes", "ListaID", c => c.Int(nullable: false));
            DropTable("dbo.ProductosXLista");
            DropTable("dbo.ListaDePrecios");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ListaDePrecios",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProductosXLista",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ListaDePreciosID = c.Int(nullable: false),
                        ProductoID = c.Int(nullable: false),
                        TipoDeUnidadID = c.Int(nullable: false),
                        ClienteID = c.Int(nullable: false),
                        Precio = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            DropForeignKey("dbo.ListaPrecios", "ProductoID", "dbo.Productos");
            DropForeignKey("dbo.ListaPrecios", "ListaID", "dbo.Listas");
            DropForeignKey("dbo.Listas", "ID", "dbo.Clientes");
            DropIndex("dbo.Listas", new[] { "ID" });
            DropIndex("dbo.ListaPrecios", new[] { "ProductoID" });
            DropIndex("dbo.ListaPrecios", new[] { "ListaID" });
            DropColumn("dbo.Clientes", "ListaID");
            DropTable("dbo.Listas");
            DropTable("dbo.ListaPrecios");
            CreateIndex("dbo.ProductosXLista", "ClienteID");
            CreateIndex("dbo.ProductosXLista", "TipoDeUnidadID");
            CreateIndex("dbo.ProductosXLista", "ProductoID");
            CreateIndex("dbo.ProductosXLista", "ListaDePreciosID");
            AddForeignKey("dbo.ProductosXLista", "TipoDeUnidadID", "dbo.TiposDeUnidad", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ProductosXLista", "ProductoID", "dbo.Productos", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ProductosXLista", "ListaDePreciosID", "dbo.ListaDePrecios", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ProductosXLista", "ClienteID", "dbo.Clientes", "ID", cascadeDelete: true);
        }
    }
}
