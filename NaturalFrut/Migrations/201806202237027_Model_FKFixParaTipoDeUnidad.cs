namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Model_FKFixParaTipoDeUnidad : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TiposDeUnidad", "ID", "dbo.ProductosXVenta");
            DropForeignKey("dbo.ListaDePrecios", "TipoDeUnidadID", "dbo.TiposDeUnidad");
            DropIndex("dbo.TiposDeUnidad", new[] { "ID" });
            DropPrimaryKey("dbo.TiposDeUnidad");
            AlterColumn("dbo.TiposDeUnidad", "ID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.TiposDeUnidad", "ID");
            CreateIndex("dbo.ProductosXVenta", "TipoDeUnidadID");
            AddForeignKey("dbo.ProductosXVenta", "TipoDeUnidadID", "dbo.TiposDeUnidad", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ListaDePrecios", "TipoDeUnidadID", "dbo.TiposDeUnidad", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ListaDePrecios", "TipoDeUnidadID", "dbo.TiposDeUnidad");
            DropForeignKey("dbo.ProductosXVenta", "TipoDeUnidadID", "dbo.TiposDeUnidad");
            DropIndex("dbo.ProductosXVenta", new[] { "TipoDeUnidadID" });
            DropPrimaryKey("dbo.TiposDeUnidad");
            AlterColumn("dbo.TiposDeUnidad", "ID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.TiposDeUnidad", "ID");
            CreateIndex("dbo.TiposDeUnidad", "ID");
            AddForeignKey("dbo.ListaDePrecios", "TipoDeUnidadID", "dbo.TiposDeUnidad", "ID", cascadeDelete: true);
            AddForeignKey("dbo.TiposDeUnidad", "ID", "dbo.ProductosXVenta", "ID");
        }
    }
}
