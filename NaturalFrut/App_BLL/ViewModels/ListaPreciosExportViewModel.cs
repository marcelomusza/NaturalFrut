using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using NaturalFrut.Models;

namespace NaturalFrut.App_BLL.ViewModels
{
    public class ListaPreciosExportViewModel
    {

        public int ListaId { get; set; }

        public string Nombre { get; set; }

        public IEnumerable<ListaPrecio> ListaPrecios { get; set; }

        public IEnumerable<Categoria> Categorias { get; set; }

        public IEnumerable<Marca> Marcas { get; set; }

       
    }
}