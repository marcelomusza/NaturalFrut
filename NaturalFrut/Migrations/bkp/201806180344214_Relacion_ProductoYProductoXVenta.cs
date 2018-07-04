namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Relacion_ProductoYProductoXVenta : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.ProductosXVenta", "ProductoID");
            AddForeignKey("dbo.ProductosXVenta", "ProductoID", "dbo.Productos", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductosXVenta", "ProductoID", "dbo.Productos");
            DropIndex("dbo.ProductosXVenta", new[] { "ProductoID" });
        }
    }
}
