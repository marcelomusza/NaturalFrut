namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seAgregaTablaStock : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Stocks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProductoID = c.Int(nullable: false),
                        CantidadXKg = c.Int(nullable: false),
                        CantidadXBulto = c.Int(nullable: false),
                        CantidadXPaquete = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Productos", t => t.ProductoID, cascadeDelete: true)
                .Index(t => t.ProductoID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stocks", "ProductoID", "dbo.Productos");
            DropIndex("dbo.Stocks", new[] { "ProductoID" });
            DropTable("dbo.Stocks");
        }
    }
}
