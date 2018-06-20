namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Relacion_ProductoXVentaYTipoDeUnidad : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.TiposDeUnidad");
            AddColumn("dbo.ProductosXVenta", "TipoDeUnidadID", c => c.Int(nullable: false));
            AlterColumn("dbo.TiposDeUnidad", "ID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.TiposDeUnidad", "ID");
            CreateIndex("dbo.TiposDeUnidad", "ID");
            AddForeignKey("dbo.TiposDeUnidad", "ID", "dbo.ProductosXVenta", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TiposDeUnidad", "ID", "dbo.ProductosXVenta");
            DropIndex("dbo.TiposDeUnidad", new[] { "ID" });
            DropPrimaryKey("dbo.TiposDeUnidad");
            AlterColumn("dbo.TiposDeUnidad", "ID", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.ProductosXVenta", "TipoDeUnidadID");
            AddPrimaryKey("dbo.TiposDeUnidad", "ID");
        }
    }
}
