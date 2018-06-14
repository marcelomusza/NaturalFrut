using NaturalFrut.App_BLL;
using NaturalFrut.App_BLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NaturalFrut.Controllers
{
    public class VentaController : Controller
    {

        private readonly ClienteLogic clienteBL;
        private readonly ProveedorLogic proveedorBL;
        private readonly CommonLogic commonBL;
        private readonly ProductoLogic productoBL;
        private readonly VendedorLogic vendedorBL;

        public VentaController(ClienteLogic ClienteLogic, CommonLogic CommonLogic, ProveedorLogic ProveedorLogic, ProductoLogic ProductoLogic, VendedorLogic VendedorLogic)
        {
            clienteBL = ClienteLogic;
            commonBL = CommonLogic;
            proveedorBL = ProveedorLogic;
            productoBL = ProductoLogic;
            vendedorBL = VendedorLogic;
        }

        // GET: Venta
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IngresarVenta()
        {

            var viewModel = new VentaViewModel();

            var productos = productoBL.GetAllProducto();
            var clientes = clienteBL.GetAllClientes();
            
            if(productos != null)
            {
              viewModel.Productos = productos;
            }

            if(clientes != null)
            {
              viewModel.Clientes = clientes;
            }         


            return View(viewModel);
        }


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

       public ActionResult GetCondicionIVAAsync()
       {
            return Json(clienteBL.GetCondicionIvaList(), JsonRequestBehavior.AllowGet);
       }

       public ActionResult GetTipoClienteAsync()
       {
           return Json(clienteBL.GetTipoClienteList(), JsonRequestBehavior.AllowGet);
       }

    }
}