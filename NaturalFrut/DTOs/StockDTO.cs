using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NaturalFrut.DTOs
{
    public class StockDTO
    {

        public int ID { get; set; }

        [Required]
        [Display(Name = "Producto")]
        public int ProductoID { get; set; }

        [Required]
        [Display(Name = "Tipo de Unidad")]
        public int TipoDeUnidadID { get; set; }

        [Required]
        [Display(Name = "Cantidad")]
        public double Cantidad { get; set; }


        public Producto Producto { get; set; }

        public TipoDeUnidad TipoDeUnidad { get; set; }

    }
}