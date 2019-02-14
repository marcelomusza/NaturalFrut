using log4net;
using NaturalFrut.App_BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NaturalFrut.Controllers
{
    public class ValidationController : Controller
    {

        private readonly ClienteLogic clienteBL;
        private readonly ProveedorLogic proveedorBL;
        private readonly CommonLogic commonBL;
        private readonly ProductoLogic productoBL;
        private readonly VendedorLogic vendedorBL;
        private readonly ListaPreciosLogic listaPreciosBL;
        private readonly StockLogic stockBL;

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ValidationController(ClienteLogic ClienteLogic, CommonLogic CommonLogic, ProveedorLogic ProveedorLogic, ProductoLogic ProductoLogic, VendedorLogic VendedorLogic, ListaPreciosLogic ListaPreciosLogic, StockLogic StockLogic)
        {
            clienteBL = ClienteLogic;
            commonBL = CommonLogic;
            proveedorBL = ProveedorLogic;
            productoBL = ProductoLogic;
            vendedorBL = VendedorLogic;
            listaPreciosBL = ListaPreciosLogic;
            stockBL = StockLogic;
        }

        // GET: Validation
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult IsMarca_Available(string Nombre)
        {

            var marcas = commonBL.GetAllMarcas();

            var ocurrencia = marcas.Find(m => m.Nombre.ToLower().Equals(Nombre.ToLower()));

            if (ocurrencia == null)
                return Json(true, JsonRequestBehavior.AllowGet);

            log.Error("La marca: " + Nombre + " ya existe en la base de datos...");
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsCategoria_Available(string Nombre)
        {

            var categorias = commonBL.GetAllCategorias();

            var ocurrencia = categorias.Find(m => m.Nombre.ToLower().Equals(Nombre.ToLower()));

            if (ocurrencia == null)
                return Json(true, JsonRequestBehavior.AllowGet);

            log.Error("La categoria: " + Nombre + " ya existe en la base de datos...");
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsCliente_Available(string Nombre)
        {

            var clientes = clienteBL.GetAllClientes();

            var ocurrencia = clientes.Find(m => m.Nombre.ToLower().Equals(Nombre.ToLower()));

            if (ocurrencia == null)
                return Json(true, JsonRequestBehavior.AllowGet);

            log.Error("El Cliente: " + Nombre + " ya existe en la base de datos...");
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsProveedor_Available(string Nombre)
        {

            var proveedores = proveedorBL.GetAllProveedores();

            var ocurrencia = proveedores.Find(m => m.Nombre.ToLower().Equals(Nombre.ToLower()));

            if (ocurrencia == null)
                return Json(true, JsonRequestBehavior.AllowGet);

            log.Error("El Proveedor: " + Nombre + " ya existe en la base de datos...");
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsVendedor_Available(string Nombre)
        {

            var vendedores = vendedorBL.GetAllVendedores();

            var ocurrencia = vendedores.Find(m => m.Nombre.ToLower().Equals(Nombre.ToLower()));

            if (ocurrencia == null)
                return Json(true, JsonRequestBehavior.AllowGet);

            log.Error("El Vendedor: " + Nombre + " ya existe en la base de datos...");
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsProducto_Available(string Nombre)
        {

            var productos = productoBL.GetAllProducto();

            var ocurrencia = productos.Find(m => m.Nombre.ToLower().Equals(Nombre.ToLower()));

            if (ocurrencia == null)
                return Json(true, JsonRequestBehavior.AllowGet);

            log.Error("El Producto: " + Nombre + " ya existe en la base de datos...");
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsClasificacion_Available(string Nombre)
        {

            var clasificaciones = commonBL.GetAllClasificacion();

            var ocurrencia = clasificaciones.Find(m => m.Nombre.ToLower().Equals(Nombre.ToLower()));

            if (ocurrencia == null)
                return Json(true, JsonRequestBehavior.AllowGet);

            log.Error("La Clasificación: " + Nombre + " ya existe en la base de datos...");
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsProductoLista_Available(int ProductoId)
        {

            var listaPrecios = listaPreciosBL.GetAllListaPrecio();

            var ocurrencia = listaPrecios.Find(m => m.ProductoID.Equals(ProductoId));

            if (ocurrencia == null)
                return Json(true, JsonRequestBehavior.AllowGet);

            log.Error("El Producto: " + ocurrencia.Producto.Nombre + " ya existe en la base de datos...");
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsProductoBlisterLista_Available(int ProductoId)
        {

            var listaPreciosBlister = listaPreciosBL.GetAllListaPrecioBlister();

            var ocurrencia = listaPreciosBlister.Find(m => m.ProductoID.Equals(ProductoId));

            if (ocurrencia == null)
                return Json(true, JsonRequestBehavior.AllowGet);

            log.Error("El Producto: " + ocurrencia.Producto.Nombre + " ya existe en la base de datos...");
            return Json(false, JsonRequestBehavior.AllowGet);
        }
    }
}