namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seAgregaLocal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Compra", "Local", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Compra", "Local");
        }
    }
}
