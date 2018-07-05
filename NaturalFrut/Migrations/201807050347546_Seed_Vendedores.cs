namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Seed_Vendedores : DbMigration
    {
        public override void Up()
        {

            Sql("INSERT INTO [dbo].[Vendedor] VALUES ('Roberto', 'Bolanoz', 'rob@asdas.com', '1212131')");
            Sql("INSERT INTO [dbo].[Vendedor] VALUES ('Mariano', 'Joz', 'rob@asdas.com', '1212131')");
            Sql("INSERT INTO [dbo].[Vendedor] VALUES ('Mariana', 'Ravio', 'rob@asdas.com', '1212131')");
            Sql("INSERT INTO [dbo].[Vendedor] VALUES ('Julian', 'Neres', 'rob@asdas.com', '1212131')");
            Sql("INSERT INTO [dbo].[Vendedor] VALUES ('Adrian', 'Campero', 'rob@asdas.com', '1212131')");
            Sql("INSERT INTO [dbo].[Vendedor] VALUES ('Andres', 'Peasz', 'rob@asdas.com', '1212131')");
            Sql("INSERT INTO [dbo].[Vendedor] VALUES ('Valeria', 'Malez', 'rob@asdas.com', '1212131')");
            Sql("INSERT INTO [dbo].[Vendedor] VALUES ('Daniel', 'Porta', 'rob@asdas.com', '1212131')");
            Sql("INSERT INTO [dbo].[Vendedor] VALUES ('Federico', 'Nioqui', 'rob@asdas.com', '1212131')");


        }
        
        public override void Down()
        {
        }
    }
}
