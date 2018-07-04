namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Relacion_Cliente_Lista : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Listas", "ClienteID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Listas", "ClienteID", c => c.Int(nullable: false));
        }
    }
}
