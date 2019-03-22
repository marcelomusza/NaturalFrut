using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using NaturalFrut.App_BLL.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace NaturalFrut.Models
{
    [Table("Clasificacion")]
    public class Clasificacion : IEntity
    {

        public int ID { get; set; }

        [Required]
        [Remote("IsClasificacion_Available", "Validation", AdditionalFields = "ID")]
        public string Nombre { get; set; }

       
    }
}