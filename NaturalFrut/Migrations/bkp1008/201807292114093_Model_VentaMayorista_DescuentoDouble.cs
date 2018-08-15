namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Model_VentaMayorista_DescuentoDouble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.VentasMayoristas", "Descuento", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VentasMayoristas", "Descuento", c => c.Double(nullable: false));
        }
    }
}
