namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Model_StockCantDouble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Stock", "Cantidad", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Stock", "Cantidad", c => c.Int(nullable: false));
        }
    }
}
