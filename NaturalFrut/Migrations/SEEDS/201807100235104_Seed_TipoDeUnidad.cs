namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Seed_TipoDeUnidad : DbMigration
    {
        public override void Up()
        {

            Sql("INSERT INTO TiposDeUnidad Values('Kg')");
            Sql("INSERT INTO TiposDeUnidad Values('Bulto Cerrado')");
            Sql("INSERT INTO TiposDeUnidad Values('Unidad')");

        }
        
        public override void Down()
        {
        }
    }
}
