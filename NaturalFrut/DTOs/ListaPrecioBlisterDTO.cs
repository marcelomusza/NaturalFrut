using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NaturalFrut.DTOs
{
    public class ListaPrecioBlisterDTO
    {

      

        public int? ID { get; set; }

        public int Gramos { get; set; }

        public string Precio { get; set; }


        public int? ProductoID { get; set; }

        public ProductoDTO Producto { get; set; }



    }
}