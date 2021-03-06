﻿using NaturalFrut.App_BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NaturalFrut.Models
{
    [Table("Compra")]
    public class Compra : IEntity
    {

        public int ID { get; set; }

        public int NumeroCompra { get; set; }

        public string Factura { get; set; }

        public DateTime Fecha { get; set; }

        public double Iva { get; set; }

        public double SumaTotal { get; set; }

        public double EntregaEfectivo { get; set; }

        public double ImporteIva { get; set; }

        public double DescuentoPorc { get; set; }

        public double ImporteIibbbsas { get; set; }

        public double Iibbbsas { get; set; }

        public double Descuento { get; set; }

        public double ImporteIibbcaba { get; set; }

        public double Iibbcaba { get; set; }

        public double Subtotal { get; set; }

        public double ImportePercIva { get; set; }

        public double PercIva { get; set; }

        public double ImporteNoGravado { get; set; }

        public double Total { get; set; }

        public double TotalGastos { get; set; }

        public string TipoFactura { get; set; }

        public string Local { get; set; }

        public bool NoConcretado { get; set; }

        [Required]
        public int ProveedorID { get; set; }

        [Required]
        public int ClasificacionID { get; set; }

        public double? Debe { get; set; }

        public double? SaldoAfavor { get; set; }



        public Clasificacion Clasificacion { get; set; }        

        public Proveedor Proveedor { get; set; }

        public IList<ProductoXCompra> ProductosXCompra { get; set; }
    }
}