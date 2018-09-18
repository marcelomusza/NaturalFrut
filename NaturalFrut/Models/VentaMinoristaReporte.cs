using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NaturalFrut.Models
{
    
    public class VentaMinoristaReporte
    {
        
        public int ID { get; set; }

        public string Fecha { get; set; }
        
        public string Local { get; set; }
        
        public string Importe_Informe_Z { get; set; }
        
        public string IVA { get; set; }
        
        public string Importe_IVA { get; set; }
        
        public int Factura_N { get; set; }
        
        public string Tipo_Factura { get; set; }
        
        public int Primer_Numero_Tic { get; set; }
        
        public int Ultimo_Numero_Tic { get; set; }

    }
}