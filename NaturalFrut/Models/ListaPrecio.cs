using NaturalFrut.App_BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NaturalFrut.Models
{ 
    [Table("ListaPrecios")]
    public class ListaPrecio : IEntity
    {
        public int ID { get; set; }

        
        public int? ListaID { get; set; }

        [Required]
        [Display(Name = "Producto")]
        public int ProductoID { get; set; }        

        [Required]
        [Display(Name = "Precio x Kg.")]
        public double PrecioXKG { get; set; }

        [Required]
        [Display(Name = "Precio x Bulto Cerrado")]
        public double PrecioXBultoCerrado{ get; set; }

        [Required]
        [Display(Name = "Kg. del Bulto Cerrado")]
        public double KGBultoCerrado { get; set; }

        [Required]
        [Display(Name = "Precio x Unidad")]
        public double PrecioXUnidad { get; set; }


        public Lista Lista { get; set; }

        public Producto Producto { get; set; }

        

    }
}