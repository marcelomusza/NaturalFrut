namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VtaMinorista_ReqLocaleImporte : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.VentasMinoristas", "NumFactura", c => c.String());
            AlterColumn("dbo.VentasMinoristas", "RazonSocial", c => c.String());
            AlterColumn("dbo.VentasMinoristas", "TipoFactura", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VentasMinoristas", "TipoFactura", c => c.String(nullable: false));
            AlterColumn("dbo.VentasMinoristas", "RazonSocial", c => c.String(nullable: false));
            AlterColumn("dbo.VentasMinoristas", "NumFactura", c => c.String(nullable: false));
        }
    }
}
