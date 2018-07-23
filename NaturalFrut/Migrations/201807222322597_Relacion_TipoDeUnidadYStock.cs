namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Relacion_TipoDeUnidadYStock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stock", "TipoDeUnidadID", c => c.Int(nullable: false));
            AddColumn("dbo.Stock", "Cantidad", c => c.Int(nullable: false));
            CreateIndex("dbo.Stock", "TipoDeUnidadID");
            AddForeignKey("dbo.Stock", "TipoDeUnidadID", "dbo.TiposDeUnidad", "ID", cascadeDelete: true);
            DropColumn("dbo.Stock", "CantidadXKg");
            DropColumn("dbo.Stock", "CantidadXUnidad");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Stock", "CantidadXUnidad", c => c.Int(nullable: false));
            AddColumn("dbo.Stock", "CantidadXKg", c => c.Int(nullable: false));
            DropForeignKey("dbo.Stock", "TipoDeUnidadID", "dbo.TiposDeUnidad");
            DropIndex("dbo.Stock", new[] { "TipoDeUnidadID" });
            DropColumn("dbo.Stock", "Cantidad");
            DropColumn("dbo.Stock", "TipoDeUnidadID");
        }
    }
}
