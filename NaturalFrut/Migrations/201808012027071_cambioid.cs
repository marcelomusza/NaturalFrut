namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cambioid : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ListaPrecios", "ListaID", "dbo.Listas");
            DropIndex("dbo.ListaPrecios", new[] { "ListaID" });
            AlterColumn("dbo.ListaPrecios", "ListaID", c => c.Int(nullable: false));
            CreateIndex("dbo.ListaPrecios", "ListaID");
            AddForeignKey("dbo.ListaPrecios", "ListaID", "dbo.Listas", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ListaPrecios", "ListaID", "dbo.Listas");
            DropIndex("dbo.ListaPrecios", new[] { "ListaID" });
            AlterColumn("dbo.ListaPrecios", "ListaID", c => c.Int());
            CreateIndex("dbo.ListaPrecios", "ListaID");
            AddForeignKey("dbo.ListaPrecios", "ListaID", "dbo.Listas", "ID");
        }
    }
}
