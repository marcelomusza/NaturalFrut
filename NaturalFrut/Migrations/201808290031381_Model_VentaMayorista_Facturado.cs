namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Model_VentaMayorista_Facturado : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VentasMayoristas", "Facturado", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VentasMayoristas", "Facturado");
        }
    }
}
