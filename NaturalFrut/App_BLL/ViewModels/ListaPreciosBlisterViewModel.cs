using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using NaturalFrut.Models;

namespace NaturalFrut.App_BLL.ViewModels
{
    public class ListaPreciosBlisterViewModel
    {

        public int? ID { get; set; }

        [Required]
        public int Gramos { get; set; }

        [Required]
        public string Precio { get; set; }        


        public IEnumerable<Producto> Producto { get; set; }

        [Required]       
        public int? ProductoID { get; set; }

      



        public ListaPreciosBlisterViewModel()
        {
            ID = 0;
        }

        public ListaPreciosBlisterViewModel(ListaPrecioBlister listaPrecioBlister)
        {
            ID = listaPrecioBlister.ID;                 
            ProductoID = listaPrecioBlister.ProductoID;
            Gramos = listaPrecioBlister.Gramos;
            Precio = listaPrecioBlister.Precio;
            
        }

        public string Titulo
        {

            get
            {
                return (ID != 0) ? "Editar Productos" : "Ingresar Productos";
            }

        }

    }
}