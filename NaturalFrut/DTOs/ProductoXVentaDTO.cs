using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NaturalFrut.DTOs
{
    public class ProductoXVentaDTO
    {

        [Required]
        public int Cantidad { get; set; }

        public double? Descuento { get; set; }

        [Required]
        public double Importe { get; set; }

        [Required]
        public double Total { get; set; }

        [Required]
        public int TipoDeUnidadID { get; set; }

        [Required]
        public int ProductoID { get; set; }      


    }
}