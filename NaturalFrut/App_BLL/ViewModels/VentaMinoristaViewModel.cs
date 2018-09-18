using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NaturalFrut.App_BLL.ViewModels
{
    public class VentaMinoristaViewModel
    {

        public int? ID { get; set; }

        [Required]
        [Display(Name = "Número Venta")]
        public int NumeroVenta { get; set; }

        [Required]
        public string Local { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        [Display(Name = "Importe Venta Total")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal ImporteVentaTotal { get; set; }

        [Required]
        [Display(Name = "Importe Informe Z")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public double ImporteInformeZ { get; set; }

        [Required]
        [Display(Name = "IVA")]
        public double Iva { get; set; }

        [Required]
        [Display(Name = "Cantidad Personas")]
        public int CantidadPersonas { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public double Promedio { get; set; }

        [Required]
        [Display(Name = "Importe IVA")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public double ImporteIva { get; set; }

        [Required]
        [Display(Name = "Primer Número Ticket")]
        public int PrimerNumeroTicket { get; set; }

        [Required]
        [Display(Name = "Último Número Ticket")]
        public int UltimoNumeroTicket { get; set; }

        [Required]
        [Display(Name = "Número Factura")]
        public int NumFactura { get; set; }

        [Required]
        [Display(Name = "Razón Social")]
        public string RazonSocial { get; set; }

        [Required]
        [Display(Name = "Tipo Factura")]
        public string TipoFactura { get; set; }

        [Required]
        [Display(Name = "Tarjeta VISA")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public double TarjetaVisa { get; set; }

        [Required]
        [Display(Name = "Tarjeta VISA Debito")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public double TarjetaVisaDeb { get; set; }

        [Required]
        [Display(Name = "Tarjeta Master")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public double TarjetaMaster { get; set; }

        [Required]
        [Display(Name = "Tarjeta Maestro")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public double TarjetaMaestro { get; set; }

        [Required]
        [Display(Name = "Tarjeta Cabal")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public double TarjetaCabal { get; set; }

        [Required]
        [Display(Name = "Total Tarjetas")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public double TotalTarjetas { get; set; }



        public VentaMinoristaViewModel()
        {
            ID = 0;
        }

        public VentaMinoristaViewModel(VentaMinorista ventaMinorista)
        {
            ID = ventaMinorista.ID;
            NumeroVenta = ventaMinorista.NumeroVenta;
            Local = ventaMinorista.Local;
            Fecha = ventaMinorista.Fecha;
            ImporteVentaTotal = ventaMinorista.ImporteVentaTotal;
            ImporteInformeZ = ventaMinorista.ImporteInformeZ;
            Iva = ventaMinorista.Iva;
            CantidadPersonas = ventaMinorista.CantidadPersonas;
            Promedio = ventaMinorista.Promedio;
            ImporteIva = ventaMinorista.ImporteIva;
            PrimerNumeroTicket = ventaMinorista.PrimerNumeroTicket;
            UltimoNumeroTicket = ventaMinorista.UltimoNumeroTicket;
            NumFactura = ventaMinorista.NumFactura;
            RazonSocial = ventaMinorista.RazonSocial;
            TipoFactura = ventaMinorista.TipoFactura;
            TarjetaVisa = ventaMinorista.TarjetaVisa;
            TarjetaVisaDeb = ventaMinorista.TarjetaVisaDeb;
            TarjetaMaster = ventaMinorista.TarjetaMaster;
            TarjetaMaestro = ventaMinorista.TarjetaMaestro;
            TarjetaCabal = ventaMinorista.TarjetaCabal;
            TotalTarjetas = ventaMinorista.TotalTarjetas;

         }

        public string Titulo
        {

            get
            {
                return (ID != 0) ? "Editar Venta Minorista" : "Nueva Venta Minorista";
            }

        }

}
}