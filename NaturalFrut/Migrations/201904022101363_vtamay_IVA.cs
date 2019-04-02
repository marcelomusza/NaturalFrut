namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vtamay_IVA : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VentasMayoristas", "IVA", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.VentasMayoristas", "IVA");
        }
    }
}
