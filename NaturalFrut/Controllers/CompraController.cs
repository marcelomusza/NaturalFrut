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
using System.Data;
using log4net;

namespace NaturalFrut.Controllers
{
    [Authorize(Roles = "Administrator, User")]
    public class CompraController : Controller
    {
        private readonly CompraLogic compraBL;
        private readonly ProveedorLogic proveedorBL;
        private readonly ProductoLogic productoBL;
        private readonly CommonLogic commonBL;
        private readonly StockLogic stockBL;
        private readonly ProductoXCompraLogic productoxCompraBL;

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public CompraController(CompraLogic CompraLogic, 
            ProveedorLogic ProveedorLogic,
            CommonLogic CommonLogic,
            StockLogic StockLogic,
            ProductoLogic ProductoLogic,
            ProductoXCompraLogic ProductoXCompraLogic)
        {
            compraBL = CompraLogic;
            proveedorBL = ProveedorLogic;
            productoBL = ProductoLogic;
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

        public ActionResult IndexReporte()
        {

            return View();
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
            var serverTime = DateTime.UtcNow;
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Argentina Standard Time");
            var serverTimeConverted = TimeZoneInfo.ConvertTime(serverTime, timeZone);

            ViewBag.Fecha = serverTimeConverted;
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

        public ActionResult EditarCompra(int Id)
        {
            var compra = compraBL.GetCompraById(Id);
            ViewBag.CompraID = Id;

            if (compra == null)
            {
                log.Error("No se pudo encontrar compra con ID: " + Id);
                return View("Error");
            }
                

            return View("CompraFormEdit", compra);
        }


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

        public ActionResult NuevoProducto()
        {
            Producto model = new Producto();


            return PartialView(model);
        }

        public ActionResult GetMarcaAsync()
        {
            return Json(productoBL.GetMarcaList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCategoriaAsync()
        {
            return Json(productoBL.GetCategoriaList(), JsonRequestBehavior.AllowGet);
        }

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

        public ActionResult GetSaldoProveedorAsync(int proveedorID)
        {

            try
            {

                Proveedor proveedor = proveedorBL.GetProveedorById(proveedorID);

                if (proveedor == null)
                {
                    log.Error("Error al traer el Proveedor con ID: " + proveedorID);
                    throw new Exception("Proveedor invalido al cargar Saldo Deudor");
                }
                    

                var saldo = proveedor.Debe;

                return Json(new { Saldo = saldo }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                log.Error("Se ha producido una excepción al operar con Proveedor Async. Error: " + ex.Message);
                return Json(new { Success = false, Error = ex.Message }, JsonRequestBehavior.AllowGet);
            }


          }

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



        //[AllowAnonymous]
        //public ActionResult GenerarNotaPedido(int Id)
        //{

        //    var venta = ventaMayoristaBL.GetVentaMayoristaById(Id);

        //    VentaMayoristaViewModel viewModel = new VentaMayoristaViewModel(venta)
        //    {
        //        Clientes = clienteBL.GetClienteById(venta.ClienteID),
        //        ProductoXVenta = productoxVentaBL.GetProductoXVentaByIdVenta(venta.ID)

        //    };



        //    ViewBag.ProductoXVenta = productoxVentaBL.GetProductoXVentaByIdVenta(venta.ID);




        //    return View("NotaDePedidoForm", viewModel);
        //}


        public ActionResult GetReporteCompraAsync(DateTime fechaDesde, DateTime fechaHasta, string local)
        {

            try
            {

                var reporteCompra = compraBL.GetAllCompraSegunFechas(fechaDesde, fechaHasta, local);

                if (reporteCompra == null)
                {
                    log.Error("No se encontraron compras según el rango de fechas seleccionada.");
                    throw new Exception("No se encontraron Compras según el rango de fecha seleccionada");
                }
                    


                return Json(new { Success = true, ReporteCompra = reporteCompra }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                log.Error("Se ha producido una excepción al operar con Reporte Compras. Error: " +ex.Message);
                return Json(new { Success = false, Error = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult ReporteCompra()
        {

            return View("Reportes\\ReporteCompraEntreFechas");
        }

        [AllowAnonymous]
        public ActionResult GenerarReporteTxt(string compra)
        {


            GenerarTxt txt = new GenerarTxt(compraBL);

            txt.CrearTxtCompra(compra);

            return View("");

        }

        [AllowAnonymous]
        public ActionResult GenerarReporteExcel(string compra)
        {

            ExcelExportHelper excel = new ExcelExportHelper(compraBL);            

            DataTable tablaCompras = excel.ArmarExcel(compra, Constants.COMPRA_EXCEL);

            string[] columns = { "Id", "Proveedor", "CUIT", "IIBB", "Mes Año", "Tipo", "Suma Total", "Descuento %", "Descuento",
                                "Subtotal", "IVA", "Importe IVA", "IIBB BSAS", "IIBB CABA", "Percepción IVA", "Clasificación", "Total"};
            
            //byte[] filecontent = ExcelExportHelper.ExportExcel(technologies, "Technology", true, columns);
            byte[] filecontent = ExcelExportHelper.ExportExcel(tablaCompras, "Reporte Compras", true, columns);

            string fecha = string.Format("{0}{1}{2}", DateTime.Now.Date.Day, DateTime.Now.Date.Month, DateTime.Now.Date.Year);


            return File(filecontent, ExcelExportHelper.ExcelContentType, "ReporteCompra_" + fecha + ".xlsx");            

        }

        //public ActionResult ReporteProductosProveedor(int proveedorID)
        //{

        //    var proveedor = proveedorBL.GetProveedorById(proveedorID);

        //    if (proveedor == null)
        //        return HttpNotFound();

        //    ViewBag.ProveedorNombre = proveedor.Nombre;
        //    ViewBag.ProveedorID = proveedor.ID;

        //    return View("Reportes\\ReporteProductosPorProveedor");
        //}



        [AllowAnonymous]
        public ActionResult PrintAll(int Id)
        {


            GenerarPdfCompra pdf = new GenerarPdfCompra(compraBL, productoxCompraBL);

            pdf.CrearPdf(Id);

            var compra = compraBL.GetAllCompra();

            if (compra == null)
            {
                log.Error("Error al acceder a todas las compras.");
                return View("Error");
            }
                

            return View(compra);



        }

        public ActionResult ReporteComprasProveedor(int proveedorID)
        {

            var proveedor = proveedorBL.GetProveedorById(proveedorID);

            if (proveedor == null)
            {
                log.Error("Error al acceder al proveedor con ID: " + proveedorID);
                return View("Error");
            }
                

            ViewBag.ProveedorNombre = proveedor.Nombre;
            ViewBag.ProveedorID = proveedor.ID;

            return View("Reportes\\ReporteComprasProveedor");
        }

        public ActionResult ReporteProductosProveedor(int productoID)
        {

            var producto = productoBL.GetProductoById(productoID);

            if (producto == null)
            {
                log.Error("Error al acceder a Producto con ID: " + productoID);
                return View("Error");
            }
               
            
            ViewBag.ProductoNombre = producto.Nombre;
            ViewBag.ProductoID = producto.ID;
            
            return View("Reportes\\ReporteProductosProveedor");
        }

    }
}