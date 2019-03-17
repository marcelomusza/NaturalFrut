namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cliente_ReqNombreYDire_FIX : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Clientes", "ListaId", "dbo.Listas");
            DropForeignKey("dbo.Clientes", "CondicionIVAId", "dbo.CondicionIVAs");
            DropForeignKey("dbo.Clientes", "TipoClienteId", "dbo.TipoClientes");
            DropIndex("dbo.Clientes", new[] { "CondicionIVAId" });
            DropIndex("dbo.Clientes", new[] { "TipoClienteId" });
            DropIndex("dbo.Clientes", new[] { "ListaId" });
            AlterColumn("dbo.Clientes", "CondicionIVAId", c => c.Int());
            AlterColumn("dbo.Clientes", "TipoClienteId", c => c.Int());
            AlterColumn("dbo.Clientes", "ListaId", c => c.Int());
            CreateIndex("dbo.Clientes", "CondicionIVAId");
            CreateIndex("dbo.Clientes", "TipoClienteId");
            CreateIndex("dbo.Clientes", "ListaId");
            AddForeignKey("dbo.Clientes", "ListaId", "dbo.Listas", "ID");
            AddForeignKey("dbo.Clientes", "CondicionIVAId", "dbo.CondicionIVAs", "ID");
            AddForeignKey("dbo.Clientes", "TipoClienteId", "dbo.TipoClientes", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Clientes", "TipoClienteId", "dbo.TipoClientes");
            DropForeignKey("dbo.Clientes", "CondicionIVAId", "dbo.CondicionIVAs");
            DropForeignKey("dbo.Clientes", "ListaId", "dbo.Listas");
            DropIndex("dbo.Clientes", new[] { "ListaId" });
            DropIndex("dbo.Clientes", new[] { "TipoClienteId" });
            DropIndex("dbo.Clientes", new[] { "CondicionIVAId" });
            AlterColumn("dbo.Clientes", "ListaId", c => c.Int(nullable: false));
            AlterColumn("dbo.Clientes", "TipoClienteId", c => c.Int(nullable: false));
            AlterColumn("dbo.Clientes", "CondicionIVAId", c => c.Int(nullable: false));
            CreateIndex("dbo.Clientes", "ListaId");
            CreateIndex("dbo.Clientes", "TipoClienteId");
            CreateIndex("dbo.Clientes", "CondicionIVAId");
            AddForeignKey("dbo.Clientes", "TipoClienteId", "dbo.TipoClientes", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Clientes", "CondicionIVAId", "dbo.CondicionIVAs", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Clientes", "ListaId", "dbo.Listas", "ID", cascadeDelete: true);
        }
    }
}
