namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class model_vtamayorista_tipodescuento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VentasMayoristas", "TipoDescuentoTotal", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.VentasMayoristas", "TipoDescuentoTotal");
        }
    }
}
