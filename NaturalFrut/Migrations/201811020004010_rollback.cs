namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rollback : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clientes", "Saldo", c => c.Double());
            AddColumn("dbo.VentasMayoristas", "Saldo", c => c.Double());
            AddColumn("dbo.VentasMayoristas", "NuevoSaldo", c => c.Double());
            DropColumn("dbo.Clientes", "SaldoReal");
            DropColumn("dbo.Clientes", "SaldoParcial");
            DropColumn("dbo.VentasMayoristas", "SaldoReal");
            DropColumn("dbo.VentasMayoristas", "SaldoParcial");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VentasMayoristas", "SaldoParcial", c => c.Double());
            AddColumn("dbo.VentasMayoristas", "SaldoReal", c => c.Double());
            AddColumn("dbo.Clientes", "SaldoParcial", c => c.Double());
            AddColumn("dbo.Clientes", "SaldoReal", c => c.Double());
            DropColumn("dbo.VentasMayoristas", "NuevoSaldo");
            DropColumn("dbo.VentasMayoristas", "Saldo");
            DropColumn("dbo.Clientes", "Saldo");
        }
    }
}
