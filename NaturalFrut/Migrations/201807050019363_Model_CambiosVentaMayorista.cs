namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Model_CambiosVentaMayorista : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Ventas", newName: "VentasMayoristas");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.VentasMayoristas", newName: "Ventas");
        }
    }
}
