namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nuevas : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.VentasMayoristas", "Saldo", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VentasMayoristas", "Saldo", c => c.Double(nullable: false));
        }
    }
}
