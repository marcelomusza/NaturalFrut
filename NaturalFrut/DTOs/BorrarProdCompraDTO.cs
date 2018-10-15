using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NaturalFrut.Models
{
    public class BorrarProdCompraDTO
    {
        public int CompraID { get; set; }
        public double TotalProducto { get; set; }
        public int ProductoID { get; set; }
    }
}