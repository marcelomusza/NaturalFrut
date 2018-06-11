using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NaturalFrut.App_BLL.ViewModels
{
    public class VentaViewModel
    {

        public int? ID { get; set; }

        public string Nombre { get; set; }

        public string Prop1 { get; set; }

        public string Prop2 { get; set; }

        public string Prop3 { get; set; }
        
        public IEnumerable<Producto> Productos { get; set; }


        public VentaViewModel()
        {
            ID = 0;
        }



    }
}