namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nuevoscamposProveedor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Proveedores", "Debe", c => c.Double());
            AddColumn("dbo.Proveedores", "SaldoAfavor", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Proveedores", "SaldoAfavor");
            DropColumn("dbo.Proveedores", "Debe");
        }
    }
}
