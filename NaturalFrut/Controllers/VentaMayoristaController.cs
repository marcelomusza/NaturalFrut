using NaturalFrut.App_BLL;
using NaturalFrut.App_BLL.ViewModels;
using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NaturalFrut.Helpers;
using Rotativa;
using Rotativa.Options;

namespace NaturalFrut.Controllers
{
    public class VentaMayoristaController : Controller
    {
        private readonly VentaMayoristaLogic ventaMayoristaBL;
        private readonly ClienteLogic clienteBL;
        private readonly CommonLogic commonBL;
        private readonly StockLogic stockBL;
        private readonly ProductoXVentaLogic productoxVentaBL;

        public VentaMayoristaController(VentaMayoristaLogic VentaMayoristaLogic, 
            ClienteLogic ClienteLogic,
            CommonLogic CommonLogic,
            StockLogic StockLogic,
            ProductoXVentaLogic ProductoXVentaLogic)
        {
            ventaMayoristaBL = VentaMayoristaLogic;
            clienteBL = ClienteLogic;
            commonBL = CommonLogic;
            stockBL = StockLogic;
            productoxVentaBL = ProductoXVentaLogic;
        }

        // GET: Venta
        public ActionResult Index()
        {
            
            var venta = ventaMayoristaBL.GetAllVentaMayorista();

            return View(venta);
        }

        

        //public ActionResult VentaMayorista()
        //{
        //    var venta = ventaMayoristaBL.GetAllVentaMayorista();

        //    return View(venta);

        //}

        public ActionResult NuevaVentaMayorista()
        {

            var ultimaVenta = ventaMayoristaBL.GetNumeroDeVenta();            

            //Cargamos datos a mandar a la view
            ViewBag.Fecha = DateTime.Now;
            ViewBag.Vendedores = ventaMayoristaBL.GetVendedorList();

            if (ultimaVenta == null)
            {
                //No se ha cargado ventas en el sistema, asignamos numero cero
                ViewBag.NumeroVenta = 0;
            }
            else
            {
                //Asignamos número siguiente a la última venta cargada
                ViewBag.NumeroVenta = ultimaVenta.NumeroVenta + 1;
            }



            return View("VentaMayoristaForm");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult GuardarVentaMayorista(VentaMayorista ventaMayorista)
        //{

        //    if (!ModelState.IsValid)
        //    {

        //        VentaMayoristaViewModel viewModel = new VentaMayoristaViewModel(ventaMayorista)
        //        {
                    
        //        };

        //        return View("VentaMayoristaForm", viewModel);
        //    }

        //    if (ventaMayorista.ID == 0)
        //    {
        //        //Agregamos nueva Venta Mayorista
        //        ventaMayoristaBL.AddVentaMayorista(ventaMayorista);
        //    }
        //    else
        //    {
        //        //Edicion de Venta Mayorista existente
        //        ventaMayoristaBL.UpdateVentaMayorista(ventaMayorista);
        //    }

        //    return RedirectToAction("Clientes", "Admin");

        //}


        public ActionResult NuevoCliente()
        {

            var condicionIva = clienteBL.GetCondicionIvaList();
            var tipoCliente = clienteBL.GetTipoClienteList();

            ClienteViewModel viewModel = new ClienteViewModel
            {
                CondicionIVA = condicionIva,
                TipoCliente = tipoCliente
            };

            return PartialView(viewModel);
        }

        public ActionResult NuevoVendedor()
        {
            Vendedor model = new Vendedor();


            return PartialView(model);
        }

        public ActionResult GetCondicionIVAAsync()
        {
            return Json(clienteBL.GetCondicionIvaList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTipoClienteAsync()
        {
           return Json(clienteBL.GetTipoClienteList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListaAsociadaAsync()
        {
            return Json(clienteBL.GetListaList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CalcularStockYValorProductoAsync(int clienteID, int productoID, int cantidad, int tipoUnidadID, int counter)
        {

            double importe;
            double importeTotal;

            Stock productoSegunStock = stockBL.ValidarStockProducto(productoID, tipoUnidadID);

            ListaPrecio productoSegunLista = ventaMayoristaBL.CalcularImporteSegunCliente(clienteID, productoID, cantidad);
            

            switch(tipoUnidadID)
            {
                case Constants.PRECIO_X_KG:
                    importe = productoSegunLista.PrecioXKG;
                    break;
                case Constants.PRECIO_X_BULTO:
                    importe = productoSegunLista.PrecioXBultoCerrado;
                    break;
                case Constants.PRECIO_X_UNIDAD:
                    importe = productoSegunLista.PrecioXUnidad;
                    break;
                default:
                    importe = 0;
                    break;

            }

            //Sumamos el importe total
            importeTotal = importe * cantidad;


            return Json(new { Importe = importe, ImporteTotal = importeTotal, Counter = counter, Stock = productoSegunStock.Cantidad }, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult GetTiposDeUnidadDynamicAsync(int counter)
        {

            List<TipoDeUnidad> tiposDeUnidad = commonBL.GetAllTiposDeUnidad();

            return Json(new { TiposDeUnidad = tiposDeUnidad, Counter = counter }, JsonRequestBehavior.AllowGet);
        }


        [AllowAnonymous]
        public ActionResult GenerarNotaPedido(int Id)
        {

            var venta = ventaMayoristaBL.GetVentaMayoristaById(Id);

            VentaMayoristaViewModel viewModel = new VentaMayoristaViewModel(venta)
            {
               // Clientes = clienteBL.GetClienteById(venta.ClienteID),
                ProductoXVenta = productoxVentaBL.GetProductoXVentaByIdVenta(venta.ID)

            };



            ViewBag.ProductoXVenta = productoxVentaBL.GetProductoXVentaByIdVenta(venta.ID);




            return View("NotaDePedidoForm", viewModel);
        }

        [AllowAnonymous]
        public ActionResult PrintAll(int Id)
        {
            var venta = ventaMayoristaBL.GetVentaMayoristaById(Id);
            var q = new ActionAsPdf("GenerarNotaPedido", new { Id = Id }) { FileName = "ExamReport.pdf",
                PageSize = Size.A4,
                CustomSwitches = "--disable-smart-shrinking",
                PageMargins = { Left = 20, Bottom = 20, Right = 20, Top = 20 },
                /* CustomSwitches =
                     "--header-center \"Name: " + venta.Cliente.Nombre + "  DOS: " +
                     DateTime.Now.Date.ToString("MM/dd/yyyy") + "  Page: [page]/[toPage]\"" +
                     " --header-line --footer-font-size \"9\" "*/

            };


            return q;

        }

    }
}