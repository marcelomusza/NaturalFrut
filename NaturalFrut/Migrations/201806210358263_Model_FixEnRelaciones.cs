namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Model_FixEnRelaciones : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ListaDePrecios", "Cliente_ID", "dbo.Clientes");
            DropForeignKey("dbo.ListaDePrecios", "TipoDeUnidad_ID", "dbo.TiposDeUnidad");
            DropForeignKey("dbo.ListaDePrecios", "Producto_ID", "dbo.Productos");
            DropIndex("dbo.ListaDePrecios", new[] { "Cliente_ID" });
            DropIndex("dbo.ListaDePrecios", new[] { "TipoDeUnidad_ID" });
            DropIndex("dbo.ListaDePrecios", new[] { "Producto_ID" });
            DropColumn("dbo.ListaDePrecios", "Cliente_ID");
            DropColumn("dbo.ListaDePrecios", "TipoDeUnidad_ID");
            DropColumn("dbo.ListaDePrecios", "Producto_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ListaDePrecios", "Producto_ID", c => c.Int());
            AddColumn("dbo.ListaDePrecios", "TipoDeUnidad_ID", c => c.Int());
            AddColumn("dbo.ListaDePrecios", "Cliente_ID", c => c.Int());
            CreateIndex("dbo.ListaDePrecios", "Producto_ID");
            CreateIndex("dbo.ListaDePrecios", "TipoDeUnidad_ID");
            CreateIndex("dbo.ListaDePrecios", "Cliente_ID");
            AddForeignKey("dbo.ListaDePrecios", "Producto_ID", "dbo.Productos", "ID");
            AddForeignKey("dbo.ListaDePrecios", "TipoDeUnidad_ID", "dbo.TiposDeUnidad", "ID");
            AddForeignKey("dbo.ListaDePrecios", "Cliente_ID", "dbo.Clientes", "ID");
        }
    }
}
