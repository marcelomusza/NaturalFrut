namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nuevaMigracion1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Compra", "Debe");
            DropColumn("dbo.Compra", "SaldoAfavor");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Compra", "SaldoAfavor", c => c.Double());
            AddColumn("dbo.Compra", "Debe", c => c.Double());
        }
    }
}
