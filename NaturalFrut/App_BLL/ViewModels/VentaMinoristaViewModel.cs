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

        [Display(Name = "Número Venta")]
        public int NumeroVenta { get; set; }

        [Required]
        public string Local { get; set; }

        public DateTime Fecha { get; set; }

        //[Required]
        //[Display(Name = "Importe Venta Total")]
        //[DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        //public decimal ImporteVentaTotal { get; set; }

        [Required]
        [Display(Name = "Importe Informe Z")]
        public double ImporteInformeZ { get; set; }

        //[Required]
        //[Display(Name = "IVA")]
        //public double Iva { get; set; }

        //[Required]
        //[Display(Name = "Cantidad Personas")]
        //public int CantidadPersonas { get; set; }

        //[Required]
        //[DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        //public double Promedio { get; set; }

        [Display(Name = "Importe IVA")]
        public double ImporteIva { get; set; }

        [Display(Name = "Primer Número Ticket")]
        public int PrimerNumeroTicket { get; set; }

        [Display(Name = "Último Número Ticket")]
        public int UltimoNumeroTicket { get; set; }

        [Display(Name = "Número Factura")]
        public string NumFactura { get; set; }

        [Display(Name = "Razón Social")]
        public string RazonSocial { get; set; }

        [Display(Name = "Tipo Factura")]
        public string TipoFactura { get; set; }

        [Display(Name = "Tarjeta VISA")]
        public double TarjetaVisa { get; set; }

        [Display(Name = "Tarjeta VISA Debito")]
        public double TarjetaVisaDeb { get; set; }

        [Display(Name = "Tarjeta Master")]
        public double TarjetaMaster { get; set; }

        [Display(Name = "Tarjeta Maestro")]
        public double TarjetaMaestro { get; set; }

        [Display(Name = "Tarjeta Cabal")]
        public double TarjetaCabal { get; set; }

        [Display(Name = "Total Tarjetas")]
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
            //ImporteVentaTotal = ventaMinorista.ImporteVentaTotal;
            ImporteInformeZ = ventaMinorista.ImporteInformeZ;
            //Iva = ventaMinorista.Iva;
            //CantidadPersonas = ventaMinorista.CantidadPersonas;
            //Promedio = ventaMinorista.Promedio;
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