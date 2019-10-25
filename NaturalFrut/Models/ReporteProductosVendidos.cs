using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NaturalFrut.Models
{
    public class ReporteProductosVendidos
    {

        public DateTime FechaVenta { get; set; }

        public string NombreProducto { get; set; }

        public int NumeroVenta { get; set; }

        public string NombreCliente { get; set; }

        public double Cantidad { get; set; }

        public double Importe { get; set; }

    }
}