using NaturalFrut.App_BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NaturalFrut.Models
{
    [Table("ProductosXCompra")]
    public class ProductoXCompra : IEntity
    {

        public int ID { get; set; }

        public double Cantidad { get; set; }

        public double Importe { get; set; }

        public double Descuento { get; set; }

        public double ImporteDescuento { get; set; }

        public double Iibbbsas { get; set; }

        public double Iibbcaba { get; set; }

        public double Iva { get; set; }

        public double ImporteIva { get; set; }

        public double PrecioUnitario { get; set; }

        public double Total { get; set; }

        [Required]
        public int TipoDeUnidadID { get; set; }




        [Required]
        public int ProductoID { get; set; }

        [Required]
        public int CompraID { get; set; }



        public TipoDeUnidad TipoDeUnidad { get; set; }
        public Producto Producto { get; set; }

        public Compra Compra { get; set; }

    }
}