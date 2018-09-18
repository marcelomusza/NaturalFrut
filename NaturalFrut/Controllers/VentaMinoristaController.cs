using iTextSharp.text;
using NaturalFrut.App_BLL;
using NaturalFrut.App_BLL.ViewModels;
using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace NaturalFrut.Controllers
{
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

    }
}