using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using NaturalFrut.App_BLL;

namespace NaturalFrut.Controllers
{

    [AllowAnonymous]
    public class HomeController : Controller
    {

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ProductoLogic productoBL;

        public HomeController(ProductoLogic ProductoLogic)
        {            
            productoBL = ProductoLogic;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            productoBL.UpdateProductoAuxiliar();


            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}