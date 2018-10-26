namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stringfactura : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Compra", "Factura", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Compra", "Factura", c => c.Int());
        }
    }
}
