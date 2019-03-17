namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Proveedor_NombreReqOnly : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Proveedores", "Contacto", c => c.String());
            AlterColumn("dbo.Proveedores", "Direccion", c => c.String());
            AlterColumn("dbo.Proveedores", "Localidad", c => c.String());
            AlterColumn("dbo.Proveedores", "Cuit", c => c.String());
            AlterColumn("dbo.Proveedores", "Iibb", c => c.String());
            AlterColumn("dbo.Proveedores", "Email", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Proveedores", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.Proveedores", "Iibb", c => c.String(nullable: false));
            AlterColumn("dbo.Proveedores", "Cuit", c => c.String(nullable: false));
            AlterColumn("dbo.Proveedores", "Localidad", c => c.String(nullable: false));
            AlterColumn("dbo.Proveedores", "Direccion", c => c.String(nullable: false));
            AlterColumn("dbo.Proveedores", "Contacto", c => c.String(nullable: false));
        }
    }
}
