using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using NaturalFrut.App_BLL.Interfaces;

namespace NaturalFrut.Models
{
    public class Stock : IEntity
    {

        public int ID { get; set; }

        [Required]
        [Display(Name = "Producto")]
        public int ProductoID { get; set; }

        [Required]
        [Display(Name = "Cantidad x Kg")]
        public int CantidadXKg { get; set; }

        [Required]
        [Display(Name = "Cantidad x Bulto")]
        public int CantidadXBulto { get; set; }

        [Required]
        [Display(Name = "Cantidad x Paquete")]
        public int CantidadXPaquete { get; set; }



        public Producto Producto { get; set; }

    }
}