using NaturalFrut.Models;
using Newtonsoft.Json;
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
        public string PrecioXKG { get; set; }

        [Required]
        public string PrecioXBultoCerrado { get; set; }

        [Required]
        public string KGBultoCerrado { get; set; }

        [Required]
        public string PrecioXUnidad { get; set; }

       
        public Lista Lista { get; set; }

        
        public Producto Producto { get; set; }

    }
}