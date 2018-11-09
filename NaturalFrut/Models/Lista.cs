using NaturalFrut.App_BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NaturalFrut.Models
{
    [Table("Listas")]
    public class Lista : IEntity
    {

        public int ID { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Porcentaje de Aumento")]
        public int PorcentajeAumento { get; set; }


        [System.Runtime.Serialization.IgnoreDataMember]
        public IList<ListaPrecio> ListaPrecios { get; set; }
        
        public IList<Cliente> Cliente { get; set; }

       
    }
}