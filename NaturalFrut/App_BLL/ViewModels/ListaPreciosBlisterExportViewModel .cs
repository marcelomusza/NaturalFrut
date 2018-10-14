using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using NaturalFrut.Models;

namespace NaturalFrut.App_BLL.ViewModels
{
    public class ListaPreciosBlisterExportViewModel
    {

       
        public IEnumerable<ListaPrecioBlister> ListaPreciosBlister { get; set; }

        public IEnumerable<Categoria> Categorias { get; set; }

        public IEnumerable<Marca> Marcas { get; set; }

       
    }
}