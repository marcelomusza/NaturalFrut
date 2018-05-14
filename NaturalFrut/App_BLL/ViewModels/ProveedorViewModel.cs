using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NaturalFrut.App_BLL.ViewModels
{
    public class ProveedorViewModel
    {

        public int? ID { get; set; }

        [Required]
        [Display(Name = "Nombre y Apellido")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Razón Social")]
        public string RazonSocial { get; set; }

        public double Saldo { get; set; }

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

       
        public ProveedorViewModel()
        {
            ID = 0;
        }

        public ProveedorViewModel(Proveedor proveedor)
        {
            ID = proveedor.ID;
            Nombre = proveedor.Nombre;
            RazonSocial = proveedor.RazonSocial;
            Cuit = proveedor.Cuit;
            Iibb = proveedor.Iibb;
            Direccion = proveedor.Direccion;
            Provincia = proveedor.Provincia;
            Localidad = proveedor.Localidad;
            TelefonoNegocio = proveedor.TelefonoNegocio;
            TelefonoCelular = proveedor.TelefonoCelular;
            Email = proveedor.Email;
            Saldo = proveedor.Saldo;

            CondicionIVAId = proveedor.CondicionIVAId;           

        }

        public string Titulo
        {

            get
            {
                return (ID != 0) ? "Editar Proveedor" : "Nuevo Proveedor";
            }

        }

    }
}