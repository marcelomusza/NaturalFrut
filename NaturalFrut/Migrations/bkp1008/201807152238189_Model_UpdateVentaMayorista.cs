namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Model_UpdateVentaMayorista : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VentasMayoristas", "NumeroVenta", c => c.Int(nullable: false));
            AddColumn("dbo.VentasMayoristas", "SumaTotal", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VentasMayoristas", "SumaTotal");
            DropColumn("dbo.VentasMayoristas", "NumeroVenta");
        }
    }
}
