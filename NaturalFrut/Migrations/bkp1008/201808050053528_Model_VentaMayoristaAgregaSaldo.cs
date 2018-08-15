namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Model_VentaMayoristaAgregaSaldo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VentasMayoristas", "Saldo", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VentasMayoristas", "Saldo");
        }
    }
}
