namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CambiosYesi : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Compra", "NoConcretado", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Compra", "NoConcretado");
        }
    }
}
