using NaturalFrut.App_BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NaturalFrut.Models
{
    public class ListaDePrecios : IEntity
    {

        public int ID { get; set; }

        [Required]
        public string Nombre { get; set; }        

        [Required]
        public int ProductoID { get; set; }

        [Required]
        public int TipoDeUnidadID { get; set; }

        [Required]
        public int ClienteID { get; set; }

        [Required]
        public float Precio { get; set; }



        public Producto Producto { get; set; }

        public TipoDeUnidad TipoDeUnidad { get; set; }

        public Cliente Cliente { get; set; }
    }
}