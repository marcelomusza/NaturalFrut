namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class clientetelefeonotrans : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Clientes", "TelefonoTransporte", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Clientes", "TelefonoTransporte", c => c.Int(nullable: false));
        }
    }
}
