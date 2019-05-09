namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductoNombreAuxiliar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Productos", "NombreAuxiliar", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Productos", "NombreAuxiliar");
        }
    }
}
