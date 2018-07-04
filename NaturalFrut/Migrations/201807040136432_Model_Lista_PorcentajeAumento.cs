namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Model_Lista_PorcentajeAumento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Listas", "PorcentajeAumento", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Listas", "PorcentajeAumento");
        }
    }
}
