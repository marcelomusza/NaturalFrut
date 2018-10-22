using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using NaturalFrut.App_BLL.Interfaces;

namespace NaturalFrut.Models
{

    public class StockUpdate
    {
        public int ID { get; set; }

       
        public int ProductoID { get; set; }

       
        public int TipoDeUnidadID { get; set; }

       
        public double Cantidad { get; set; }

        public double NuevaCantidad { get; set; }

        public Boolean isDelete { get; set; }


        public Producto Producto { get; set; }

        public TipoDeUnidad TipoDeUnidad { get; set; }

    }
}