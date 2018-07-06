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

            return View("VentaMinoristaForm", viewModel);
        }

    }
}