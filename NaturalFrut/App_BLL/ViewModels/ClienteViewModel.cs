using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NaturalFrut.Models;

namespace NaturalFrut.App_BLL.ViewModels
{
    public class ClienteViewModel
    {

        public int? ID { get; set; }

        [Required]
        [Remote("IsCliente_Available", "Validation")]
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

        [Required]
        public string Transporte { get; set; }

        [Required]
        [Display(Name = "Dirección Transporte")]
        public string DireccionTransporte { get; set; }

        [Display(Name = "Teléfono Transporte")]
        public int TelefonoTransporte { get; set; }

        [Required]
        public string Dias { get; set; }

        [Required]
        public string Horarios { get; set; }

        public IEnumerable<CondicionIVA> CondicionIVA { get; set; }

        [Required]
        [Display(Name = "Condición ante IVA")]
        public int CondicionIVAId { get; set; }

        public IEnumerable<TipoCliente> TipoCliente { get; set; }

        [Required]
        [Display(Name = "Tipo de Cliente")]
        public int TipoClienteId { get; set; }

        public IEnumerable<Lista> Lista { get; set; }

        [Required]
        [Display(Name = "Lista de Precios Asociada")]
        public int ListaId { get; set; }

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
            Transporte = cliente.Transporte;
            DireccionTransporte = cliente.DireccionTransporte;
            TelefonoTransporte = cliente.TelefonoTransporte;
            Dias = cliente.Dias;
            Horarios = cliente.Horarios;

            CondicionIVAId = cliente.CondicionIVAId;
            TipoClienteId = cliente.TipoClienteId;
            ListaId = cliente.ListaId;
            
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