namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stocknuevascol : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stock", "ProductoAuxiliar", c => c.String());
            AddColumn("dbo.Stock", "TipoDeUnidadAuxiliar", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stock", "TipoDeUnidadAuxiliar");
            DropColumn("dbo.Stock", "ProductoAuxiliar");
        }
    }
}
