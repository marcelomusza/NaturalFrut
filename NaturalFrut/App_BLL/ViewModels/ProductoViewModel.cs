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
        public string Categoria { get; set; }

        [Required]
        public string Marca { get; set; }

        public ProductoViewModel()
        {
            ID = 0;
        }

        public ProductoViewModel(Producto producto)
        {
            ID = producto.ID;
            Descripcion = producto.Descripcion;
            Cantidad = producto.Cantidad;
            Categoria = producto.Categoria;
            Marca = producto.Marca;
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