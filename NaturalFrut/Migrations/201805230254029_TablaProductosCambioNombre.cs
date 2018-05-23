namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TablaProductosCambioNombre : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Producto", newName: "Productos");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Productos", newName: "Producto");
        }
    }
}
