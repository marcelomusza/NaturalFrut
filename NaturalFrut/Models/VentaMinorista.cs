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

        [Required]
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

        [Required]
        public double ImporteIva { get; set; }

        [Required]
        public int PrimerNumeroTicket { get; set; }

        [Required]
        public int UltimoNumeroTicket { get; set; }

        [Required]
        public string NumFactura { get; set; }

        [Required]
        public string RazonSocial { get; set; }

        [Required]
        public string TipoFactura { get; set; }

        [Required]
        public double TarjetaVisa { get; set; }

        [Required]
        public double TarjetaVisaDeb { get; set; }

        [Required]
        public double TarjetaMaster { get; set; }

        [Required]
        public double TarjetaMaestro { get; set; }

        [Required]
        public double TarjetaCabal { get; set; }

        [Required]
        public double TotalTarjetas { get; set; }

    }
}