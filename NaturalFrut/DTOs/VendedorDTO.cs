using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NaturalFrut.DTOs
{
    public class VendedorDTO
    {

        public int? ID { get; set; }
        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellido { get; set; }


        public string Email { get; set; }


        public string Telefono { get; set; }

    }
}