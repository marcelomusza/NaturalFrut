using NaturalFrut.App_BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NaturalFrut.Models
{
    [Table("TiposDeUnidad")]
    public class TipoDeUnidad : IEntity
    {
        [ForeignKey("ProductoXVenta")]
        public int ID { get; set; }

        [Required]
        public string Nombre { get; set; }



        public ProductoXVenta ProductoXVenta { get; set; }

        public IList<ListaDePrecios> ListasDePrecios { get; set; }


    }
}