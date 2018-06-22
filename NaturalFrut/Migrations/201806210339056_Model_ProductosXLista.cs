namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Model_ProductosXLista : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ListaDePrecios", "ProductoID", "dbo.Productos");
            DropForeignKey("dbo.ListaDePrecios", "ClienteID", "dbo.Clientes");
            DropForeignKey("dbo.ListaDePrecios", "TipoDeUnidadID", "dbo.TiposDeUnidad");
            DropIndex("dbo.ListaDePrecios", new[] { "ProductoID" });
            DropIndex("dbo.ListaDePrecios", new[] { "TipoDeUnidadID" });
            DropIndex("dbo.ListaDePrecios", new[] { "ClienteID" });
            RenameColumn(table: "dbo.ListaDePrecios", name: "ProductoID", newName: "Producto_ID");
            RenameColumn(table: "dbo.ListaDePrecios", name: "ClienteID", newName: "Cliente_ID");
            RenameColumn(table: "dbo.ListaDePrecios", name: "TipoDeUnidadID", newName: "TipoDeUnidad_ID");
            CreateTable(
                "dbo.ProductosXListas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ListaDePreciosID = c.Int(nullable: false),
                        ProductoID = c.Int(nullable: false),
                        TipoDeUnidadID = c.Int(nullable: false),
                        ClienteID = c.Int(nullable: false),
                        Precio = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Clientes", t => t.ClienteID, cascadeDelete: true)
                .ForeignKey("dbo.ListaDePrecios", t => t.ListaDePreciosID, cascadeDelete: true)
                .ForeignKey("dbo.Productos", t => t.ProductoID, cascadeDelete: true)
                .ForeignKey("dbo.TiposDeUnidad", t => t.TipoDeUnidadID, cascadeDelete: true)
                .Index(t => t.ListaDePreciosID)
                .Index(t => t.ProductoID)
                .Index(t => t.TipoDeUnidadID)
                .Index(t => t.ClienteID);
            
            AlterColumn("dbo.ListaDePrecios", "Producto_ID", c => c.Int());
            AlterColumn("dbo.ListaDePrecios", "TipoDeUnidad_ID", c => c.Int());
            AlterColumn("dbo.ListaDePrecios", "Cliente_ID", c => c.Int());
            CreateIndex("dbo.ListaDePrecios", "Cliente_ID");
            CreateIndex("dbo.ListaDePrecios", "TipoDeUnidad_ID");
            CreateIndex("dbo.ListaDePrecios", "Producto_ID");
            AddForeignKey("dbo.ListaDePrecios", "Producto_ID", "dbo.Productos", "ID");
            AddForeignKey("dbo.ListaDePrecios", "Cliente_ID", "dbo.Clientes", "ID");
            AddForeignKey("dbo.ListaDePrecios", "TipoDeUnidad_ID", "dbo.TiposDeUnidad", "ID");
            DropColumn("dbo.ListaDePrecios", "Precio");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ListaDePrecios", "Precio", c => c.Single(nullable: false));
            DropForeignKey("dbo.ListaDePrecios", "TipoDeUnidad_ID", "dbo.TiposDeUnidad");
            DropForeignKey("dbo.ListaDePrecios", "Cliente_ID", "dbo.Clientes");
            DropForeignKey("dbo.ListaDePrecios", "Producto_ID", "dbo.Productos");
            DropForeignKey("dbo.ProductosXListas", "TipoDeUnidadID", "dbo.TiposDeUnidad");
            DropForeignKey("dbo.ProductosXListas", "ProductoID", "dbo.Productos");
            DropForeignKey("dbo.ProductosXListas", "ListaDePreciosID", "dbo.ListaDePrecios");
            DropForeignKey("dbo.ProductosXListas", "ClienteID", "dbo.Clientes");
            DropIndex("dbo.ProductosXListas", new[] { "ClienteID" });
            DropIndex("dbo.ProductosXListas", new[] { "TipoDeUnidadID" });
            DropIndex("dbo.ProductosXListas", new[] { "ProductoID" });
            DropIndex("dbo.ProductosXListas", new[] { "ListaDePreciosID" });
            DropIndex("dbo.ListaDePrecios", new[] { "Producto_ID" });
            DropIndex("dbo.ListaDePrecios", new[] { "TipoDeUnidad_ID" });
            DropIndex("dbo.ListaDePrecios", new[] { "Cliente_ID" });
            AlterColumn("dbo.ListaDePrecios", "Cliente_ID", c => c.Int(nullable: false));
            AlterColumn("dbo.ListaDePrecios", "TipoDeUnidad_ID", c => c.Int(nullable: false));
            AlterColumn("dbo.ListaDePrecios", "Producto_ID", c => c.Int(nullable: false));
            DropTable("dbo.ProductosXListas");
            RenameColumn(table: "dbo.ListaDePrecios", name: "TipoDeUnidad_ID", newName: "TipoDeUnidadID");
            RenameColumn(table: "dbo.ListaDePrecios", name: "Cliente_ID", newName: "ClienteID");
            RenameColumn(table: "dbo.ListaDePrecios", name: "Producto_ID", newName: "ProductoID");
            CreateIndex("dbo.ListaDePrecios", "ClienteID");
            CreateIndex("dbo.ListaDePrecios", "TipoDeUnidadID");
            CreateIndex("dbo.ListaDePrecios", "ProductoID");
            AddForeignKey("dbo.ListaDePrecios", "TipoDeUnidadID", "dbo.TiposDeUnidad", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ListaDePrecios", "ClienteID", "dbo.Clientes", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ListaDePrecios", "ProductoID", "dbo.Productos", "ID", cascadeDelete: true);
        }
    }
}
