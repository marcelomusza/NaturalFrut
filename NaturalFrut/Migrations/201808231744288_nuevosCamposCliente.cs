namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nuevosCamposCliente : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clientes", "Transporte", c => c.String(nullable: false));
            AddColumn("dbo.Clientes", "DireccionTransporte", c => c.String(nullable: false));
            AddColumn("dbo.Clientes", "TelefonoTransporte", c => c.Int(nullable: false));
            AddColumn("dbo.Clientes", "Dias", c => c.String(nullable: false));
            AddColumn("dbo.Clientes", "Horarios", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clientes", "Horarios");
            DropColumn("dbo.Clientes", "Dias");
            DropColumn("dbo.Clientes", "TelefonoTransporte");
            DropColumn("dbo.Clientes", "DireccionTransporte");
            DropColumn("dbo.Clientes", "Transporte");
        }
    }
}
