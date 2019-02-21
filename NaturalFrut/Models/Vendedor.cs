using NaturalFrut.App_BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NaturalFrut.Models
{
    [Table("Vendedor")]
    public class Vendedor : IEntity
    {
        public int ID { get; set; }

        [Required]
        [Remote("IsVendedor_Available", "Validation")]
        public string Nombre { get; set; }

        [Required]
        public string Apellido { get; set; }

        
        public string Email { get; set; }

        
        public string Telefono { get; set; }

    }
}