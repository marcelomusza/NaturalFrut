namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Model_StockUpdate : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Stocks", newName: "Stock");
            DropColumn("dbo.Stock", "CantidadXBulto");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Stock", "CantidadXBulto", c => c.Int(nullable: false));
            RenameTable(name: "dbo.Stock", newName: "Stocks");
        }
    }
}
