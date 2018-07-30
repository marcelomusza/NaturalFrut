using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NaturalFrut.DTOs
{
    public class VentaMayoristaDTO
    {
        public int ID { get; set; }

        [Required]
        public string Fecha { get; set; }

        public bool Impreso { get; set; }

        public bool NoConcretado { get; set; }

        public double EntregaEfectivo { get; set; }

        public double? Descuento { get; set; }

        public int NumeroVenta { get; set; }

        public double SumaTotal { get; set; }

        [Required]
        public int ClienteID { get; set; }

        [Required]
        public int VendedorID { get; set; }


        public Cliente Cliente { get; set; }

        public Vendedor Vendedor { get; set; }

        [Required]
        public IList<ProductoXVentaDTO> ProductosXVenta { get; set; }
    }
}