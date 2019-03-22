using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NaturalFrut.App_BLL.Interfaces;

namespace NaturalFrut.Models
{
    public class Categoria : IEntity
    {

        public int ID { get; set; }

        [Required]
        [Remote("IsCategoria_Available", "Validation", AdditionalFields = "ID")]
        public string Nombre { get; set; }

        public IList<Producto> Productos { get; set; }

    }
}