namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cambioTipo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clientes", "Horarios", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clientes", "Horarios");
        }
    }
}
