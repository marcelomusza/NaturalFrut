using NaturalFrut.App_BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NaturalFrut.Models
{
    [Table("ProductosXVenta")]
    public class ProductoXVenta : IEntity
    {

        public int ID { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        public float Importe { get; set; }

        [Required]
        public float Total { get; set; }

        [Required]
        public int TipoDeUnidadID { get; set; }

        [Required]
        public int ProductoID { get; set; }

        [Required]
        public int VentaID { get; set; }


        public TipoDeUnidad TipoDeUnidad { get; set; }

        public Producto Producto { get; set; }

        public Venta Venta { get; set; }

    }
}