namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Seed_Listas : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO [dbo].[Listas] VALUES('Lista Principal', 0)");
            Sql("INSERT INTO [dbo].[Listas] VALUES('Lista Uno', 10)");
            Sql("INSERT INTO [dbo].[Listas] VALUES('Lista Dos', 25)");
            Sql("INSERT INTO [dbo].[Listas] VALUES('Lista Tres', 50)");

        }
        
        public override void Down()
        {
        }
    }
}
