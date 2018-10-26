namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullFactura : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Compra", "Factura", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Compra", "Factura", c => c.Int(nullable: false));
        }
    }
}
