using NaturalFrut.App_BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NaturalFrut.Models
{
    [Table("ListaPreciosBlister")]
    public class ListaPrecioBlister : IEntity
    {

        public int ID { get; set; }

        public int Gramos { get; set; }

        public string Precio { get; set; }


        public int? ProductoID { get; set; }

        public Producto Producto { get; set; }
    }
}