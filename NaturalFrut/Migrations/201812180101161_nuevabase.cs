namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nuevabase : DbMigration
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
                        CategoriaId = c.Int(),
                        MarcaId = c.Int(),
                        EsBlister = c.Boolean(nullable: false),
                        EsMix = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Categorias", t => t.CategoriaId)
                .ForeignKey("dbo.Marcas", t => t.MarcaId)
                .Index(t => t.CategoriaId)
                .Index(t => t.MarcaId);
            
            CreateTable(
                "dbo.ListaPrecios",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ListaID = c.Int(),
                        ProductoID = c.Int(nullable: false),
                        PrecioXKG = c.String(nullable: false),
                        PrecioXBultoCerrado = c.String(nullable: false),
                        KGBultoCerrado = c.String(nullable: false),
                        PrecioXUnidad = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Listas", t => t.ListaID)
                .ForeignKey("dbo.Productos", t => t.ProductoID, cascadeDelete: true)
                .Index(t => t.ListaID)
                .Index(t => t.ProductoID);
            
            CreateTable(
                "dbo.Listas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        PorcentajeAumento = c.Int(nullable: false),
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
                        Saldo = c.Double(),
                        SaldoParcial = c.Double(),
                        Email = c.String(nullable: false),
                        CondicionIVAId = c.Int(nullable: false),
                        TipoClienteId = c.Int(nullable: false),
                        ListaId = c.Int(nullable: false),
                        Transporte = c.String(nullable: false),
                        DireccionTransporte = c.String(nullable: false),
                        TelefonoTransporte = c.Int(nullable: false),
                        Dias = c.String(nullable: false),
                        Horarios = c.String(nullable: false),
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
                        Descuento = c.Double(),
                        Importe = c.Double(nullable: false),
                        Total = c.Double(nullable: false),
                        TipoDeUnidadID = c.Int(nullable: false),
                        ProductoID = c.Int(nullable: false),
                        VentaID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Productos", t => t.ProductoID, cascadeDelete: true)
                .ForeignKey("dbo.TiposDeUnidad", t => t.TipoDeUnidadID, cascadeDelete: true)
                .ForeignKey("dbo.VentasMayoristas", t => t.VentaID)
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
                "dbo.Stock",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProductoID = c.Int(nullable: false),
                        TipoDeUnidadID = c.Int(nullable: false),
                        Cantidad = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Productos", t => t.ProductoID, cascadeDelete: true)
                .ForeignKey("dbo.TiposDeUnidad", t => t.TipoDeUnidadID, cascadeDelete: true)
                .Index(t => t.ProductoID)
                .Index(t => t.TipoDeUnidadID);
            
            CreateTable(
                "dbo.VentasMayoristas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        Impreso = c.Boolean(nullable: false),
                        NoConcretado = c.Boolean(nullable: false),
                        Facturado = c.Boolean(nullable: false),
                        EntregaEfectivo = c.Double(nullable: false),
                        TipoDescuentoTotal = c.Int(),
                        Descuento = c.Double(),
                        Saldo = c.Double(),
                        SaldoParcial = c.Double(),
                        ClienteID = c.Int(nullable: false),
                        VendedorID = c.Int(nullable: false),
                        NumeroVenta = c.Int(nullable: false),
                        SumaTotal = c.Double(nullable: false),
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
                "dbo.Clasificacion",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Compra",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NumeroCompra = c.Int(nullable: false),
                        Factura = c.String(),
                        Fecha = c.DateTime(nullable: false),
                        Iva = c.Double(nullable: false),
                        SumaTotal = c.Double(nullable: false),
                        ImporteIva = c.Double(nullable: false),
                        DescuentoPorc = c.Double(nullable: false),
                        ImporteIibbbsas = c.Double(nullable: false),
                        Iibbbsas = c.Double(nullable: false),
                        Descuento = c.Double(nullable: false),
                        ImporteIibbcaba = c.Double(nullable: false),
                        Iibbcaba = c.Double(nullable: false),
                        Subtotal = c.Double(nullable: false),
                        ImportePercIva = c.Double(nullable: false),
                        PercIva = c.Double(nullable: false),
                        ImporteNoGravado = c.Double(nullable: false),
                        Total = c.Double(nullable: false),
                        TotalGastos = c.Double(nullable: false),
                        TipoFactura = c.String(),
                        Local = c.String(),
                        NoConcretado = c.Boolean(nullable: false),
                        ProveedorID = c.Int(nullable: false),
                        ClasificacionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Clasificacion", t => t.ClasificacionID, cascadeDelete: true)
                .ForeignKey("dbo.Proveedores", t => t.ProveedorID, cascadeDelete: true)
                .Index(t => t.ProveedorID)
                .Index(t => t.ClasificacionID);
            
            CreateTable(
                "dbo.ProductosXCompra",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Cantidad = c.Int(nullable: false),
                        Importe = c.Double(nullable: false),
                        Descuento = c.Double(nullable: false),
                        ImporteDescuento = c.Double(nullable: false),
                        Iibbbsas = c.Double(nullable: false),
                        Iibbcaba = c.Double(nullable: false),
                        Iva = c.Double(nullable: false),
                        ImporteIva = c.Double(nullable: false),
                        PrecioUnitario = c.Double(nullable: false),
                        Total = c.Double(nullable: false),
                        TipoDeUnidadID = c.Int(nullable: false),
                        ProductoID = c.Int(nullable: false),
                        CompraID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Compra", t => t.CompraID, cascadeDelete: true)
                .ForeignKey("dbo.Productos", t => t.ProductoID, cascadeDelete: true)
                .ForeignKey("dbo.TiposDeUnidad", t => t.TipoDeUnidadID, cascadeDelete: true)
                .Index(t => t.TipoDeUnidadID)
                .Index(t => t.ProductoID)
                .Index(t => t.CompraID);
            
            CreateTable(
                "dbo.Proveedores",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        Contacto = c.String(nullable: false),
                        Direccion = c.String(nullable: false),
                        Localidad = c.String(nullable: false),
                        TelefonoOficina = c.Int(nullable: false),
                        TelefonoCelular = c.Int(nullable: false),
                        TelefonoOtros = c.Int(nullable: false),
                        Cuit = c.String(nullable: false),
                        Iibb = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Debe = c.Double(),
                        SaldoAfavor = c.Double(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ListaPreciosBlister",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Gramos = c.Int(nullable: false),
                        Precio = c.String(),
                        ProductoID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Productos", t => t.ProductoID)
                .Index(t => t.ProductoID);
            
            CreateTable(
                "dbo.ProductosMix",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProdMixId = c.Int(),
                        ProductoDelMixId = c.Int(),
                        Cantidad = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Productos", t => t.ProdMixId)
                .ForeignKey("dbo.Productos", t => t.ProductoDelMixId)
                .Index(t => t.ProdMixId)
                .Index(t => t.ProductoDelMixId);
            
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
            
            CreateTable(
                "dbo.VentasMinoristas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NumeroVenta = c.Int(nullable: false),
                        Local = c.String(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        ImporteInformeZ = c.Double(nullable: false),
                        ImporteIva = c.Double(nullable: false),
                        PrimerNumeroTicket = c.Int(nullable: false),
                        UltimoNumeroTicket = c.Int(nullable: false),
                        NumFactura = c.Int(nullable: false),
                        RazonSocial = c.String(nullable: false),
                        TipoFactura = c.String(nullable: false),
                        TarjetaVisa = c.Double(nullable: false),
                        TarjetaVisaDeb = c.Double(nullable: false),
                        TarjetaMaster = c.Double(nullable: false),
                        TarjetaMaestro = c.Double(nullable: false),
                        TarjetaCabal = c.Double(nullable: false),
                        TotalTarjetas = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ProductosMix", "ProductoDelMixId", "dbo.Productos");
            DropForeignKey("dbo.ProductosMix", "ProdMixId", "dbo.Productos");
            DropForeignKey("dbo.ListaPreciosBlister", "ProductoID", "dbo.Productos");
            DropForeignKey("dbo.Compra", "ProveedorID", "dbo.Proveedores");
            DropForeignKey("dbo.ProductosXCompra", "TipoDeUnidadID", "dbo.TiposDeUnidad");
            DropForeignKey("dbo.ProductosXCompra", "ProductoID", "dbo.Productos");
            DropForeignKey("dbo.ProductosXCompra", "CompraID", "dbo.Compra");
            DropForeignKey("dbo.Compra", "ClasificacionID", "dbo.Clasificacion");
            DropForeignKey("dbo.VentasMayoristas", "VendedorID", "dbo.Vendedor");
            DropForeignKey("dbo.ProductosXVenta", "VentaID", "dbo.VentasMayoristas");
            DropForeignKey("dbo.VentasMayoristas", "ClienteID", "dbo.Clientes");
            DropForeignKey("dbo.Stock", "TipoDeUnidadID", "dbo.TiposDeUnidad");
            DropForeignKey("dbo.Stock", "ProductoID", "dbo.Productos");
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
            DropIndex("dbo.ProductosMix", new[] { "ProductoDelMixId" });
            DropIndex("dbo.ProductosMix", new[] { "ProdMixId" });
            DropIndex("dbo.ListaPreciosBlister", new[] { "ProductoID" });
            DropIndex("dbo.ProductosXCompra", new[] { "CompraID" });
            DropIndex("dbo.ProductosXCompra", new[] { "ProductoID" });
            DropIndex("dbo.ProductosXCompra", new[] { "TipoDeUnidadID" });
            DropIndex("dbo.Compra", new[] { "ClasificacionID" });
            DropIndex("dbo.Compra", new[] { "ProveedorID" });
            DropIndex("dbo.VentasMayoristas", new[] { "VendedorID" });
            DropIndex("dbo.VentasMayoristas", new[] { "ClienteID" });
            DropIndex("dbo.Stock", new[] { "TipoDeUnidadID" });
            DropIndex("dbo.Stock", new[] { "ProductoID" });
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
            DropTable("dbo.VentasMinoristas");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ProductosMix");
            DropTable("dbo.ListaPreciosBlister");
            DropTable("dbo.Proveedores");
            DropTable("dbo.ProductosXCompra");
            DropTable("dbo.Compra");
            DropTable("dbo.Clasificacion");
            DropTable("dbo.Vendedor");
            DropTable("dbo.VentasMayoristas");
            DropTable("dbo.Stock");
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
