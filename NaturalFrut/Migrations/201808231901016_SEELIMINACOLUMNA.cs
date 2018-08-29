namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SEELIMINACOLUMNA : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Clientes", "Horarios");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clientes", "Horarios", c => c.DateTime(nullable: false));
        }
    }
}
