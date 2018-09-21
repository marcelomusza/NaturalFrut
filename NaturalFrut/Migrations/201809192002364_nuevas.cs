namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nuevas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VentasMinoristas", "NumeroVenta", c => c.Int(nullable: false));
            AlterColumn("dbo.VentasMinoristas", "ImporteVentaTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VentasMinoristas", "ImporteVentaTotal", c => c.Double(nullable: false));
            DropColumn("dbo.VentasMinoristas", "NumeroVenta");
        }
    }
}
