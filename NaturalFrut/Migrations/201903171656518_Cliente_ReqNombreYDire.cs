namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cliente_ReqNombreYDire : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Clientes", "RazonSocial", c => c.String());
            AlterColumn("dbo.Clientes", "Cuit", c => c.String());
            AlterColumn("dbo.Clientes", "Iibb", c => c.String());
            AlterColumn("dbo.Clientes", "Provincia", c => c.String());
            AlterColumn("dbo.Clientes", "Localidad", c => c.String());
            AlterColumn("dbo.Clientes", "Email", c => c.String());
            AlterColumn("dbo.Clientes", "Transporte", c => c.String());
            AlterColumn("dbo.Clientes", "DireccionTransporte", c => c.String());
            AlterColumn("dbo.Clientes", "Dias", c => c.String());
            AlterColumn("dbo.Clientes", "Horarios", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Clientes", "Horarios", c => c.String(nullable: false));
            AlterColumn("dbo.Clientes", "Dias", c => c.String(nullable: false));
            AlterColumn("dbo.Clientes", "DireccionTransporte", c => c.String(nullable: false));
            AlterColumn("dbo.Clientes", "Transporte", c => c.String(nullable: false));
            AlterColumn("dbo.Clientes", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.Clientes", "Localidad", c => c.String(nullable: false));
            AlterColumn("dbo.Clientes", "Provincia", c => c.String(nullable: false));
            AlterColumn("dbo.Clientes", "Iibb", c => c.String(nullable: false));
            AlterColumn("dbo.Clientes", "Cuit", c => c.String(nullable: false));
            AlterColumn("dbo.Clientes", "RazonSocial", c => c.String(nullable: false));
        }
    }
}
