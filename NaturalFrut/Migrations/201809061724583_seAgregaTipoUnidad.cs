namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seAgregaTipoUnidad : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductosXCompra", "TipoDeUnidadID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductosXCompra", "TipoDeUnidadID");
        }
    }
}
