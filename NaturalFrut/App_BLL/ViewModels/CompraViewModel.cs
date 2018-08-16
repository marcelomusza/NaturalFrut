﻿using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NaturalFrut.App_BLL.ViewModels
{
    public class CompraViewModel
    {

        public int? ID { get; set; }

        public IEnumerable<Proveedor> Proveedor { get; set; }

        public IEnumerable<Clasificacion> Clasificacion { get; set; }

        public int Factura { get; set; }

        public DateTime Fecha { get; set; }

        public double Iva { get; set; }

        public double SumaTotal { get; set; }

        public double ImporteIva { get; set; }

        public double DescuentoPorc { get; set; }

        public double Iibbbsas { get; set; }

        public double Descuento { get; set; }

        public double Iibbcaba { get; set; }

        public double Subtotal { get; set; }

        public double PercIva { get; set; }

        public double ImporteNoGravado { get; set; }

        public double Total { get; set; }



        public CompraViewModel()
        {
            ID = 0;
        }



    }
}