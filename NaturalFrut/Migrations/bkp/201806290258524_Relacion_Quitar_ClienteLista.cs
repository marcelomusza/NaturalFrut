namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Relacion_Quitar_ClienteLista : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Listas", "ID", "dbo.Clientes");
            DropForeignKey("dbo.ListaPrecios", "ListaID", "dbo.Listas");
            DropIndex("dbo.Listas", new[] { "ID" });
            DropPrimaryKey("dbo.Listas");
            AlterColumn("dbo.Listas", "ID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Listas", "ID");
            AddForeignKey("dbo.ListaPrecios", "ListaID", "dbo.Listas", "ID", cascadeDelete: true);
            DropColumn("dbo.Clientes", "ListaID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clientes", "ListaID", c => c.Int(nullable: false));
            DropForeignKey("dbo.ListaPrecios", "ListaID", "dbo.Listas");
            DropPrimaryKey("dbo.Listas");
            AlterColumn("dbo.Listas", "ID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Listas", "ID");
            CreateIndex("dbo.Listas", "ID");
            AddForeignKey("dbo.ListaPrecios", "ListaID", "dbo.Listas", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Listas", "ID", "dbo.Clientes", "ID");
        }
    }
}
