namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modelMarcaProductoCategoria : DbMigration
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
            
            
            
            CreateTable(
                "dbo.Marcas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.Productos",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Descripcion = c.String(nullable: false),
                    Cantidad = c.Int(nullable: false),
                    CategoriaId = c.Int(nullable: false),
                    MarcaId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Categorias", t => t.CategoriaId, cascadeDelete: true)
                .ForeignKey("dbo.Marcas", t => t.MarcaId, cascadeDelete: true)
                .Index(t => t.CategoriaId)
                .Index(t => t.MarcaId);

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Productos", "MarcaId", "dbo.Marcas");
            DropForeignKey("dbo.Productos", "CategoriaId", "dbo.Categorias");
            DropIndex("dbo.Productos", new[] { "MarcaId" });
            DropIndex("dbo.Productos", new[] { "CategoriaId" });
            DropTable("dbo.Marcas");
            DropTable("dbo.Productos");
            DropTable("dbo.Categorias");
        }
    }
}
