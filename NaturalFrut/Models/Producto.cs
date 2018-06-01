using NaturalFrut.App_BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NaturalFrut.Models
{
    [Table("Productos")]
    public class Producto : IEntity
    {
        public int ID { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        public int Cantidad { get; set; }
        
        public Categoria Categoria { get; set; }

        [Required]
        public int CategoriaId { get; set; }

        
        public Marca Marca { get; set; }

        [Required]
        public int MarcaId { get; set; }

    }
}