using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NaturalFrut.Models
{
    public class ProductosMixUpdate
    {

        public List<ProductoMix> ProductosAnteriores { get; set; }
        public List<ProductoMix> ProductosNuevos { get; set; }

    }
}