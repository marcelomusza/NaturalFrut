using NaturalFrut.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NaturalFrut.Models
{
    public class VentaUpdate
    {

        public VentaMayoristaDTO VentaMayorista { get; set; }
        public List<ProductoXVentaDTO> ProductosXVenta { get; set; }

    }
}