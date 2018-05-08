using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using NaturalFrut.Models;

namespace NaturalFrut.App_BLL.ViewModels
{
    public class ClienteViewModel
    {

        public int? ID { get; set; }

        [Required]
        [Display(Name = "Nombre y Apellido")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Razón Social")]
        public string RazonSocial { get; set; }

        [Required]
        [Display(Name = "CUIT")]
        public string Cuit { get; set; }

        [Required]
        [Display(Name = "IIBB")]
        public string Iibb { get; set; }

        [Required]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [Required]
        public string Provincia { get; set; }

        [Required]
        public string Localidad { get; set; }

        [Display(Name = "Teléfono Negocio")]
        public int TelefonoNegocio { get; set; }

        [Display(Name = "Teléfono Celular")]
        public int TelefonoCelular { get; set; }

        [Required]
        [Display(Name = "Dirección Email")]
        public string Email { get; set; }

        public IEnumerable<CondicionIVA> CondicionIVA { get; set; }

        [Required]
        [Display(Name = "Condición ante IVA")]
        public int CondicionIVAId { get; set; }

        public IEnumerable<TipoCliente> TipoCliente { get; set; }

        [Required]
        [Display(Name = "Tipo de Cliente")]
        public int TipoClienteId { get; set; }

        public ClienteViewModel()
        {
            ID = 0;
        }

        public ClienteViewModel(Cliente cliente)
        {
            ID = cliente.ID;
            Nombre = cliente.Nombre;
            RazonSocial = cliente.RazonSocial;
            Cuit = cliente.Cuit;
            Iibb = cliente.Iibb;
            Direccion = cliente.Direccion;
            Provincia = cliente.Provincia;
            Localidad = cliente.Localidad;
            TelefonoNegocio = cliente.TelefonoNegocio;
            TelefonoCelular = cliente.TelefonoCelular;
            Email = cliente.Email;

            CondicionIVAId = cliente.CondicionIVAId;
            TipoClienteId = cliente.TipoClienteId;
            
        }

        public string Titulo
        {

            get
            {
                return (ID != 0) ? "Editar Cliente" : "Nuevo Cliente";
            }

        }

    }
}