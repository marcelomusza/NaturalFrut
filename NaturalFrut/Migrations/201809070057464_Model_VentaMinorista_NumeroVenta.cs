namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Model_VentaMinorista_NumeroVenta : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VentasMinoristas", "NumeroVenta", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VentasMinoristas", "NumeroVenta");
        }
    }
}
