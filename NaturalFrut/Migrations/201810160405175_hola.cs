namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hola : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProductosXVenta", "VentaID", "dbo.VentasMayoristas");
            DropIndex("dbo.ProductosXVenta", new[] { "VentaID" });
            AlterColumn("dbo.ProductosXVenta", "VentaID", c => c.Int(nullable: false));
            CreateIndex("dbo.ProductosXVenta", "VentaID");
            AddForeignKey("dbo.ProductosXVenta", "VentaID", "dbo.VentasMayoristas", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductosXVenta", "VentaID", "dbo.VentasMayoristas");
            DropIndex("dbo.ProductosXVenta", new[] { "VentaID" });
            AlterColumn("dbo.ProductosXVenta", "VentaID", c => c.Int());
            CreateIndex("dbo.ProductosXVenta", "VentaID");
            AddForeignKey("dbo.ProductosXVenta", "VentaID", "dbo.VentasMayoristas", "ID");
        }
    }
}
