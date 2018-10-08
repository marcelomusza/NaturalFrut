namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class model_vtamayoristanuevosaldo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VentasMayoristas", "NuevoSaldo", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.VentasMayoristas", "NuevoSaldo");
        }
    }
}
