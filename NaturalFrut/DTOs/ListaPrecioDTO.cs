using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NaturalFrut.DTOs
{
    public class ListaPrecioDTO
    {

        public int? ID { get; set; }

        
        public int? ListaID { get; set; }

        [Required]
        public int ProductoID { get; set; }

        [Required]
        public double PrecioXKG { get; set; }

        [Required]
        public double PrecioXBultoCerrado { get; set; }

        [Required]
        public double KGBultoCerrado { get; set; }

        [Required]
        public double PrecioXUnidad { get; set; }


        public Lista Lista { get; set; }

        public Producto Producto { get; set; }

    }
}