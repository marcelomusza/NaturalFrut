namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClienteSaldoProp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clientes", "SaldoReal", c => c.Double());
            AddColumn("dbo.Clientes", "SaldoParcial", c => c.Double());
            DropColumn("dbo.Clientes", "Saldo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clientes", "Saldo", c => c.Double());
            DropColumn("dbo.Clientes", "SaldoParcial");
            DropColumn("dbo.Clientes", "SaldoReal");
        }
    }
}
