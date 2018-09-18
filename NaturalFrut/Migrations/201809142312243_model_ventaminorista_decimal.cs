namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class model_ventaminorista_decimal : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.VentasMinoristas", "ImporteVentaTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VentasMinoristas", "ImporteVentaTotal", c => c.Double(nullable: false));
        }
    }
}
