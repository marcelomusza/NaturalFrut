using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NaturalFrut.DTOs
{
    public class ProductoMixCreateDTO
    {
        public int ID { get; set; }

        public List<ProductoMixDTO> ProductoMix { get; set; }
    }
}