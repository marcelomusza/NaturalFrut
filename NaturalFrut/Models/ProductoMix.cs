using NaturalFrut.App_BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NaturalFrut.Models
{
    [Table("ProductosMix")]
    public class ProductoMix :IEntity
    {

        public int ID { get; set; }

        public int? ProdMixId { get; set; }

        public int? ProductoDelMixId { get; set; }

        public double Cantidad { get; set; }

        //[ForeignKey("Producto")]
        public Producto ProdMix { get; set; }

        //[ForeignKey("Producto")]
        public Producto ProductoDelMix { get; set; }  



    }
}