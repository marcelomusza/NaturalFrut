namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cambioAStringnuevo : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ListaPrecios", "PrecioXKG", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ListaPrecios", "PrecioXKG", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
