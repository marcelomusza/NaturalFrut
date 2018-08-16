namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class repararModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ListaPreciosBlister",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Gramos = c.Int(nullable: false),
                        Precio = c.String(),
                        ProductoID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Productos", t => t.ProductoID)
                .Index(t => t.ProductoID);
            
            AddColumn("dbo.Productos", "EsBlister", c => c.Boolean(nullable: false));
            AddColumn("dbo.VentasMayoristas", "Saldo", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ListaPreciosBlister", "ProductoID", "dbo.Productos");
            DropIndex("dbo.ListaPreciosBlister", new[] { "ProductoID" });
            DropColumn("dbo.VentasMayoristas", "Saldo");
            DropColumn("dbo.Productos", "EsBlister");
            DropTable("dbo.ListaPreciosBlister");
        }
    }
}
