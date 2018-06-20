namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Relacion_VentaYProductosXVenta : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductosXVenta", "VentaID", c => c.Int(nullable: false));
            CreateIndex("dbo.ProductosXVenta", "VentaID");
            AddForeignKey("dbo.ProductosXVenta", "VentaID", "dbo.Ventas", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductosXVenta", "VentaID", "dbo.Ventas");
            DropIndex("dbo.ProductosXVenta", new[] { "VentaID" });
            DropColumn("dbo.ProductosXVenta", "VentaID");
        }
    }
}
