using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NaturalFrut.DTOs
{
    public class ClienteDTO
    {

        public int? ID { get; set; }

        [Required]
        public string Nombre { get; set; }

       
        public string RazonSocial { get; set; }

       
        public string Cuit { get; set; }

      
        public string Iibb { get; set; }

        [Required]
        public string Direccion { get; set; }

       
        public string Provincia { get; set; }

      
        public string Localidad { get; set; }

        public int? TelefonoNegocio { get; set; }

        public int? TelefonoCelular { get; set; }

        public double? Debe { get; set; }

        public double? SaldoAfavor { get; set; }

        
        public string Email { get; set; }

        public CondicionIVADTO CondicionIVA { get; set; }

        
        public int? CondicionIVAId { get; set; }

        public TipoClienteDTO TipoCliente { get; set; }

       
        public int? TipoClienteId { get; set; }

        public ListaDTO Lista { get; set; }

       
        public int? ListaId { get; set; }

        
        public string Transporte { get; set; }

        
        public string DireccionTransporte { get; set; }

        public int? TelefonoTransporte { get; set; }

        public string Dias { get; set; }

        
        public string Horarios { get; set; }

    }
}