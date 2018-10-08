using NaturalFrut.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NaturalFrut.Models
{
    public class CompraUpdate
    {

        public CompraDTO Compra { get; set; }
        public List<ProductoXCompraDTO> ProductosXCompra { get; set; }

    }
}