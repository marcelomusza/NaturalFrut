namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seAgregaClasificacion1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Clasificacions", newName: "Clasificacion");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Clasificacion", newName: "Clasificacions");
        }
    }
}
