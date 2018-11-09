namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nuevasMigrations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clientes", "SaldoParcial", c => c.Double());
            AddColumn("dbo.VentasMayoristas", "SaldoParcial", c => c.Double());
            DropColumn("dbo.VentasMayoristas", "NuevoSaldo");
            DropColumn("dbo.VentasMinoristas", "ImporteVentaTotal");
            DropColumn("dbo.VentasMinoristas", "Iva");
            DropColumn("dbo.VentasMinoristas", "CantidadPersonas");
            DropColumn("dbo.VentasMinoristas", "Promedio");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VentasMinoristas", "Promedio", c => c.Double(nullable: false));
            AddColumn("dbo.VentasMinoristas", "CantidadPersonas", c => c.Int(nullable: false));
            AddColumn("dbo.VentasMinoristas", "Iva", c => c.Double(nullable: false));
            AddColumn("dbo.VentasMinoristas", "ImporteVentaTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.VentasMayoristas", "NuevoSaldo", c => c.Double());
            DropColumn("dbo.VentasMayoristas", "SaldoParcial");
            DropColumn("dbo.Clientes", "SaldoParcial");
        }
    }
}
