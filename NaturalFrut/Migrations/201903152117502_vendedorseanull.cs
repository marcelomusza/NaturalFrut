namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vendedorseanull : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.VentasMayoristas", "VendedorID", "dbo.Vendedor");
            DropIndex("dbo.VentasMayoristas", new[] { "VendedorID" });
            AlterColumn("dbo.VentasMayoristas", "VendedorID", c => c.Int());
            CreateIndex("dbo.VentasMayoristas", "VendedorID");
            AddForeignKey("dbo.VentasMayoristas", "VendedorID", "dbo.Vendedor", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VentasMayoristas", "VendedorID", "dbo.Vendedor");
            DropIndex("dbo.VentasMayoristas", new[] { "VendedorID" });
            AlterColumn("dbo.VentasMayoristas", "VendedorID", c => c.Int(nullable: false));
            CreateIndex("dbo.VentasMayoristas", "VendedorID");
            AddForeignKey("dbo.VentasMayoristas", "VendedorID", "dbo.Vendedor", "ID", cascadeDelete: true);
        }
    }
}
