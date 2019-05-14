using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using NaturalFrut.App_BLL.Interfaces;

namespace NaturalFrut.Models
{
    [Table("Stock")]
    public class Stock : IEntity
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Producto")]
        public int ProductoID { get; set; }

        [Required]
        [Display(Name = "Tipo de Unidad")]
        public int TipoDeUnidadID { get; set; }

        [Required]
        [Display(Name = "Cantidad")]
        public double Cantidad { get; set; }

        public string ProductoAuxiliar { get; set; }

        public string TipoDeUnidadAuxiliar { get; set; }

        //[Required]
        //[Display(Name = "Cantidad x Bulto")]
        //public int CantidadXBulto { get; set; }

        //[Required]
        //[Display(Name = "Cantidad x Unidad")]
        //public int CantidadXUnidad { get; set; }

        public Producto Producto { get; set; }

        public TipoDeUnidad TipoDeUnidad { get; set; }

    }
}