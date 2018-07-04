namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Seed_ListaPrincipal : DbMigration
    {
        public override void Up()
        {
            
            Sql("Insert into Listas values('Lista Principal', 0)");

        }
        
        public override void Down()
        {
        }
    }
}
