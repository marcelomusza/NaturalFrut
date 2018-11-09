using NaturalFrut.App_BLL.Interfaces;
using Newtonsoft.Json;
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
        public string PrecioXKG { get; set; }

        [Required]
        [Display(Name = "Precio x Bulto Cerrado")]
        public string PrecioXBultoCerrado { get; set; }

        [Required]
        [Display(Name = "Kg. del Bulto Cerrado")]
        public string KGBultoCerrado { get; set; }

        [Required]
        [Display(Name = "Precio x Unidad")]
        public string PrecioXUnidad { get; set; }

        
        public Lista Lista { get; set; }

       
        public Producto Producto { get; set; }

        

    }
}