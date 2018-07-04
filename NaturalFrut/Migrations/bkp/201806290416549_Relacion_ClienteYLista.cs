namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Relacion_ClienteYLista : DbMigration
    {
        public override void Up()
        {
            
            AddColumn("dbo.Clientes", "ListaId", c => c.Int(nullable: false));
            CreateIndex("dbo.Clientes", "ListaId");
            AddForeignKey("dbo.Clientes", "ListaId", "dbo.Listas", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Clientes", "ListaId", "dbo.Listas");
            DropIndex("dbo.Clientes", new[] { "ListaId" });
            DropColumn("dbo.Clientes", "ListaId");
        }
    }
}
