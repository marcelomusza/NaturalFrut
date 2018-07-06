namespace NaturalFrut.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tablaVentaMinoristas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VentasMinoristas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Local = c.String(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        ImporteVentaTotal = c.Double(nullable: false),
                        ImporteInformeZ = c.Double(nullable: false),
                        Iva = c.Double(nullable: false),
                        CantidadPersonas = c.Int(nullable: false),
                        Promedio = c.Double(nullable: false),
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
            DropTable("dbo.VentasMinoristas");
        }
    }
}
