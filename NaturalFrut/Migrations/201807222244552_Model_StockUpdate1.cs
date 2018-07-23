namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Model_StockUpdate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stock", "CantidadXUnidad", c => c.Int(nullable: false));
            DropColumn("dbo.Stock", "CantidadXPaquete");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Stock", "CantidadXPaquete", c => c.Int(nullable: false));
            DropColumn("dbo.Stock", "CantidadXUnidad");
        }
    }
}
