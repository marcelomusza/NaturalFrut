﻿using NaturalFrut.App_BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NaturalFrut.Models
{
    [Table("Proveedores")]
    public class Proveedor : IEntity
    {

        public int ID { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string RazonSocial { get; set; }

        public double Saldo { get; set; }

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

        public CondicionIVA CondicionIVA { get; set; }

        [Required]
        public int CondicionIVAId { get; set; }

       
    }
}