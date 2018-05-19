namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class productoTablaNuevoNombre : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Productoes", newName: "Producto");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Producto", newName: "Productoes");
        }
    }
}
