using iTextSharp.text;
using NaturalFrut.App_BLL;
using NaturalFrut.App_BLL.ViewModels;
using NaturalFrut.Helpers;
using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace NaturalFrut.Controllers
{
    [Authorize(Roles = "Administrator, User")]
    public class VentaMinoristaController : Controller
    {
        private readonly VentaMinoristaLogic ventaMinoristaBL;
        

        public VentaMinoristaController(VentaMinoristaLogic VentaMinoristaLogic)
        {
            ventaMinoristaBL = VentaMinoristaLogic;
        }

        // GET: VentaMinorista
        public ActionResult Index()
        {

            var ventaMinorista = ventaMinoristaBL.GetAllVentaMinorista();

            return View(ventaMinorista);
        }

        public ActionResult IndexReporte()
        {

            return View();
        }

       
        public ActionResult NuevaVentaMinorista()
        {

            VentaMinoristaViewModel viewModel = new VentaMinoristaViewModel();

            var ultimaVenta = ventaMinoristaBL.GetNumeroDeVenta();


            if (ultimaVenta == null)
            {
                //No se ha cargado ventas en el sistema, asignamos numero cero
                viewModel.NumeroVenta = 0;
            }
            else
            {
                //Asignamos número siguiente a la última venta cargada
                viewModel.NumeroVenta = ultimaVenta.NumeroVenta + 1;
            }

            return View("VentaMinoristaForm", viewModel);
        }

        public ActionResult EditarVentaMinorista(int Id)
        {
            var vtaMinorista = ventaMinoristaBL.GetVentaMinoristaById(Id);

            if (vtaMinorista == null)
                return HttpNotFound();

            VentaMinoristaViewModel viewModel = new VentaMinoristaViewModel(vtaMinorista);

            return View("VentaMinoristaForm", viewModel);

        }

        public ActionResult VerVentaMinorista(int Id)
        {
            var vtaMinorista = ventaMinoristaBL.GetVentaMinoristaById(Id);

            if (vtaMinorista == null)
                return HttpNotFound();

            VentaMinoristaViewModel viewModel = new VentaMinoristaViewModel(vtaMinorista);

            return View("VentaMinoristaFormView", viewModel);
        }

        public ActionResult NuevoReporteVentaMinorista()
        {
            return View("ReporteVentaMinoristaForm");
        }

        public ActionResult GetReporteVentasMinoristasAsync(DateTime fechaDesde, DateTime fechaHasta)
        {

            try
            {

                var reporteVentas = ventaMinoristaBL.GetAllVentaMinoristaSegunFechas(fechaDesde, fechaHasta);

                if (reporteVentas == null)
                    throw new Exception("No se encontraron Ventas según el rango de fecha seleccionada");


                return Json(new { Success = true, ReporteVentas = reporteVentas }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                return Json(new { Success = false, Error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            
        }

        [AllowAnonymous]
        public ActionResult GenerarReporteTxt(string ventas)
        {

           
                GenerarTxt txt = new GenerarTxt(ventaMinoristaBL);

                txt.CrearTxt(ventas);                

                return View("");
           
        }

        [AllowAnonymous]
        public ActionResult GenerarReporteExcel(string ventas)
        {

            ExcelExportHelper excel = new ExcelExportHelper(ventaMinoristaBL);

            DataTable tablaCompras = excel.ArmarExcel(ventas, Constants.VENTA_MINORISTA_EXCEL);

            string[] columns = { "Id", "Fecha", "Local", "Importe Informe Z", "IVA", "Importe IVA",
                "Factura nº", "Tipo Factura", "Primer Número Ticket", "Último Número Ticket"};

            //byte[] filecontent = ExcelExportHelper.ExportExcel(technologies, "Technology", true, columns);
            byte[] filecontent = ExcelExportHelper.ExportExcel(tablaCompras, "Reporte Ventas Minoristas", true, columns);

            string fecha = string.Format("{0}{1}{2}", DateTime.Now.Date.Day, DateTime.Now.Date.Month, DateTime.Now.Date.Year);


            return File(filecontent, ExcelExportHelper.ExcelContentType, "ReporteVentaMinorista_" + fecha + ".xlsx");

        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarVentaMinorista(VentaMinorista ventaMinorista)
        {

            if (!ModelState.IsValid)
            {

                VentaMinoristaViewModel viewModel = new VentaMinoristaViewModel(ventaMinorista);

                return View("VentaMinoristaForm", viewModel);
            }

            if (ventaMinorista.ID == 0)
            {
                //Agregamos nueva Venta Mayorista
                ventaMinoristaBL.AddVentaMinorista(ventaMinorista);
            }
            else
            {
                //Edicion de Venta Mayorista existente
                ventaMinoristaBL.UpdateVentaMinorista(ventaMinorista);
            }

            return RedirectToAction("Index", "VentaMinorista");

        }

        public ActionResult ReporteVentas()
        {
            return View("Reportes\\ReporteVentaEntreFechas");
        }

        public ActionResult GetReporteVentaAsync(DateTime fechaDesde, DateTime fechaHasta)
        {

            try
            {

                var reporteVenta = ventaMinoristaBL.GetAllVentaMinoristaSegunFechas(fechaDesde, fechaHasta);

                if (reporteVenta == null)
                    throw new Exception("No se encontraron Compras según el rango de fecha seleccionada");


                return Json(new { Success = true, ReporteVenta = reporteVenta }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                return Json(new { Success = false, Error = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }


    }
}