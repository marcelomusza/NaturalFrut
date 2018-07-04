using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using NaturalFrut.Models;

namespace NaturalFrut.App_BLL.ViewModels
{
    public class ListaPreciosViewModel
    {

        public int? ID { get; set; }

        [Required]
        public double PrecioXKG { get; set; }

        [Required]
        public double PrecioXBultoCerrado { get; set; }

        [Required]
        public double KGBultoCerrado { get; set; }

        [Required]
        public double PrecioXUnidad { get; set; }


        public IEnumerable<Producto> Producto { get; set; }

        [Required]       
        public int ProductoId { get; set; }

        public IEnumerable<Lista> Lista { get; set; }

        [Required]
        public int ListaId { get; set; }



        public ListaPreciosViewModel()
        {
            ID = 0;
        }

        public ListaPreciosViewModel(ListaPrecio listaPrecio)
        {
            ID = listaPrecio.ID;                 
            ProductoId = listaPrecio.ProductoID;
            //ListaId = listaPrecio.ListaID;
            PrecioXKG = listaPrecio.PrecioXKG;
            PrecioXBultoCerrado = listaPrecio.PrecioXBultoCerrado;
            PrecioXUnidad = listaPrecio.PrecioXUnidad;
            KGBultoCerrado = listaPrecio.KGBultoCerrado;
            
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