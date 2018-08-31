using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NaturalFrut.Models
{
    public class ProductoMixCreate
    {
        public int ID { get; set; }

        public List<ProductoMix> ProductosMix { get; set; }
    }
}