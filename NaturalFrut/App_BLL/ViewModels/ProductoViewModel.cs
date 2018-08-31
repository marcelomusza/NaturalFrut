using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using NaturalFrut.Models;

namespace NaturalFrut.App_BLL.ViewModels
{
    public class ProductoViewModel
    {

        public int? ID { get; set; }

        [Required]
        public string Nombre { get; set; }

        
        [Display(Name = "Categoría")]
        public int? CategoriaId { get; set; }

        public IEnumerable<Categoria> Categoria { get; set; }

        
        [Display(Name = "Marca")]
        public int? MarcaId { get; set; }

        public IEnumerable<Marca> Marca { get; set; }

        [Display(Name = "El producto forma parte de la lista de Blister?")]
        [Required]
        public bool EsBlister { get; set; }

        [Display(Name = "El producto es Mix?")]
        [Required]
        public bool EsMix { get; set; }



        public ProductoViewModel()
        {
            ID = 0;
        }

        public ProductoViewModel(Producto producto)
        {
            ID = producto.ID;
            Nombre = producto.Nombre;

            if(producto.CategoriaId != null)
                CategoriaId = producto.CategoriaId;
            if(producto.MarcaId != null)
                MarcaId = producto.MarcaId;

            EsBlister = producto.EsBlister;
        }

        public string Titulo
        {

            get
            {
                return (ID != 0) ? "Editar Producto" : "Nuevo Producto";
            }

        }

    }
}