using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NaturalFrut.DTOs
{
    public class ProductoXListaDTO
    {

        public int? ID { get; set; }

        [Required]
        public float Precio { get; set; }

        public ListaDePrecios ListaDePrecios { get; set; }

        [Required]
        public int ListaDePreciosId { get; set; }

        public Cliente Cliente { get; set; }

        [Required]
        public int ClienteId { get; set; }

        public Producto Producto { get; set; }

        [Required]
        public int ProductoId { get; set; }

        public TipoDeUnidad TipoDeUnidad { get; set; }

        [Required]
        public int TipoDeUnidadId { get; set; }

    }
}