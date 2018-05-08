namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cambiosEnModelClienteYRelaciones : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Clientes", "CondicionIVA_ID", "dbo.CondicionIVAs");
            DropForeignKey("dbo.Clientes", "TipoCliente_ID", "dbo.TipoClientes");
            DropIndex("dbo.Clientes", new[] { "CondicionIVA_ID" });
            DropIndex("dbo.Clientes", new[] { "TipoCliente_ID" });
            DropColumn("dbo.Clientes", "CondicionIVAId");
            DropColumn("dbo.Clientes", "TipoClienteId");
            RenameColumn(table: "dbo.Clientes", name: "CondicionIVA_ID", newName: "CondicionIVAId");
            RenameColumn(table: "dbo.Clientes", name: "TipoCliente_ID", newName: "TipoClienteId");
            AlterColumn("dbo.Clientes", "CondicionIVAId", c => c.Int(nullable: false));
            AlterColumn("dbo.Clientes", "TipoClienteId", c => c.Int(nullable: false));
            AlterColumn("dbo.Clientes", "CondicionIVAId", c => c.Int(nullable: false));
            AlterColumn("dbo.Clientes", "TipoClienteId", c => c.Int(nullable: false));
            CreateIndex("dbo.Clientes", "CondicionIVAId");
            CreateIndex("dbo.Clientes", "TipoClienteId");
            AddForeignKey("dbo.Clientes", "CondicionIVAId", "dbo.CondicionIVAs", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Clientes", "TipoClienteId", "dbo.TipoClientes", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Clientes", "TipoClienteId", "dbo.TipoClientes");
            DropForeignKey("dbo.Clientes", "CondicionIVAId", "dbo.CondicionIVAs");
            DropIndex("dbo.Clientes", new[] { "TipoClienteId" });
            DropIndex("dbo.Clientes", new[] { "CondicionIVAId" });
            AlterColumn("dbo.Clientes", "TipoClienteId", c => c.Int());
            AlterColumn("dbo.Clientes", "CondicionIVAId", c => c.Int());
            AlterColumn("dbo.Clientes", "TipoClienteId", c => c.Byte(nullable: false));
            AlterColumn("dbo.Clientes", "CondicionIVAId", c => c.Byte(nullable: false));
            RenameColumn(table: "dbo.Clientes", name: "TipoClienteId", newName: "TipoCliente_ID");
            RenameColumn(table: "dbo.Clientes", name: "CondicionIVAId", newName: "CondicionIVA_ID");
            AddColumn("dbo.Clientes", "TipoClienteId", c => c.Byte(nullable: false));
            AddColumn("dbo.Clientes", "CondicionIVAId", c => c.Byte(nullable: false));
            CreateIndex("dbo.Clientes", "TipoCliente_ID");
            CreateIndex("dbo.Clientes", "CondicionIVA_ID");
            AddForeignKey("dbo.Clientes", "TipoCliente_ID", "dbo.TipoClientes", "ID");
            AddForeignKey("dbo.Clientes", "CondicionIVA_ID", "dbo.CondicionIVAs", "ID");
        }
    }
}
