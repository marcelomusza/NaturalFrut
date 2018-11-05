namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class models_saldoparcial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clientes", "SaldoParcial", c => c.Double());
            AddColumn("dbo.VentasMayoristas", "SaldoParcial", c => c.Double());
            DropColumn("dbo.VentasMayoristas", "NuevoSaldo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VentasMayoristas", "NuevoSaldo", c => c.Double());
            DropColumn("dbo.VentasMayoristas", "SaldoParcial");
            DropColumn("dbo.Clientes", "SaldoParcial");
        }
    }
}
