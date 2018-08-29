using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NaturalFrut.DTOs
{
    public class VentaUpdateDTO
    {
        public VentaMayoristaDTO VentaMayorista { get; set; }
        public List<ProductoXVentaDTO> ProductosXVenta { get; set; }

    }
}