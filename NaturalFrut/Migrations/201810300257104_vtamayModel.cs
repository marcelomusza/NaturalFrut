namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vtamayModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VentasMayoristas", "SaldoReal", c => c.Double());
            AddColumn("dbo.VentasMayoristas", "SaldoParcial", c => c.Double());
            DropColumn("dbo.VentasMayoristas", "Saldo");
            DropColumn("dbo.VentasMayoristas", "NuevoSaldo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VentasMayoristas", "NuevoSaldo", c => c.Double());
            AddColumn("dbo.VentasMayoristas", "Saldo", c => c.Double());
            DropColumn("dbo.VentasMayoristas", "SaldoParcial");
            DropColumn("dbo.VentasMayoristas", "SaldoReal");
        }
    }
}
