namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Model_Cliente_CampoSaldo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clientes", "Saldo", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clientes", "Saldo");
        }
    }
}
