namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modificacionTipoDato : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.VentasMayoristas", "EntregaEfectivo", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VentasMayoristas", "EntregaEfectivo", c => c.Boolean(nullable: false));
        }
    }
}
