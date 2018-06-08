namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelacionMuchosAMuchosVentasProductos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductosXVenta",
                c => new
                    {
                        Venta_ID = c.Int(nullable: false),
                        Producto_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Venta_ID, t.Producto_ID })
                .ForeignKey("dbo.Ventas", t => t.Venta_ID, cascadeDelete: true)
                .ForeignKey("dbo.Productos", t => t.Producto_ID, cascadeDelete: true)
                .Index(t => t.Venta_ID)
                .Index(t => t.Producto_ID);
            
            AddColumn("dbo.Productos", "VentaID", c => c.Int(nullable: false));
            AddColumn("dbo.Ventas", "ProductoID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductosXVenta", "Producto_ID", "dbo.Productos");
            DropForeignKey("dbo.ProductosXVenta", "Venta_ID", "dbo.Ventas");
            DropIndex("dbo.ProductosXVenta", new[] { "Producto_ID" });
            DropIndex("dbo.ProductosXVenta", new[] { "Venta_ID" });
            DropColumn("dbo.Ventas", "ProductoID");
            DropColumn("dbo.Productos", "VentaID");
            DropTable("dbo.ProductosXVenta");
        }
    }
}
