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

        //public string Cliente { get; set; }

        public DateTime Fecha { get; set; }

        public float SaldoAnterior { get; set; }

        public float EntregaEfectivo { get; set; }

        public float Saldo { get; set; }

        //public string Vendedor { get; set; }

        public IEnumerable<Producto> Productos { get; set; }

        public IEnumerable<Cliente> Clientes { get; set; }

        public IEnumerable<Vendedor> Vendedores { get; set; }

        public bool NoConcretado { get; set; } 


        public VentaViewModel()
        {
            ID = 0;
        }



    }
}