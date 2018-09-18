using NaturalFrut.App_BLL;
using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using Tababular;

namespace NaturalFrut.Controllers
{
    public class GenerarTxt : Page
    {
        private VentaMinoristaLogic ventaMinoristaBL;
        
        public GenerarTxt(VentaMinoristaLogic ventaMinoristaBL)
        {
            this.ventaMinoristaBL = ventaMinoristaBL;
        }
        
        public void CrearTxt(string ventas)
        {

            var numeroVentas = ventas.Split(',');

            VentaMinorista ventaInDB = new VentaMinorista();
            VentaMinoristaReporte reporteTemp = new VentaMinoristaReporte();
            List<VentaMinoristaReporte> ventaReporte = new List<VentaMinoristaReporte>();

            foreach (var item in numeroVentas)
            {
                if(item != string.Empty)
                {
                    ventaInDB = ventaMinoristaBL.GetVentaMinoristaByNumeroVenta(int.Parse(item));

                    //Guardamos los datos necesarios para el reporte
                    reporteTemp = new VentaMinoristaReporte();
                    reporteTemp.ID = ventaInDB.ID;
                    reporteTemp.Fecha = ventaInDB.Fecha.Date.ToString("dd/MM/yyyy");
                    reporteTemp.Local = ventaInDB.Local;
                    reporteTemp.Importe_Informe_Z = String.Format("{0:c}", ventaInDB.ImporteInformeZ);
                    reporteTemp.IVA = "1," + ventaInDB.Iva.ToString();
                    reporteTemp.Importe_IVA = String.Format("{0:c}", ventaInDB.ImporteIva);
                    reporteTemp.Factura_N = ventaInDB.NumFactura;
                    reporteTemp.Tipo_Factura = ventaInDB.TipoFactura;
                    reporteTemp.Primer_Numero_Tic = ventaInDB.PrimerNumeroTicket;
                    reporteTemp.Ultimo_Numero_Tic = ventaInDB.UltimoNumeroTicket;

                    ventaReporte.Add(reporteTemp);
                }
                    
            }


            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    StringBuilder sb = new StringBuilder();

                    var tableFormatter = new TableFormatter();
                    //List<VentaMinoristaReporte> listaModif = new List<VentaMinoristaReporte>();      

                    //foreach (var venta in ventaReporte)
                    //{
                    //    var obj = new VentaMinoristaReporte()
                    //    {
                    //        ID = venta.ID,
                    //        Fecha = venta.Fecha.Date,
                    //        Local = venta.Local,
                    //        Importe_Informe_Z = venta.Importe_Informe_Z,
                    //        IVA = venta.IVA,
                    //        Importe_IVA = venta.Importe_IVA,
                    //        Factura_N = venta.Factura_N,
                    //        Tipo_Factura = venta.Tipo_Factura,
                    //        Primer_Numero_Tic = venta.Primer_Numero_Tic,
                    //        Ultimo_Numero_Tic = venta.Ultimo_Numero_Tic
                    //    };
                        
                    //    listaModif.Add(obj);                        
                    //}

                    sb.Append(tableFormatter.FormatObjects(ventaReporte));

                    //Export HTML String as PDF.
                    StringReader sr = new StringReader(sb.ToString());
                  
                    HttpContext.Current.Response.ContentType = "text/plain";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=ReporteVenta_.txt");
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    HttpContext.Current.Response.Write(sb.ToString());
                    HttpContext.Current.Response.End();
                }
            }

        }

    }
}