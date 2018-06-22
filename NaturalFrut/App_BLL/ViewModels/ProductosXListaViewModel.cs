using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using NaturalFrut.Models;

namespace NaturalFrut.App_BLL.ViewModels
{
    public class ProductosXListaViewModel
    {

        public int? ID { get; set; }

        [Required]        
        public string Nombre { get; set; }

        [Required]
        public float Precio { get; set; }

        public IEnumerable<ListaDePrecios> ListaDePrecios { get; set; }

        [Required]
        public int ListaDePreciosId { get; set; }


        public IEnumerable<Cliente> Cliente { get; set; }

        [Required]
        public int ClienteId { get; set; }

        public IEnumerable<Producto> Producto { get; set; }

        [Required]       
        public int ProductoId { get; set; }

        public IEnumerable<TipoDeUnidad> TipoDeUnidad { get; set; }

        [Required]
        public int TipoDeUnidadId { get; set; }



        public ProductosXListaViewModel()
        {
            ID = 0;
        }

        public ProductosXListaViewModel(ProductoXLista productoXLista)
        {
            ID = productoXLista.ID;            
            Precio = productoXLista.Precio;

            ListaDePreciosId = productoXLista.ListaDePreciosID;
            ClienteId = productoXLista.ClienteID;
            ProductoId = productoXLista.ProductoID;
            TipoDeUnidadId = productoXLista.TipoDeUnidadID;
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