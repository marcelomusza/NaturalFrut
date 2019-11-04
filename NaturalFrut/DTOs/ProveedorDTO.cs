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

       
        public string Contacto { get; set; }

       
        public string Direccion { get; set; }

        
        public string Localidad { get; set; }

        public int? TelefonoOficina { get; set; }

        public int? TelefonoCelular { get; set; }

        public int? TelefonoOtros { get; set; }

        
        public string Cuit { get; set; }

       
        public string Iibb { get; set; }      

       
        public string Email { get; set; }
        public double? Debe { get; set; }

        public double? SaldoAfavor { get; set; }


    }
}