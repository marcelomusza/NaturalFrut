namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Model_ProductoMixCantidad : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductosMix", "Cantidad", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductosMix", "Cantidad");
        }
    }
}
