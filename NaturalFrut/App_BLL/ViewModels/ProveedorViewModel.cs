using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NaturalFrut.App_BLL.ViewModels
{
    public class ProveedorViewModel
    {

        public int? ID { get; set; }

        [Required]
        [Remote("IsProveedor_Available", "Validation", AdditionalFields = "ID")]
        [Display(Name = "Proveedor")]
        public string Nombre { get; set; }

        [Display(Name = "Contacto")]
        public string Contacto { get; set; }

        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        public string Localidad { get; set; }

        [Display(Name = "Teléfono Oficina")]
        public int? TelefonoOficina { get; set; }

        [Display(Name = "Teléfono Celular")]
        public int? TelefonoCelular { get; set; }

        [Display(Name = "Teléfono Otros")]
        public int? TelefonoOtros { get; set; }

        [Display(Name = "CUIT")]
        public string Cuit { get; set; }

        [Display(Name = "IIBB")]
        public string Iibb { get; set; }

        [Display(Name = "Dirección Email")]
        public string Email { get; set; }      

       
        public ProveedorViewModel()
        {
            ID = 0;
        }

        public ProveedorViewModel(Proveedor proveedor)
        {
            ID = proveedor.ID;
            Nombre = proveedor.Nombre;
            Contacto = proveedor.Contacto;
            Cuit = proveedor.Cuit;
            Iibb = proveedor.Iibb;
            Direccion = proveedor.Direccion;
            Localidad = proveedor.Localidad;
            TelefonoOficina = proveedor.TelefonoOficina;
            TelefonoCelular = proveedor.TelefonoCelular;
            TelefonoOtros = proveedor.TelefonoOtros;
            Email = proveedor.Email;          

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