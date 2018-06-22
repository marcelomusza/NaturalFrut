using NaturalFrut.App_BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NaturalFrut.Models
{ 
    [Table("ProductosXLista")]
    public class ProductoXLista : IEntity
    {
        public int ID { get; set; }

        [Required]
        public int ListaDePreciosID { get; set; }

        [Required]
        public int ProductoID { get; set; }

        [Required]
        public int TipoDeUnidadID { get; set; }

        [Required]
        public int ClienteID { get; set; }

        [Required]
        public float Precio { get; set; }


        public ListaDePrecios ListaDePrecios { get; set; }

        public Producto Producto { get; set; }

        public TipoDeUnidad TipoDeUnidad { get; set; }

        public Cliente Cliente { get; set; }

    }
}