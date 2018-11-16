using NaturalFrut.App_BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NaturalFrut.Models
{

    public class CompraReporte : IEntity
    {

        public int ID { get; set; }

       // public int NumeroCompra { get; set; }

        public string Factura { get; set; }

        public string Fecha { get; set; }

        public double Iva { get; set; }

        public string SumaTotal { get; set; }

        public string ImporteIva { get; set; }

        public double DescuentoPorc { get; set; }

        public string ImporteIibbbsas { get; set; }

    //   public double Iibbbsas { get; set; }

        public string Descuento { get; set; }

        public string ImporteIibbcaba { get; set; }

        //public double Iibbcaba { get; set; }

        public string Subtotal { get; set; }

        public string ImportePercIva { get; set; }

      //  public double PercIva { get; set; }

      //  public string ImporteNoGravado { get; set; }

        public string Total { get; set; }

    //    public string TotalGastos { get; set; }

        public string TipoFactura { get; set; }

        //public string Local { get; set; }

        public string Nombre { get; set; }

        public string Clasificacion { get; set; }

        public string Cuit { get; set; }

        public string Iibb { get; set; }

    }
}