namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Model_ProductoXVenta_Descuento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductosXVenta", "Descuento", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductosXVenta", "Descuento");
        }
    }
}
