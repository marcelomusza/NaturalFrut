namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cambiosClienteProveedor : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Clientes", "TelefonoNegocio", c => c.Int());
            AlterColumn("dbo.Clientes", "TelefonoCelular", c => c.Int());
            AlterColumn("dbo.Proveedores", "TelefonoOficina", c => c.Int());
            AlterColumn("dbo.Proveedores", "TelefonoCelular", c => c.Int());
            AlterColumn("dbo.Proveedores", "TelefonoOtros", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Proveedores", "TelefonoOtros", c => c.Int(nullable: false));
            AlterColumn("dbo.Proveedores", "TelefonoCelular", c => c.Int(nullable: false));
            AlterColumn("dbo.Proveedores", "TelefonoOficina", c => c.Int(nullable: false));
            AlterColumn("dbo.Clientes", "TelefonoCelular", c => c.Int(nullable: false));
            AlterColumn("dbo.Clientes", "TelefonoNegocio", c => c.Int(nullable: false));
        }
    }
}
