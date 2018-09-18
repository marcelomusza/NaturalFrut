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
    public class VentaMayoristaController : Controller
    {
        private readonly VentaMayoristaLogic ventaMayoristaBL;
        private readonly ClienteLogic clienteBL;
        private readonly CommonLogic commonBL;
        private readonly StockLogic stockBL;
        private readonly ProductoXVentaLogic productoxVentaBL;
        private readonly ProductoLogic productoBL;

        public VentaMayoristaController(VentaMayoristaLogic VentaMayoristaLogic, 
            ClienteLogic ClienteLogic,
            CommonLogic CommonLogic,
            StockLogic StockLogic,
            ProductoXVentaLogic ProductoXVentaLogic,
            ProductoLogic ProductoLogic)
        {
            ventaMayoristaBL = VentaMayoristaLogic;
            clienteBL = ClienteLogic;
            commonBL = CommonLogic;
            stockBL = StockLogic;
            productoxVentaBL = ProductoXVentaLogic;
            productoBL = ProductoLogic;
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
            ViewBag.TipoDeUnidadBlister = Constants.TIPODEUNIDAD_BLISTER;
            ViewBag.TipoDeUnidadMix = Constants.TIPODEUNIDAD_MIX;

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

        public ActionResult EditarVentaMayorista(int Id)
        {

            var vtaMayorista = ventaMayoristaBL.GetVentaMayoristaById(Id);
            //var productosXVentaMayorista = ventaMayoristaBL.GetProductosXVentaMayorista(Id);

            foreach (var producto in vtaMayorista.ProductosXVenta)
            {

                if(producto.Producto.Categoria != null)
                {
                    if (producto.Producto.EsBlister)
                        producto.Producto.Nombre = producto.Producto.Nombre + " (" + producto.Producto.Categoria.Nombre + ") - BLISTER - ";
                    else if(producto.Producto.EsMix)
                        producto.Producto.Nombre = producto.Producto.Nombre + " (" + producto.Producto.Categoria.Nombre + ") - MIX - ";
                    else
                        producto.Producto.Nombre = producto.Producto.Nombre + " (" + producto.Producto.Categoria.Nombre + ")";
                }
                else
                {
                    if (producto.Producto.EsBlister)
                        producto.Producto.Nombre = producto.Producto.Nombre + " (" + producto.Producto.Marca.Nombre + ") - BLISTER - ";
                    else if (producto.Producto.EsMix)
                        producto.Producto.Nombre = producto.Producto.Nombre + " (" + producto.Producto.Categoria.Nombre + ") - MIX - ";
                    else
                        producto.Producto.Nombre = producto.Producto.Nombre + " (" + producto.Producto.Categoria.Nombre + ")";
                }
            }

            ViewBag.TipoDeUnidadBlister = Constants.TIPODEUNIDAD_BLISTER;
            ViewBag.TipoDeUnidadMix = Constants.TIPODEUNIDAD_MIX;
            ViewBag.VentaMayoristaID = Id;

            if (vtaMayorista == null)
                return HttpNotFound();

            return View("VentaMayoristaFormEdit", vtaMayorista);
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
                double stockDisponible = 0;
                Producto prod = productoBL.GetProductoById(productoID);

                if (prod == null)
                    throw new Exception("El Producto no fue encontrado en el sistema.");


                //Consultamos Stock segun tipo de producto
                Stock productoSegunStock = new Stock();                
                if (prod.EsMix)
                {
                    //Calculamos el Stock en base a la cantidad 
                    var productosMixStock = stockBL.GetListaProductosMixById(productoID);
                    List<double> prodsDisponible = new List<double>();
                    int contador = 0;

                    foreach (var prodMix in productosMixStock)
                    {
                        productoSegunStock = stockBL.ValidarStockProducto(prodMix.ProductoDelMixId.GetValueOrDefault(), tipoUnidadID);

                        for (double i = prodMix.Cantidad; i < productoSegunStock.Cantidad; i += prodMix.Cantidad)
                        {
                            if (productoSegunStock.Cantidad >= prodMix.Cantidad)
                                contador++;
                        }

                        prodsDisponible.Add(contador);
                        contador = 0;

                    }

                    stockDisponible = prodsDisponible.Min();

                }
                else
                {
                    //Stock para el resto de los productos
                    productoSegunStock = stockBL.ValidarStockProducto(productoID, tipoUnidadID);

                    if (productoSegunStock == null)
                        throw new Exception("El Producto no tiene Stock Asociado para el Tipo de Unidad seleccionado. Revisar la carga del Stock en el sistema antes de continuar.");


                    stockDisponible = productoSegunStock.Cantidad;
                }


                //Calculamos los Precios
                ListaPrecio productoSegunLista = ventaMayoristaBL.CalcularImporteSegunCliente(clienteID, productoID, cantidad);

                switch (tipoUnidadID)
                {
                    case Constants.PRECIO_X_KG:

                        //Este caso aplica tanto para Productos comunes como para Productos MIX
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

                    case Constants.PRECIO_X_BLISTER:

                        Stock productoBlisterSegunStock = stockBL.ValidarStockProducto(productoID, tipoUnidadID);

                        if (productoBlisterSegunStock == null)
                            throw new Exception("El Producto no tiene Stock Asociado para el Tipo de Unidad seleccionado. Revisar la carga del Stock en el sistema antes de continuar.");

                        ListaPrecioBlister productoBlisterSegunLista = ventaMayoristaBL.CalcularImporteBlisterSegunCliente(productoID);

                        if (productoBlisterSegunLista == null)
                            throw new Exception("Error al cargar los precios de producto");

                        importe = Convert.ToDouble(productoBlisterSegunLista.Precio);

                        //Sumamos el importe total
                        importeTotal = importe * cantidad;

                        break;

                    default:
                        importe = 0;
                        break;

                }

                //Sumamos el importe total
                importeTotal = importe * cantidad;


                return Json(new { Success = true, Importe = importe, ImporteTotal = importeTotal, Counter = counter, Stock = stockDisponible }, JsonRequestBehavior.AllowGet);

                

                
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

        public ActionResult ValidarTipoDeProductoAsync(int productoID, int counter)
        {

            bool esBlister = false;
            bool esMix = false;
            Producto producto = ventaMayoristaBL.ValidateTipoDeProducto(productoID);

            if (producto.EsBlister)
                esBlister = true;
            else if (producto.EsMix)
                esMix = true;
                

            return Json(new { EsBlister = esBlister, EsMix = esMix, Counter = counter }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetSaldoClienteAsync(int clienteID)
        {

            try
            {
                
                Cliente cliente = clienteBL.GetClienteById(clienteID);                

                if (cliente == null)
                    throw new Exception("Cliente invalido al cargar Saldo Deudor");

                var saldo = cliente.Saldo;

                return Json(new { Saldo = saldo }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
           

        }

        public ActionResult CalcularStockBlisterAsync(int productoID, int tipoUnidadID, int counter)
        {
            try
            {
                Stock productoSegunStock = stockBL.ValidarStockProducto(productoID, tipoUnidadID);

                return Json(new { Success = true, Counter = counter, Stock = productoSegunStock.Cantidad }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            


        }

        //public ActionResult CalcularValorBlisterAsync(int clienteID, int productoID, int cantidad, int tipoUnidadID, int counter)
        //{

        //    try
        //    {
        //        double importe;
        //        double importeTotal;

        //        Stock productoSegunStock = stockBL.ValidarStockProducto(productoID, tipoUnidadID);

        //        if (productoSegunStock == null)
        //            throw new Exception("El Producto no tiene Stock Asociado para el Tipo de Unidad seleccionado. Revisar la carga del Stock en el sistema antes de continuar.");

        //        ListaPrecioBlister productoBlisterSegunLista = ventaMayoristaBL.CalcularImporteBlisterSegunCliente(productoID);

        //        if (productoBlisterSegunLista == null)
        //            throw new Exception("Error al cargar los precios de producto");
                
        //        importe = Convert.ToDouble(productoBlisterSegunLista.Precio);

        //        //Sumamos el importe total
        //        importeTotal = importe * cantidad;

        //        return Json(new { Success = true, Importe = importe, ImporteTotal = importeTotal, Counter = counter, Stock = productoSegunStock.Cantidad }, JsonRequestBehavior.AllowGet);


        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Success = false, Error = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }


        //}



        [AllowAnonymous]
        public ActionResult GenerarNotaPedido(int Id)
        {

            var venta = ventaMayoristaBL.GetVentaMayoristaById(Id);

            VentaMayoristaViewModel viewModel = new VentaMayoristaViewModel(venta)
            {
               // Clientes = clienteBL.GetClienteById(venta.ClienteID),
                //ProductoXVenta = productoxVentaBL.GetProductoXVentaByIdVenta(venta.ID)

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

            GenerarPdf pdf = new GenerarPdf(ventaMayoristaBL, productoxVentaBL);

            pdf.CrearPdf(Id);

            var venta = ventaMayoristaBL.GetAllVentaMayorista();

            if(venta == null)
                return HttpNotFound();

            return View(venta);



        }

    }
}