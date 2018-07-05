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

        [Required]
        public string RazonSocial { get; set; }

        [Required]
        public string Cuit { get; set; }

        [Required]
        public string Iibb { get; set; }

        [Required]
        public string Direccion { get; set; }

        [Required]
        public string Provincia { get; set; }

        [Required]
        public string Localidad { get; set; }

        public int TelefonoNegocio { get; set; }

        public int TelefonoCelular { get; set; }

        [Required]
        public string Email { get; set; }

        public CondicionIVADTO CondicionIVA { get; set; }

        [Required]
        public int CondicionIVAId { get; set; }

        public TipoClienteDTO TipoCliente { get; set; }

        [Required]
        public int TipoClienteId { get; set; }

        public ListaDTO Lista { get; set; }

        [Required]
        public int ListaId { get; set; }

    }
}