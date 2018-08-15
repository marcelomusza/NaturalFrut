namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedingTipoCliente : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO TipoClientes VALUES ('Diet�tica')");
            Sql("INSERT INTO TipoClientes VALUES ('Verduler�a')");
            Sql("INSERT INTO TipoClientes VALUES ('Supermercado')");
            Sql("INSERT INTO TipoClientes VALUES ('Distribuidor/Mayorista')");
        }
        
        public override void Down()
        {
        }
    }
}
