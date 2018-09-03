using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NaturalFrut.DTOs
{
    public class ProductosMixUpdateDTO
    {

        public List<ProductoMixDTO> ProductosAnteriores { get; set; }
        public List<ProductoMixDTO> ProductosNuevos { get; set; }

    }
}