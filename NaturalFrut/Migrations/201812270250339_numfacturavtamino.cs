namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class numfacturavtamino : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.VentasMinoristas", "NumFactura", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VentasMinoristas", "NumFactura", c => c.Int(nullable: false));
        }
    }
}
