namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixTablaProductos : DbMigration
    {
        public override void Up()
        {

            CreateTable(
                "dbo.Producto",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Descripcion = c.String(nullable: false),
                    Cantidad = c.Int(nullable: false),
                    Categoria = c.String(nullable: false),
                    Marca = c.String(nullable: false),
                })
                .PrimaryKey(t => t.ID);

            DropTable("dbo.Producto");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Producto",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        Categoria = c.String(nullable: false),
                        Marca = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
    }
}
