using NaturalFrut.App_BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NaturalFrut.Models
{
    public class Cliente : IEntity
    {
        public int ID { get; set; }

        [Required]
        public string Nombre { get; set; }

                

    }
}