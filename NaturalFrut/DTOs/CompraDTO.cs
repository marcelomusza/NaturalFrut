using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NaturalFrut.DTOs
{
    public class CompraDTO
    {
        public int ID { get; set; }

        public int NumeroCompra { get; set; }

        public string Factura { get; set; }

        public string Fecha { get; set; }

        public double Iva { get; set; }

        public double SumaTotal { get; set; }

        public double ImporteIva { get; set; }

        public double DescuentoPorc { get; set; }

        public double ImporteIibbbsas { get; set; }

        public double Iibbbsas { get; set; }

        public double Descuento { get; set; }

        public double ImporteIibbcaba { get; set; }

        public double Iibbcaba { get; set; }

        public double Subtotal { get; set; }

        public double ImportePercIva { get; set; }

        public double PercIva { get; set; }

        public double ImporteNoGravado { get; set; }

        public double Total { get; set; }

        public double TotalGastos { get; set; }

        public string TipoFactura { get; set; }

        [Required]
        public int ProveedorID { get; set; }

        [Required]
        public int ClasificacionID { get; set; }

        public bool NoConcretado { get; set; }



        public Clasificacion Clasificacion { get; set; }

        public Proveedor Proveedor { get; set; }

        public IList<ProductoXCompraDTO> ProductosXCompra { get; set; }
    }
}