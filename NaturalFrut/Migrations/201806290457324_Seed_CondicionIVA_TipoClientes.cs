namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Seed_CondicionIVA_TipoClientes : DbMigration
    {
        public override void Up()
        {

            Sql("INSERT INTO CondicionIVAs values ('Responsable Inscripto')");
            Sql("INSERT INTO CondicionIVAs values ('Monotributo')");
            Sql("INSERT INTO CondicionIVAs values ('Consumidor Final')");


            Sql("INSERT INTO TipoClientes VALUES ('Dietética')");
            Sql("INSERT INTO TipoClientes VALUES ('Verdulería')");
            Sql("INSERT INTO TipoClientes VALUES ('Supermercado')");
            Sql("INSERT INTO TipoClientes VALUES ('Distribuidor/Mayorista')");

        }
        
        public override void Down()
        {
        }
    }
}
