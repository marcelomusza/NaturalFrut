namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sePasaAString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ListaPrecios", "PrecioXKG", c => c.String(nullable: false));
            AlterColumn("dbo.ListaPrecios", "PrecioXBultoCerrado", c => c.String(nullable: false));
            AlterColumn("dbo.ListaPrecios", "KGBultoCerrado", c => c.String(nullable: false));
            AlterColumn("dbo.ListaPrecios", "PrecioXUnidad", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ListaPrecios", "PrecioXUnidad", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ListaPrecios", "KGBultoCerrado", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ListaPrecios", "PrecioXBultoCerrado", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ListaPrecios", "PrecioXKG", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
