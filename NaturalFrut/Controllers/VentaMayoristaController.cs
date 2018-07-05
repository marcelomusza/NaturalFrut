using NaturalFrut.App_BLL;
using NaturalFrut.App_BLL.ViewModels;
using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NaturalFrut.Controllers
{
    public class VentaMayoristaController : Controller
    {
        private readonly VentaMayoristaLogic ventaMayoristaBL;
        private readonly ClienteLogic clienteBL;

        public VentaMayoristaController(VentaMayoristaLogic VentaMayoristaLogic, ClienteLogic ClienteLogic)
        {
            ventaMayoristaBL = VentaMayoristaLogic;
            clienteBL = ClienteLogic;
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

            var clientes = ventaMayoristaBL.GetClienteList();
            var vendedores = ventaMayoristaBL.GetVendedorList();

            VentaMayoristaViewModel viewModel = new VentaMayoristaViewModel
            {
                Clientes = clientes,
                Vendedores = vendedores
                
            };

            return View("VentaMayoristaForm", viewModel);
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



    }
}