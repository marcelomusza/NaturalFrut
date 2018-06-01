namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class relacionMarcaProducto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Productos", "MarcaId", c => c.Int(nullable: false));
            CreateIndex("dbo.Productos", "MarcaId");
            AddForeignKey("dbo.Productos", "MarcaId", "dbo.Marcas", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Productos", "MarcaId", "dbo.Marcas");
            DropIndex("dbo.Productos", new[] { "MarcaId" });
            DropColumn("dbo.Productos", "MarcaId");
        }
    }
}
