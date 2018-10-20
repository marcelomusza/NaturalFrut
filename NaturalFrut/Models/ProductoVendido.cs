using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NaturalFrut.Models
{
    public class ProductoVendido
    {

        public DateTime Fecha { get; set; }

        public string Producto { get; set; }

        public int NumeroVenta { get; set; }

        public string Cliente { get; set; }

        public int Cantidad { get; set; }

        public decimal Precio { get; set; }

    }
}