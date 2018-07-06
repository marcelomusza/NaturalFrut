using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NaturalFrut.App_BLL.ViewModels
{
    public class VentaMinoristaViewModel
    {

        public int? ID { get; set; }

        public string Local { get; set; }

        public DateTime Fecha { get; set; }

        public double ImporteVentaTotal { get; set; }

        public double ImporteInformeZ { get; set; }

        public double Iva { get; set; }

        public int CantidadPersonas { get; set; }

        public double Promedio { get; set; }

        public double ImporteIva { get; set; }

        public int PrimerNumeroTicket { get; set; }

        public int UltimoNumeroTicket { get; set; }

        public int NumFactura { get; set; }

        public string RazonSocial { get; set; }

        public string TipoFactura { get; set; }

        public double TarjetaVisa { get; set; }

        public double TarjetaVisaDeb { get; set; }

        public double TarjetaMaster { get; set; }

        public double TarjetaMaestro { get; set; }

        public double TarjetaCabal { get; set; }

        public double TotalTarjetas { get; set; }



        public VentaMinoristaViewModel()
        {
            ID = 0;
        }



    }
}