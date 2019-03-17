using NaturalFrut.App_BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NaturalFrut.Models
{
    [Table("VentasMinoristas")]
    public class VentaMinorista : IEntity
    {

        public int ID { get; set; }
        
        public int NumeroVenta { get; set; }

        [Required]
        public string Local { get; set; }

        public DateTime Fecha { get; set; }

        //[Required]
        //public decimal ImporteVentaTotal { get; set; }

        [Required]
        public double ImporteInformeZ { get; set; }

        //[Required]
        //public double Iva { get; set; }

        //[Required]
        //public int CantidadPersonas { get; set; }

        //[Required]
        //public double Promedio { get; set; }

        public double ImporteIva { get; set; }

        public int PrimerNumeroTicket { get; set; }

        public int UltimoNumeroTicket { get; set; }

        public string NumFactura { get; set; }

        public string RazonSocial { get; set; }

        public string TipoFactura { get; set; }

        public double TarjetaVisa { get; set; }

        public double TarjetaVisaDeb { get; set; }

        public double TarjetaMaster { get; set; }

        public double TarjetaMaestro { get; set; }

        public double TarjetaCabal { get; set; }

        public double TotalTarjetas { get; set; }

    }
}