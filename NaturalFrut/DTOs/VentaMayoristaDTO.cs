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
        
        public string Fecha { get; set; }

        public bool Impreso { get; set; }

        public bool NoConcretado { get; set; }

        public bool Facturado { get; set; }

        public double EntregaEfectivo { get; set; }

        public double? EntregaEfectivoHistorico { get; set; }

        public int? TipoDescuentoTotal { get; set; }

        public double? Descuento { get; set; }

        public double? Debe { get; set; }

        public double? SaldoAFavor { get; set; }

        public int NumeroVenta { get; set; }

        public double SumaTotal { get; set; }

        public double? IVA { get; set; }

        [Required]
        public int ClienteID { get; set; }

        public int? VendedorID { get; set; }


        public Cliente Cliente { get; set; }

        public Vendedor Vendedor { get; set; }
        
        public IList<ProductoXVenta> ProductosXVenta { get; set; }
    }
}