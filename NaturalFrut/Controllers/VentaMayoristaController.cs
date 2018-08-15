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
using NaturalFrut.Pdf;
//using NaturalFrut.Pdf;

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

            try
            {
                double importe;
                double importeTotal;

                Stock productoSegunStock = stockBL.ValidarStockProducto(productoID, tipoUnidadID);

                if (productoSegunStock == null)
                    throw new Exception("El Producto no tiene Stock Asociado para el Tipo de Unidad seleccionado. Revisar la carga del Stock en el sistema antes de continuar.");

                ListaPrecio productoSegunLista = ventaMayoristaBL.CalcularImporteSegunCliente(clienteID, productoID, cantidad);

                switch (tipoUnidadID)
                {
                    case Constants.PRECIO_X_KG:

                        if (Convert.ToDouble(productoSegunLista.PrecioXKG) > 0)
                        {
                            //Casos en la lista de precios donde hay precio x kg y precio por bulto en base a cantidad
                            if (Convert.ToDouble(productoSegunLista.KGBultoCerrado) != 0 && (cantidad >= Convert.ToDouble(productoSegunLista.KGBultoCerrado)))
                                importe = Convert.ToDouble(productoSegunLista.PrecioXBultoCerrado);
                            else
                                importe = Convert.ToDouble(productoSegunLista.PrecioXKG);

                        }
                        else
                        {
                            //Casos en la lista de precios donde no hay precio x kg pero sí hay precio por bulto
                            if (Convert.ToDouble(productoSegunLista.PrecioXBultoCerrado) > 0)
                                importe = Convert.ToDouble(productoSegunLista.PrecioXBultoCerrado);
                            else
                                throw new Exception("El Producto seleccionado no tiene precios correctamente cargados en el sistema, " +
                                    "por favor revisar la tabla de Lista de Precios antes de continuar");

                        }

                        break;

                    case Constants.PRECIO_X_UNIDAD:

                        if (Convert.ToDouble(productoSegunLista.PrecioXUnidad) > 0)
                        {
                            //Casos en la lista de precios donde hay precio x unidad y precio por bulto en base a cantidad
                            if (Convert.ToDouble(productoSegunLista.KGBultoCerrado) != 0 && (cantidad >= Convert.ToDouble(productoSegunLista.KGBultoCerrado)))
                                importe = Convert.ToDouble(productoSegunLista.PrecioXBultoCerrado);
                            else
                                importe = Convert.ToDouble(productoSegunLista.PrecioXUnidad);

                        }
                        else
                        {
                            //Casos en la lista de precios donde no hay precio x unidad pero sí hay precio por bulto
                            if (Convert.ToDouble(productoSegunLista.PrecioXBultoCerrado) > 0)
                                importe = Convert.ToDouble(productoSegunLista.PrecioXBultoCerrado);
                            else
                                throw new Exception("El Producto seleccionado no tiene precios correctamente cargados en el sistema, " +
                                    "por favor revisar la tabla de Lista de Precios antes de continuar");

                        }


                        break;

                    default:
                        importe = 0;
                        break;

                }

                //Sumamos el importe total
                importeTotal = importe * cantidad;


                return Json(new { Success = true, Importe = importe, ImporteTotal = importeTotal, Counter = counter, Stock = productoSegunStock.Cantidad }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Error = ex.Message }, JsonRequestBehavior.AllowGet);
            }







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
           // var venta = ventaMayoristaBL.GetVentaMayoristaById(Id);
            //var q = new ActionAsPdf("GenerarNotaPedido", new { Id = Id }) { FileName = "ExamReport.pdf",
            //    PageSize = Size.A4,
            //    CustomSwitches = "--disable-smart-shrinking",
            //    PageMargins = { Left = 20, Bottom = 20, Right = 20, Top = 20 },
            //    /* CustomSwitches =
            //         "--header-center \"Name: " + venta.Cliente.Nombre + "  DOS: " +
            //         DateTime.Now.Date.ToString("MM/dd/yyyy") + "  Page: [page]/[toPage]\"" +
            //         " --header-line --footer-font-size \"9\" "*/

            //};

            GenerarPdf pdf = new GenerarPdf(ventaMayoristaBL,productoxVentaBL);

            pdf.CrearPdf(Id);


            return View("Index");



        }

    }
}