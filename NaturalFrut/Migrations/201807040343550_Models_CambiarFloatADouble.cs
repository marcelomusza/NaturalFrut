namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Models_CambiarFloatADouble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ListaPrecios", "PrecioXKG", c => c.Double(nullable: false));
            AlterColumn("dbo.ListaPrecios", "PrecioXBultoCerrado", c => c.Double(nullable: false));
            AlterColumn("dbo.ListaPrecios", "KGBultoCerrado", c => c.Double(nullable: false));
            AlterColumn("dbo.ListaPrecios", "PrecioXUnidad", c => c.Double(nullable: false));
            AlterColumn("dbo.ProductosXVenta", "Importe", c => c.Double(nullable: false));
            AlterColumn("dbo.ProductosXVenta", "Total", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProductosXVenta", "Total", c => c.Single(nullable: false));
            AlterColumn("dbo.ProductosXVenta", "Importe", c => c.Single(nullable: false));
            AlterColumn("dbo.ListaPrecios", "PrecioXUnidad", c => c.Single(nullable: false));
            AlterColumn("dbo.ListaPrecios", "KGBultoCerrado", c => c.Single(nullable: false));
            AlterColumn("dbo.ListaPrecios", "PrecioXBultoCerrado", c => c.Single(nullable: false));
            AlterColumn("dbo.ListaPrecios", "PrecioXKG", c => c.Single(nullable: false));
        }
    }
}
