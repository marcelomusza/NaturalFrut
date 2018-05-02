namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class crearTableCondicionIva : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CondicionIVAs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.TipoClientes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Clientes", "RazonSocial", c => c.String(nullable: false));
            AddColumn("dbo.Clientes", "Cuit", c => c.String(nullable: false));
            AddColumn("dbo.Clientes", "Iibb", c => c.String(nullable: false));
            AddColumn("dbo.Clientes", "Direccion", c => c.String(nullable: false));
            AddColumn("dbo.Clientes", "Provincia", c => c.String(nullable: false));
            AddColumn("dbo.Clientes", "Localidad", c => c.String(nullable: false));
            AddColumn("dbo.Clientes", "TelefonoNegocio", c => c.Int(nullable: false));
            AddColumn("dbo.Clientes", "TelefonoCelular", c => c.Int(nullable: false));
            AddColumn("dbo.Clientes", "Email", c => c.String(nullable: false));
            AddColumn("dbo.Clientes", "CondicionIVA_ID", c => c.Int());
            AddColumn("dbo.Clientes", "TipoCliente_ID", c => c.Int());
            CreateIndex("dbo.Clientes", "CondicionIVA_ID");
            CreateIndex("dbo.Clientes", "TipoCliente_ID");
            AddForeignKey("dbo.Clientes", "CondicionIVA_ID", "dbo.CondicionIVAs", "ID");
            AddForeignKey("dbo.Clientes", "TipoCliente_ID", "dbo.TipoClientes", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Clientes", "TipoCliente_ID", "dbo.TipoClientes");
            DropForeignKey("dbo.Clientes", "CondicionIVA_ID", "dbo.CondicionIVAs");
            DropIndex("dbo.Clientes", new[] { "TipoCliente_ID" });
            DropIndex("dbo.Clientes", new[] { "CondicionIVA_ID" });
            DropColumn("dbo.Clientes", "TipoCliente_ID");
            DropColumn("dbo.Clientes", "CondicionIVA_ID");
            DropColumn("dbo.Clientes", "Email");
            DropColumn("dbo.Clientes", "TelefonoCelular");
            DropColumn("dbo.Clientes", "TelefonoNegocio");
            DropColumn("dbo.Clientes", "Localidad");
            DropColumn("dbo.Clientes", "Provincia");
            DropColumn("dbo.Clientes", "Direccion");
            DropColumn("dbo.Clientes", "Iibb");
            DropColumn("dbo.Clientes", "Cuit");
            DropColumn("dbo.Clientes", "RazonSocial");
            DropTable("dbo.TipoClientes");
            DropTable("dbo.CondicionIVAs");
        }
    }
}
