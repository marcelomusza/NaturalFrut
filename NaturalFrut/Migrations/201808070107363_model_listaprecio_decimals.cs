namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class model_listaprecio_decimals : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ListaPrecios", "PrecioXBultoCerrado", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ListaPrecios", "KGBultoCerrado", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ListaPrecios", "PrecioXUnidad", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ListaPrecios", "PrecioXUnidad", c => c.String(nullable: false));
            AlterColumn("dbo.ListaPrecios", "KGBultoCerrado", c => c.String(nullable: false));
            AlterColumn("dbo.ListaPrecios", "PrecioXBultoCerrado", c => c.String(nullable: false));
        }
    }
}
