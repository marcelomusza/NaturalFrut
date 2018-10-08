using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NaturalFrut.App_BLL.ViewModels
{
    public class VentaMayoristaViewModel
    {
        private VentaMayorista ventaMayorista;

        public int? ID { get; set; }

        public Cliente ClienteObj { get; set; }

        public String Cliente { get; set; }

        public DateTime Fecha { get; set; }

        public double SaldoAnterior { get; set; }

        public double EntregaEfectivo { get; set; }

        public double Total { get; set; }

        public double SumaTotalMasSaldo { get; set; }

        public double Saldo { get; set; }

        public double NuevoSaldo { get; set; }

        public int NumeroVenta { get; set; }

        public Vendedor VendedorObj { get; set; }

        public string Vendedor { get; set; }

        //public IEnumerable<Producto> Productos { get; set; }

        public IEnumerable<Cliente> Clientes { get; set; }

        public IEnumerable<Vendedor> Vendedores { get; set; }

        public IEnumerable<TipoDeUnidad> TiposDeUnidad { get; set; }

        public IEnumerable<ProductoXVenta> ProductoXVenta { get; set; }

        public bool NoConcretado { get; set; }


        public VentaMayoristaViewModel()
        {
            ID = 0;
        }

        public VentaMayoristaViewModel(VentaMayorista ventaMayorista)
        {
            NumeroVenta = ventaMayorista.NumeroVenta;
            Fecha = ventaMayorista.Fecha;
            ClienteObj = ventaMayorista.Cliente;
            VendedorObj = ventaMayorista.Vendedor;
            Total = ventaMayorista.SumaTotal;
            EntregaEfectivo = ventaMayorista.EntregaEfectivo;

            this.ventaMayorista = ventaMayorista;
        }
    }
}
