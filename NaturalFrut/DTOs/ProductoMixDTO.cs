using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NaturalFrut.DTOs
{
    public class ProductoMixDTO
    {

        public int ID { get; set; }

        public int ProdMixId { get; set; }

        public int ProductoDelMixId { get; set; }

        public double Cantidad { get; set; }

        public ProductoDTO ProdMix { get; set; }

        public ProductoDTO ProductoDelMix { get; set; }

    }
}