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
        public string Descripcion { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Display(Name = "Categoría")]
        public int CategoriaId { get; set; }

        public IEnumerable<Categoria> Categoria { get; set; }

        [Required]
        [Display(Name = "Marca")]
        public int MarcaId { get; set; }

        public IEnumerable<Marca> Marca { get; set; }

        public ProductoViewModel()
        {
            ID = 0;
        }

        public ProductoViewModel(Producto producto)
        {
            ID = producto.ID;
            Descripcion = producto.Descripcion;
            Cantidad = producto.Cantidad;
            MarcaId = producto.MarcaId;

            CategoriaId = producto.CategoriaId;
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