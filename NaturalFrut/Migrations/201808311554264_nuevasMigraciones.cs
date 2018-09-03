namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nuevasMigraciones : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductosMix",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProdMixId = c.Int(),
                        ProductoDelMixId = c.Int(),
                        Cantidad = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Productos", t => t.ProdMixId)
                .ForeignKey("dbo.Productos", t => t.ProductoDelMixId)
                .Index(t => t.ProdMixId)
                .Index(t => t.ProductoDelMixId);
            
            AddColumn("dbo.Productos", "EsMix", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductosMix", "ProductoDelMixId", "dbo.Productos");
            DropForeignKey("dbo.ProductosMix", "ProdMixId", "dbo.Productos");
            DropIndex("dbo.ProductosMix", new[] { "ProductoDelMixId" });
            DropIndex("dbo.ProductosMix", new[] { "ProdMixId" });
            DropColumn("dbo.Productos", "EsMix");
            DropTable("dbo.ProductosMix");
        }
    }
}
