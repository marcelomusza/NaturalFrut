namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cambiosEnModel_CondicionIVA_ViewModel_Cliente : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clientes", "CondicionIVAId", c => c.Byte(nullable: false));
            AddColumn("dbo.Clientes", "TipoClienteId", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clientes", "TipoClienteId");
            DropColumn("dbo.Clientes", "CondicionIVAId");
        }
    }
}
