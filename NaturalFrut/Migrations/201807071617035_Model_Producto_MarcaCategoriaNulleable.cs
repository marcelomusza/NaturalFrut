namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Model_Producto_MarcaCategoriaNulleable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Productos", "CategoriaId", "dbo.Categorias");
            DropForeignKey("dbo.Productos", "MarcaId", "dbo.Marcas");
            DropIndex("dbo.Productos", new[] { "CategoriaId" });
            DropIndex("dbo.Productos", new[] { "MarcaId" });
            AlterColumn("dbo.Productos", "CategoriaId", c => c.Int());
            AlterColumn("dbo.Productos", "MarcaId", c => c.Int());
            CreateIndex("dbo.Productos", "CategoriaId");
            CreateIndex("dbo.Productos", "MarcaId");
            AddForeignKey("dbo.Productos", "CategoriaId", "dbo.Categorias", "ID");
            AddForeignKey("dbo.Productos", "MarcaId", "dbo.Marcas", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Productos", "MarcaId", "dbo.Marcas");
            DropForeignKey("dbo.Productos", "CategoriaId", "dbo.Categorias");
            DropIndex("dbo.Productos", new[] { "MarcaId" });
            DropIndex("dbo.Productos", new[] { "CategoriaId" });
            AlterColumn("dbo.Productos", "MarcaId", c => c.Int(nullable: false));
            AlterColumn("dbo.Productos", "CategoriaId", c => c.Int(nullable: false));
            CreateIndex("dbo.Productos", "MarcaId");
            CreateIndex("dbo.Productos", "CategoriaId");
            AddForeignKey("dbo.Productos", "MarcaId", "dbo.Marcas", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Productos", "CategoriaId", "dbo.Categorias", "ID", cascadeDelete: true);
        }
    }
}
