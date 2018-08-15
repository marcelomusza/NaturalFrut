namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedingCondicionIva : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO CondicionIVAs values ('Responsable Inscripto')");
            Sql("INSERT INTO CondicionIVAs values ('Monotributo')");
            Sql("INSERT INTO CondicionIVAs values ('Consumidor Final')");
        }
        
        public override void Down()
        {
        }
    }
}
