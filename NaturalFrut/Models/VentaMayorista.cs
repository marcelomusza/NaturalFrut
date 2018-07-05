﻿using NaturalFrut.App_BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NaturalFrut.Models
{
    [Table("VentasMayoristas")]
    public class VentaMayorista : IEntity
    {

        public int ID { get; set; }

        [Required]
        public DateTime Fecha { get; set; }        

        public bool Impreso { get; set; }

        public bool NoConcretado { get; set; }

        public bool EntregaEfectivo { get; set; }

        public int Descuento { get; set; }

        [Required]
        public int ClienteID { get; set; }

        [Required]
        public int VendedorID { get; set; }



        public Cliente Cliente { get; set; }        

        public Vendedor Vendedor { get; set; }

        public IList<ProductoXVenta> ProductosXVenta { get; set; }
    }
}