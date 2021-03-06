﻿using NaturalFrut.App_BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NaturalFrut.Models
{
    [Table("Productos")]
    public class Producto : IEntity
    {
        public int ID { get; set; }

        [Required]
        public string Nombre { get; set; }

        
        public int? CategoriaId { get; set; }

        
        public int? MarcaId { get; set; }

        public bool EsBlister { get; set; }

        public bool EsMix { get; set; }

        public string NombreAuxiliar { get; set; }

        public Categoria Categoria { get; set; }

        public Marca Marca { get; set; }


        public IList<ListaPrecio> ListaPrecios { get; set; }

        public IList<ProductoXVenta> ProductosXVenta { get; set; }

        //public IList<ProductoMix> ProdMix { get; set; }
        //public IList<ProductoMix> ProductoDelMix { get; set; }

        public IList<Stock> Stock { get; set; }

    }
}