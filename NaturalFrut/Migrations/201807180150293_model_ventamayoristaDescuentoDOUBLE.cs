namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class model_ventamayoristaDescuentoDOUBLE : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.VentasMayoristas", "Descuento", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VentasMayoristas", "Descuento", c => c.Int(nullable: false));
        }
    }
}
