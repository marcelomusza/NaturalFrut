namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedingTipoCliente : DbMigration
    {
        public override void Up()
        {
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
