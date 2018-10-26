using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NaturalFrut.DTOs
{
    public class ProductoDTO
    {

        public int? ID { get; set; }

        [Required]
        public string Nombre { get; set; }

        public CategoriaDTO Categoria { get; set; }

    
        public int? CategoriaId { get; set; }


        public Marca Marca { get; set; }

      
        public int? MarcaId { get; set; }

        public bool EsBlister { get; set; }

        public bool EsMix { get; set; }

        public IList<ListaPrecio> ListaPrecios { get; set; }

        public IList<ProductoXVenta> ProductosXVenta { get; set; }

        public IList<Stock> Stock { get; set; }
    }
}