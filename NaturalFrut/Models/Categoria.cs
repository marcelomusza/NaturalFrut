using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using NaturalFrut.App_BLL.Interfaces;

namespace NaturalFrut.Models
{
    public class Categoria : IEntity
    {

        public int ID { get; set; }

        [Required]
        public string Nombre { get; set; }

        public IList<Producto> Productos { get; set; }

    }
}