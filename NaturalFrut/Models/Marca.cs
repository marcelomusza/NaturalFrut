using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NaturalFrut.App_BLL.Interfaces;

namespace NaturalFrut.Models
{
    public class Marca : IEntity
    {

        public int ID { get; set; }

        [Required]
        [Remote("IsMarca_Available", "Validation")]
        public string Nombre { get; set; }

        public IList<Producto> Productos { get; set; }

    }
}