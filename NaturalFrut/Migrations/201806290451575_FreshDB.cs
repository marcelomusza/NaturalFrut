namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FreshDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categorias",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Productos",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        CategoriaId = c.Int(nullable: false),
                        MarcaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Categorias", t => t.CategoriaId, cascadeDelete: true)
                .ForeignKey("dbo.Marcas", t => t.MarcaId, cascadeDelete: true)
                .Index(t => t.CategoriaId)
                .Index(t => t.MarcaId);
            
            CreateTable(
                "dbo.ListaPrecios",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ListaID = c.Int(nullable: false),
                        ProductoID = c.Int(nullable: false),
                        PrecioXKG = c.Single(nullable: false),
                        PrecioXBultoCerrado = c.Single(nullable: false),
                        KGBultoCerrado = c.Single(nullable: false),
                        PrecioXUnidad = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Listas", t => t.ListaID, cascadeDelete: true)
                .ForeignKey("dbo.Productos", t => t.ProductoID, cascadeDelete: true)
                .Index(t => t.ListaID)
                .Index(t => t.ProductoID);
            
            CreateTable(
                "dbo.Listas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Clientes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        RazonSocial = c.String(nullable: false),
                        Cuit = c.String(nullable: false),
                        Iibb = c.String(nullable: false),
                        Direccion = c.String(nullable: false),
                        Provincia = c.String(nullable: false),
                        Localidad = c.String(nullable: false),
                        TelefonoNegocio = c.Int(nullable: false),
                        TelefonoCelular = c.Int(nullable: false),
                        Email = c.String(nullable: false),
                        CondicionIVAId = c.Int(nullable: false),
                        TipoClienteId = c.Int(nullable: false),
                        ListaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CondicionIVAs", t => t.CondicionIVAId, cascadeDelete: true)
                .ForeignKey("dbo.Listas", t => t.ListaId, cascadeDelete: true)
                .ForeignKey("dbo.TipoClientes", t => t.TipoClienteId, cascadeDelete: true)
                .Index(t => t.CondicionIVAId)
                .Index(t => t.TipoClienteId)
                .Index(t => t.ListaId);
            
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
            
            CreateTable(
                "dbo.Marcas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProductosXVenta",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Cantidad = c.Int(nullable: false),
                        Importe = c.Single(nullable: false),
                        Total = c.Single(nullable: false),
                        TipoDeUnidadID = c.Int(nullable: false),
                        ProductoID = c.Int(nullable: false),
                        VentaID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Productos", t => t.ProductoID, cascadeDelete: true)
                .ForeignKey("dbo.TiposDeUnidad", t => t.TipoDeUnidadID, cascadeDelete: true)
                .ForeignKey("dbo.Ventas", t => t.VentaID, cascadeDelete: true)
                .Index(t => t.TipoDeUnidadID)
                .Index(t => t.ProductoID)
                .Index(t => t.VentaID);
            
            CreateTable(
                "dbo.TiposDeUnidad",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Ventas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        Impreso = c.Boolean(nullable: false),
                        NoConcretado = c.Boolean(nullable: false),
                        EntregaEfectivo = c.Boolean(nullable: false),
                        Descuento = c.Int(nullable: false),
                        ClienteID = c.Int(nullable: false),
                        VendedorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Clientes", t => t.ClienteID, cascadeDelete: true)
                .ForeignKey("dbo.Vendedor", t => t.VendedorID, cascadeDelete: true)
                .Index(t => t.ClienteID)
                .Index(t => t.VendedorID);
            
            CreateTable(
                "dbo.Vendedor",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        Apellido = c.String(nullable: false),
                        Email = c.String(),
                        Telefono = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Proveedores",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        RazonSocial = c.String(nullable: false),
                        Saldo = c.Double(nullable: false),
                        Cuit = c.String(nullable: false),
                        Iibb = c.String(nullable: false),
                        Direccion = c.String(nullable: false),
                        Provincia = c.String(nullable: false),
                        Localidad = c.String(nullable: false),
                        TelefonoNegocio = c.Int(nullable: false),
                        TelefonoCelular = c.Int(nullable: false),
                        Email = c.String(nullable: false),
                        CondicionIVAId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CondicionIVAs", t => t.CondicionIVAId, cascadeDelete: true)
                .Index(t => t.CondicionIVAId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Proveedores", "CondicionIVAId", "dbo.CondicionIVAs");
            DropForeignKey("dbo.Ventas", "VendedorID", "dbo.Vendedor");
            DropForeignKey("dbo.ProductosXVenta", "VentaID", "dbo.Ventas");
            DropForeignKey("dbo.Ventas", "ClienteID", "dbo.Clientes");
            DropForeignKey("dbo.ProductosXVenta", "TipoDeUnidadID", "dbo.TiposDeUnidad");
            DropForeignKey("dbo.ProductosXVenta", "ProductoID", "dbo.Productos");
            DropForeignKey("dbo.Productos", "MarcaId", "dbo.Marcas");
            DropForeignKey("dbo.ListaPrecios", "ProductoID", "dbo.Productos");
            DropForeignKey("dbo.ListaPrecios", "ListaID", "dbo.Listas");
            DropForeignKey("dbo.Clientes", "TipoClienteId", "dbo.TipoClientes");
            DropForeignKey("dbo.Clientes", "ListaId", "dbo.Listas");
            DropForeignKey("dbo.Clientes", "CondicionIVAId", "dbo.CondicionIVAs");
            DropForeignKey("dbo.Productos", "CategoriaId", "dbo.Categorias");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Proveedores", new[] { "CondicionIVAId" });
            DropIndex("dbo.Ventas", new[] { "VendedorID" });
            DropIndex("dbo.Ventas", new[] { "ClienteID" });
            DropIndex("dbo.ProductosXVenta", new[] { "VentaID" });
            DropIndex("dbo.ProductosXVenta", new[] { "ProductoID" });
            DropIndex("dbo.ProductosXVenta", new[] { "TipoDeUnidadID" });
            DropIndex("dbo.Clientes", new[] { "ListaId" });
            DropIndex("dbo.Clientes", new[] { "TipoClienteId" });
            DropIndex("dbo.Clientes", new[] { "CondicionIVAId" });
            DropIndex("dbo.ListaPrecios", new[] { "ProductoID" });
            DropIndex("dbo.ListaPrecios", new[] { "ListaID" });
            DropIndex("dbo.Productos", new[] { "MarcaId" });
            DropIndex("dbo.Productos", new[] { "CategoriaId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Proveedores");
            DropTable("dbo.Vendedor");
            DropTable("dbo.Ventas");
            DropTable("dbo.TiposDeUnidad");
            DropTable("dbo.ProductosXVenta");
            DropTable("dbo.Marcas");
            DropTable("dbo.TipoClientes");
            DropTable("dbo.CondicionIVAs");
            DropTable("dbo.Clientes");
            DropTable("dbo.Listas");
            DropTable("dbo.ListaPrecios");
            DropTable("dbo.Productos");
            DropTable("dbo.Categorias");
        }
    }
}
