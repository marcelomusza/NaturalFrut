using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NaturalFrut.DTOs
{
    public class ProveedorDTO
    {

        public int? ID { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Contacto { get; set; }

        [Required]
        public string Direccion { get; set; }

        [Required]
        public string Localidad { get; set; }

        public int TelefonoOficina { get; set; }

        public int TelefonoCelular { get; set; }

        public int TelefonoOtros { get; set; }

        [Required]
        public string Cuit { get; set; }

        [Required]
        public string Iibb { get; set; }      

        [Required]
        public string Email { get; set; }

    }
}