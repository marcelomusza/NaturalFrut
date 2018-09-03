namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seAgregaClasificacion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clasificacions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Clasificacions");
        }
    }
}
