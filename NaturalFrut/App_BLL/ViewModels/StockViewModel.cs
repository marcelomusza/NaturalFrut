

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using NaturalFrut.Models;

namespace NaturalFrut.App_BLL.ViewModels
{
    public class StockViewModel
    {

        public int ID { get; set; }

        [Required]
        [Display(Name = "Producto")]
        public int ProductoID { get; set; }

        [Required]
        [Display(Name = "Tipo de Unidad")]
        public int TipoDeUnidadID { get; set; }

        [Required]
        [Display(Name = "Cantidad")]
        public double Cantidad { get; set; }

        public double NuevaCantidad { get; set; }

        public Boolean isDelete { get; set; }

        public IEnumerable<Producto> Producto { get; set; }

        public IEnumerable<TipoDeUnidad> TipoDeUnidad { get; set; }

        public StockViewModel()
        {
            ID = 0;
        }

        public StockViewModel(Stock stock)
        {
            ID = stock.ID;
            ProductoID = stock.ProductoID;
            TipoDeUnidadID = stock.TipoDeUnidadID;
            Cantidad = stock.Cantidad;

        }

        public string Titulo
        {

            get
            {
                return (ID != 0) ? "Editar Stock" : "Agregar Stock";
            }

        }

    }
}
