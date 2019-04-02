namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClienteDebeYSaldoAFavor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clientes", "Debe", c => c.Double());
            AddColumn("dbo.Clientes", "SaldoAfavor", c => c.Double());
            AddColumn("dbo.VentasMayoristas", "Debe", c => c.Double());
            AddColumn("dbo.VentasMayoristas", "SaldoAFavor", c => c.Double());
            DropColumn("dbo.Clientes", "Saldo");
            DropColumn("dbo.Clientes", "SaldoParcial");
            DropColumn("dbo.VentasMayoristas", "Saldo");
            DropColumn("dbo.VentasMayoristas", "SaldoParcial");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VentasMayoristas", "SaldoParcial", c => c.Double());
            AddColumn("dbo.VentasMayoristas", "Saldo", c => c.Double());
            AddColumn("dbo.Clientes", "SaldoParcial", c => c.Double());
            AddColumn("dbo.Clientes", "Saldo", c => c.Double());
            DropColumn("dbo.VentasMayoristas", "SaldoAFavor");
            DropColumn("dbo.VentasMayoristas", "Debe");
            DropColumn("dbo.Clientes", "SaldoAfavor");
            DropColumn("dbo.Clientes", "Debe");
        }
    }
}
