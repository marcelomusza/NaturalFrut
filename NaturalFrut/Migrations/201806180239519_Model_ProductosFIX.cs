namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Model_ProductosFIX : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Productos", "Nombre", c => c.String(nullable: false));
            AddColumn("dbo.Productos", "Venta_ID", c => c.Int());
            AddColumn("dbo.Ventas", "ProductoID", c => c.Int(nullable: false));
            CreateIndex("dbo.Productos", "Venta_ID");
            AddForeignKey("dbo.Productos", "Venta_ID", "dbo.Ventas", "ID");
            DropColumn("dbo.Productos", "Descripcion");
            DropColumn("dbo.Productos", "Cantidad");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Productos", "Cantidad", c => c.Int(nullable: false));
            AddColumn("dbo.Productos", "Descripcion", c => c.String(nullable: false));
            DropForeignKey("dbo.Productos", "Venta_ID", "dbo.Ventas");
            DropIndex("dbo.Productos", new[] { "Venta_ID" });
            DropColumn("dbo.Ventas", "ProductoID");
            DropColumn("dbo.Productos", "Venta_ID");
            DropColumn("dbo.Productos", "Nombre");
        }
    }
}
