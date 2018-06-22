namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Model_ProductosXLista_TableNameChange : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ProductosXListas", newName: "ProductosXLista");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.ProductosXLista", newName: "ProductosXListas");
        }
    }
}
