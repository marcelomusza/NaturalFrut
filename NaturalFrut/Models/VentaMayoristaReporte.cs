using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NaturalFrut.Models
{
    
    public class VentaMayoristaReporte
    {

        //public int ID { get; set; }

        public string Fecha { get; set; }

        public string EntregaEfectivo { get; set; }

        public string Debe { get; set; }

        public string SaldoAFavor { get; set; }

        public int ClienteID { get; set; }

        public int VendedorID { get; set; }

        public string SumaTotal { get; set; }

        public string Cantidad { get; set; }

        public string Importe { get; set; }

        public string Total { get; set; }

        public int TipoDeUnidadID { get; set; }

        public int ProductoID { get; set; }

        public int VentaID { get; set; }


    }
}