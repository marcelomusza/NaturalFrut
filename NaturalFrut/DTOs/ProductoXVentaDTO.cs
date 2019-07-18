using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NaturalFrut.DTOs
{
    public class ProductoXVentaDTO
    {

        public int ID { get; set; }

        [Required]
        public double Cantidad { get; set; }

        public double? Descuento { get; set; }

        [Required]
        public double Importe { get; set; }

        [Required]
        public double Total { get; set; }

        [Required]
        public int TipoDeUnidadID { get; set; }

        [Required]
        public int ProductoID { get; set; }

        [Required]
        public int VentaID { get; set; }


        public TipoDeUnidad TipoDeUnidad { get; set; }

        public Producto Producto { get; set; }

        public VentaMayorista Venta { get; set; }


    }
}