namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Model_ProductoFIXVenta : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Productos", "Venta_ID", "dbo.Ventas");
            DropIndex("dbo.Productos", new[] { "Venta_ID" });
            AddColumn("dbo.ProductosXVenta", "ProductoID", c => c.Int(nullable: false));
            DropColumn("dbo.Productos", "Venta_ID");
            DropColumn("dbo.Ventas", "ProductoID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ventas", "ProductoID", c => c.Int(nullable: false));
            AddColumn("dbo.Productos", "Venta_ID", c => c.Int());
            DropColumn("dbo.ProductosXVenta", "ProductoID");
            CreateIndex("dbo.Productos", "Venta_ID");
            AddForeignKey("dbo.Productos", "Venta_ID", "dbo.Ventas", "ID");
        }
    }
}
