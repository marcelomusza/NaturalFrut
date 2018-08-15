namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Model_Producto_EsBlister : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Productos", "EsBlister", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Productos", "EsBlister");
        }
    }
}
