using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NaturalFrut.DTOs
{
    public class CompraUpdateDTO
    {
        public CompraDTO Compra { get; set; }
        public List<ProductoXCompraDTO> ProductosXCompra { get; set; }

    }
}