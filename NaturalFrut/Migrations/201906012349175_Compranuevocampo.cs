namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Compranuevocampo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Compra", "EntregaEfectivo", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Compra", "EntregaEfectivo");
        }
    }
}
