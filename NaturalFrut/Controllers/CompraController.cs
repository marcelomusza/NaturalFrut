﻿using NaturalFrut.App_BLL;
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

namespace NaturalFrut.Controllers
{
    public class CompraController : Controller
    {
        private readonly CompraLogic compraBL;
        private readonly ProveedorLogic proveedorBL;
        private readonly CommonLogic commonBL;
        private readonly StockLogic stockBL;
        private readonly ProductoXCompraLogic productoxCompraBL;

        public CompraController(CompraLogic CompraLogic, 
            ProveedorLogic ProveedorLogic,
            CommonLogic CommonLogic,
            StockLogic StockLogic,
            ProductoXCompraLogic ProductoXCompraLogic)
        {
            compraBL = CompraLogic;
            proveedorBL = ProveedorLogic;
            commonBL = CommonLogic;
            stockBL = StockLogic;
            productoxCompraBL = ProductoXCompraLogic;
        }

        // GET: Compra
        public ActionResult Index()
        {
            
            var compra = compraBL.GetAllCompra();

            return View(compra);
        }



        public ActionResult Compra()
        {
            var compra = compraBL.GetAllCompra();

            return View(compra);

        }

        public ActionResult NuevaCompra()
        {

            var ultimaCompra = compraBL.GetNumeroDeCompra();

            //Cargamos datos a mandar a la view
            ViewBag.Fecha = DateTime.Now;
            ViewBag.Clasificacion = commonBL.GetAllClasificacion();
            //ViewBag.TipoDeUnidadBlister = Constants.TIPODEUNIDAD_BLISTER;

            if (ultimaCompra == null)
            {
                //No se ha cargado ventas en el sistema, asignamos numero cero
                ViewBag.NumeroCompra = 0;
            }
            else
            {
                //Asignamos número siguiente a la última venta cargada
                ViewBag.NumeroCompra = ultimaCompra.NumeroCompra + 1;
            }



            return View("CompraForm");
        }

        //    public ActionResult EditarVentaMayorista(int Id)
        //    {

        //        var vtaMayorista = ventaMayoristaBL.GetVentaMayoristaById(Id);
        //        //var productosXVentaMayorista = ventaMayoristaBL.GetProductosXVentaMayorista(Id);

        //        foreach (var producto in vtaMayorista.ProductosXVenta)
        //        {

        //            if(producto.Producto.Categoria != null)
        //            {
        //                if (producto.Producto.EsBlister)
        //                    producto.Producto.Nombre = producto.Producto.Nombre + " (" + producto.Producto.Categoria.Nombre + ") - BLISTER - ";
        //                else
        //                    producto.Producto.Nombre = producto.Producto.Nombre + " (" + producto.Producto.Categoria.Nombre + ")";
        //            }
        //            else
        //            {
        //                if (producto.Producto.EsBlister)
        //                    producto.Producto.Nombre = producto.Producto.Nombre + " (" + producto.Producto.Marca.Nombre + ") - BLISTER - ";
        //                else
        //                    producto.Producto.Nombre = producto.Producto.Nombre + " (" + producto.Producto.Marca.Nombre + ")";
        //            }
        //        }

        //        ViewBag.TipoDeUnidadBlister = Constants.TIPODEUNIDAD_BLISTER;
        //        ViewBag.VentaMayoristaID = Id;

        //        if (vtaMayorista == null)
        //            return HttpNotFound();

        //        return View("VentaMayoristaFormEdit", vtaMayorista);
        //    }

        //    //[HttpPost]
        //    //[ValidateAntiForgeryToken]
        //    //public ActionResult GuardarVentaMayorista(VentaMayorista ventaMayorista)
        //    //{

        //    //    if (!ModelState.IsValid)
        //    //    {

        //    //        VentaMayoristaViewModel viewModel = new VentaMayoristaViewModel(ventaMayorista)
        //    //        {

        //    //        };

        //    //        return View("VentaMayoristaForm", viewModel);
        //    //    }

        //    //    if (ventaMayorista.ID == 0)
        //    //    {
        //    //        //Agregamos nueva Venta Mayorista
        //    //        ventaMayoristaBL.AddVentaMayorista(ventaMayorista);
        //    //    }
        //    //    else
        //    //    {
        //    //        //Edicion de Venta Mayorista existente
        //    //        ventaMayoristaBL.UpdateVentaMayorista(ventaMayorista);
        //    //    }

        //    //    return RedirectToAction("Clientes", "Admin");

        //    //}


        public ActionResult NuevoProveedor()
        {

            Proveedor model = new Proveedor();

           

            return PartialView(model);
        }

        public ActionResult NuevaClasificacion()
        {
            Clasificacion model = new Clasificacion();


            return PartialView(model);
        }

        //    public ActionResult GetCondicionIVAAsync()
        //    {
        //        return Json(clienteBL.GetCondicionIvaList(), JsonRequestBehavior.AllowGet);
        //    }

        //    public ActionResult GetTipoClienteAsync()
        //    {
        //       return Json(clienteBL.GetTipoClienteList(), JsonRequestBehavior.AllowGet);
        //    }

        //    public ActionResult GetListaAsociadaAsync()
        //    {
        //        return Json(clienteBL.GetListaList(), JsonRequestBehavior.AllowGet);
        //    }

        //    public ActionResult CalcularStockYValorProductoAsync(int clienteID, int productoID, int cantidad, int tipoUnidadID, int counter)
        //    {

        //        try
        //        {
        //            double importe;
        //            double importeTotal;

        //            Stock productoSegunStock = stockBL.ValidarStockProducto(productoID, tipoUnidadID);

        //            if (productoSegunStock == null)
        //                throw new Exception("El Producto no tiene Stock Asociado para el Tipo de Unidad seleccionado. Revisar la carga del Stock en el sistema antes de continuar.");

        //            ListaPrecio productoSegunLista = ventaMayoristaBL.CalcularImporteSegunCliente(clienteID, productoID, cantidad);

        //            switch (tipoUnidadID)
        //            {
        //                case Constants.PRECIO_X_KG:

        //                    if (Convert.ToDouble(productoSegunLista.PrecioXKG) > 0)
        //                    {
        //                        //Casos en la lista de precios donde hay precio x kg y precio por bulto en base a cantidad
        //                        if (Convert.ToDouble(productoSegunLista.KGBultoCerrado) != 0 && (cantidad >= Convert.ToDouble(productoSegunLista.KGBultoCerrado)))
        //                            importe = Convert.ToDouble(productoSegunLista.PrecioXBultoCerrado);
        //                        else
        //                            importe = Convert.ToDouble(productoSegunLista.PrecioXKG);

        //                    }
        //                    else
        //                    {
        //                        //Casos en la lista de precios donde no hay precio x kg pero sí hay precio por bulto
        //                        if (Convert.ToDouble(productoSegunLista.PrecioXBultoCerrado) > 0)
        //                            importe = Convert.ToDouble(productoSegunLista.PrecioXBultoCerrado);
        //                        else
        //                            throw new Exception("El Producto seleccionado no tiene precios correctamente cargados en el sistema, " +
        //                                "por favor revisar la tabla de Lista de Precios antes de continuar");

        //                    }

        //                    break;

        //                case Constants.PRECIO_X_UNIDAD:

        //                    if (Convert.ToDouble(productoSegunLista.PrecioXUnidad) > 0)
        //                    {
        //                        //Casos en la lista de precios donde hay precio x unidad y precio por bulto en base a cantidad
        //                        if (Convert.ToDouble(productoSegunLista.KGBultoCerrado) != 0 && (cantidad >= Convert.ToDouble(productoSegunLista.KGBultoCerrado)))
        //                            importe = Convert.ToDouble(productoSegunLista.PrecioXBultoCerrado);
        //                        else
        //                            importe = Convert.ToDouble(productoSegunLista.PrecioXUnidad);

        //                    }
        //                    else
        //                    {
        //                        //Casos en la lista de precios donde no hay precio x unidad pero sí hay precio por bulto
        //                        if (Convert.ToDouble(productoSegunLista.PrecioXBultoCerrado) > 0)
        //                            importe = Convert.ToDouble(productoSegunLista.PrecioXBultoCerrado);
        //                        else
        //                            throw new Exception("El Producto seleccionado no tiene precios correctamente cargados en el sistema, " +
        //                                "por favor revisar la tabla de Lista de Precios antes de continuar");

        //                    }


        //                    break;

        //                default:
        //                    importe = 0;
        //                    break;

        //            }

        //            //Sumamos el importe total
        //            importeTotal = importe * cantidad;


        //            return Json(new { Success = true, Importe = importe, ImporteTotal = importeTotal, Counter = counter, Stock = productoSegunStock.Cantidad }, JsonRequestBehavior.AllowGet);


        //        }
        //        catch (Exception ex)
        //        {
        //            return Json(new { Success = false, Error = ex.Message }, JsonRequestBehavior.AllowGet);
        //        }


        //    }


        public ActionResult GetTiposDeUnidadDynamicAsync(int counter)
        {

            List<TipoDeUnidad> tiposDeUnidad = commonBL.GetAllTiposDeUnidad();

            return Json(new { TiposDeUnidad = tiposDeUnidad, Counter = counter }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ValidarTipoDeProductoAsync(int productoID, int counter)
        {

            bool esBlister = compraBL.ValidateTipoDeProducto(productoID);

            return Json(new { EsBlister = esBlister, Counter = counter }, JsonRequestBehavior.AllowGet);

        }

        //    public ActionResult GetSaldoClienteAsync(int clienteID)
        //    {

        //        try
        //        {

        //            Cliente cliente = clienteBL.GetClienteById(clienteID);                

        //            if (cliente == null)
        //                throw new Exception("Cliente invalido al cargar Saldo Deudor");

        //            var saldo = cliente.Saldo;

        //            return Json(new { Saldo = saldo }, JsonRequestBehavior.AllowGet);

        //        }
        //        catch (Exception ex)
        //        {
        //            return Json(new { Success = false, Error = ex.Message }, JsonRequestBehavior.AllowGet);
        //        }


        //    }

        //    public ActionResult CalcularStockBlisterAsync(int productoID, int tipoUnidadID, int counter)
        //    {
        //        try
        //        {
        //            Stock productoSegunStock = stockBL.ValidarStockProducto(productoID, tipoUnidadID);

        //            return Json(new { Success = true, Counter = counter, Stock = productoSegunStock.Cantidad }, JsonRequestBehavior.AllowGet);


        //        }
        //        catch (Exception ex)
        //        {
        //            return Json(new { Success = false, Error = ex.Message }, JsonRequestBehavior.AllowGet);
        //        }



        //    }

        //    public ActionResult CalcularValorBlisterAsync(int clienteID, int productoID, int cantidad, int tipoUnidadID, int counter)
        //    {

        //        try
        //        {
        //            double importe;
        //            double importeTotal;

        //            Stock productoSegunStock = stockBL.ValidarStockProducto(productoID, tipoUnidadID);

        //            if (productoSegunStock == null)
        //                throw new Exception("El Producto no tiene Stock Asociado para el Tipo de Unidad seleccionado. Revisar la carga del Stock en el sistema antes de continuar.");

        //            ListaPrecioBlister productoBlisterSegunLista = ventaMayoristaBL.CalcularImporteBlisterSegunCliente(productoID);

        //            if (productoBlisterSegunLista == null)
        //                throw new Exception("Error al cargar los precios de producto");

        //            importe = Convert.ToDouble(productoBlisterSegunLista.Precio);

        //            //Sumamos el importe total
        //            importeTotal = importe * cantidad;

        //            return Json(new { Success = true, Importe = importe, ImporteTotal = importeTotal, Counter = counter, Stock = productoSegunStock.Cantidad }, JsonRequestBehavior.AllowGet);


        //        }
        //        catch (Exception ex)
        //        {
        //            return Json(new { Success = false, Error = ex.Message }, JsonRequestBehavior.AllowGet);
        //        }


        //    }



        //    [AllowAnonymous]
        //    public ActionResult GenerarNotaPedido(int Id)
        //    {

        //        var venta = ventaMayoristaBL.GetVentaMayoristaById(Id);

        //        VentaMayoristaViewModel viewModel = new VentaMayoristaViewModel(venta)
        //        {
        //           // Clientes = clienteBL.GetClienteById(venta.ClienteID),
        //            //ProductoXVenta = productoxVentaBL.GetProductoXVentaByIdVenta(venta.ID)

        //        };



        //        ViewBag.ProductoXVenta = productoxVentaBL.GetProductoXVentaByIdVenta(venta.ID);




        //        return View("NotaDePedidoForm", viewModel);
        //    }


        //    [AllowAnonymous]
        //    public ActionResult PrintAll(int Id)
        //    {
        //        // var venta = ventaMayoristaBL.GetVentaMayoristaById(Id);
        //        //var q = new ActionAsPdf("GenerarNotaPedido", new { Id = Id }) { FileName = "ExamReport.pdf",
        //        //    PageSize = Size.A4,
        //        //    CustomSwitches = "--disable-smart-shrinking",
        //        //    PageMargins = { Left = 20, Bottom = 20, Right = 20, Top = 20 },
        //        //    /* CustomSwitches =
        //        //         "--header-center \"Name: " + venta.Cliente.Nombre + "  DOS: " +
        //        //         DateTime.Now.Date.ToString("MM/dd/yyyy") + "  Page: [page]/[toPage]\"" +
        //        //         " --header-line --footer-font-size \"9\" "*/

        //        //};

        //        GenerarPdf pdf = new GenerarPdf(ventaMayoristaBL, productoxVentaBL);

        //        pdf.CrearPdf(Id);

        //        var venta = ventaMayoristaBL.GetAllVentaMayorista();

        //        if(venta == null)
        //            return HttpNotFound();

        //        return View(venta);



        //    }

    }
}