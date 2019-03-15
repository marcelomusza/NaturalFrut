namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class compranuevacolumna : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Compra", "Debe", c => c.Double());
            AddColumn("dbo.Compra", "SaldoAfavor", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Compra", "SaldoAfavor");
            DropColumn("dbo.Compra", "Debe");
        }
    }
}
