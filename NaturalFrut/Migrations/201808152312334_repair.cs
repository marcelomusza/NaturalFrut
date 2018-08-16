namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class repair : DbMigration
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
            AlterColumn("dbo.ListaPrecios", "PrecioXUnidad", c => c.Double(nullable: false));
            AlterColumn("dbo.ListaPrecios", "KGBultoCerrado", c => c.Double(nullable: false));
            AlterColumn("dbo.ListaPrecios", "PrecioXBultoCerrado", c => c.Double(nullable: false));
            AlterColumn("dbo.ListaPrecios", "PrecioXKG", c => c.Double(nullable: false));
        }
    }
}
