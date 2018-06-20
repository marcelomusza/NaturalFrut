namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Model_ProductosXVenta : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductosXVenta",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Cantidad = c.Int(nullable: false),
                        Importe = c.Single(nullable: false),
                        Total = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProductosXVenta");
        }
    }
}
