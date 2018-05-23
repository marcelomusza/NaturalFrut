namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NuevoModelCategorias : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categorias",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Producto", "CategoriaId", c => c.Int(nullable: false));
            CreateIndex("dbo.Producto", "CategoriaId");
            AddForeignKey("dbo.Producto", "CategoriaId", "dbo.Categorias", "ID", cascadeDelete: true);
            DropColumn("dbo.Producto", "Categoria");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Producto", "Categoria", c => c.String(nullable: false));
            DropForeignKey("dbo.Producto", "CategoriaId", "dbo.Categorias");
            DropIndex("dbo.Producto", new[] { "CategoriaId" });
            DropColumn("dbo.Producto", "CategoriaId");
            DropTable("dbo.Categorias");
        }
    }
}
