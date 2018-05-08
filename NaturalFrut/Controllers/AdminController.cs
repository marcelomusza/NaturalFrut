using NaturalFrut.Models;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using NaturalFrut.App_BLL;
using NaturalFrut.App_BLL.Interfaces;
using NaturalFrut.App_BLL.ViewModels;

namespace NaturalFrut.Controllers
{
    public class AdminController : Controller
    {
        
        private readonly ClienteLogic clienteBL;
        private readonly CommonLogic commonBL;

        public AdminController(ClienteLogic ClienteLogic, CommonLogic CommonLogic)
        {            
            clienteBL = ClienteLogic;
            commonBL = CommonLogic;
        }

        public ActionResult Index()
        {
            return View();
        }

        #region Acciones Cliente
        public ActionResult Clientes()
        {

            var clientes = clienteBL.GetAllClientes();

            return View(clientes);
        }

        public ActionResult Nuevo()
        {

            var condicionIva = clienteBL.GetCondicionIvaList();
            var tipoCliente = clienteBL.GetTipoClienteList();

            ClienteViewModel viewModel = new ClienteViewModel
            {
                CondicionIVA = condicionIva,
                TipoCliente = tipoCliente
            };

            return View("ClienteForm", viewModel);
        }

        public ActionResult Editar(int Id)
        {

            var cliente = clienteBL.GetClienteById(Id);

            ClienteViewModel viewModel = new ClienteViewModel(cliente)
            {
                CondicionIVA = clienteBL.GetCondicionIvaList(),
                TipoCliente = clienteBL.GetTipoClienteList()
            };

            if (cliente == null)
                return HttpNotFound();

            return View("ClienteForm", viewModel);
        }

        public ActionResult Borrar(int Id)
        {
            var cliente = clienteBL.GetClienteById(Id);

            if (cliente != null)
                clienteBL.RemoveCliente(cliente);
            else
                return HttpNotFound();

            return RedirectToAction("Clientes", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guardar(Cliente cliente)
        {

            if (!ModelState.IsValid)
            {

                ClienteViewModel viewModel = new ClienteViewModel(cliente)
                {
                    CondicionIVA = clienteBL.GetCondicionIvaList(),
                    TipoCliente = clienteBL.GetTipoClienteList()
                };

                return View("ClienteForm", viewModel);
            }

            if (cliente.ID == 0)
            {
                //Agregamos nuevo Cliente
                clienteBL.AddCliente(cliente);
            }
            else
            {
                //Edicion de Cliente Existente
                clienteBL.UpdateCliente(cliente);
            }

            return RedirectToAction("Clientes", "Admin");

        }
        #endregion
        
        #region Acciones Condicion IVA
        public ActionResult CondicionIVA()
        {

            var condicionIVA = commonBL.GetAllCondicionIVA();

            return View(condicionIVA);
        }

        public ActionResult NuevoCondicionIVA()
        {

            CondicionIVA condicionIVA = new CondicionIVA();

            return View("CondicionIVAForm", condicionIVA);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarCondicionIVA(CondicionIVA condicionIVA)
        {

            if (!ModelState.IsValid)
            {
                return View("CondicionIVAForm");
            }

            if (condicionIVA.ID == 0)
            {
                //Agregamos nuevo CondicionIVA
                commonBL.AddCondicionIVA(condicionIVA);
            }
            else
            {
                //Edicion de Cliente Existente
                commonBL.UpdateCondicionIVA(condicionIVA);
            }

            return RedirectToAction("CondicionIVA", "Admin");

        }

        public ActionResult EditarCondicionIVA(int Id)
        {

            var condicionIVA = commonBL.GetCondicionIVAById(Id);


            if (condicionIVA == null)
                return HttpNotFound();

            return View("CondicionIVAForm", condicionIVA);
        }

        public ActionResult BorrarCondicionIVA(int Id)
        {
            var condicionIVA = commonBL.GetCondicionIVAById(Id);

            if (condicionIVA != null)
                commonBL.RemoveCondicionIVA(condicionIVA);
            else
                return HttpNotFound();

            return RedirectToAction("CondicionIVA", "Admin");
        }
        #endregion

        public ActionResult TipoCliente()
        {

            var tipoCliente = commonBL.GetAllTipoCliente();

            return View(tipoCliente);
        }

        public ActionResult NuevoTipoCliente()
        {

            TipoCliente tipoCliente = new TipoCliente();

            return View("TipoClienteForm", tipoCliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarTipoCliente(TipoCliente tipoCliente)
        {

            if (!ModelState.IsValid)
            {
                return View("TipoClienteForm");
            }

            if (tipoCliente.ID == 0)
            {
                //Agregamos nuevo TipoCliente
                commonBL.AddTipoCliente(tipoCliente);
            }
            else
            {
                //Edicion de Tipo Cliente existente
                commonBL.UpdateTipoCliente(tipoCliente);
            }

            return RedirectToAction("TipoCliente", "Admin");

        }

        public ActionResult EditarTipoCliente(int Id)
        {

            var tipoCliente = commonBL.GetTipoClienteById(Id);


            if (tipoCliente == null)
                return HttpNotFound();

            return View("TipoClienteForm", tipoCliente);
        }

        public ActionResult BorrarTipoCliente(int Id)
        {
            var tipoCliente = commonBL.GetTipoClienteById(Id);

            if (tipoCliente != null)
                commonBL.RemoveTipoCliente(tipoCliente);
            else
                return HttpNotFound();

            return RedirectToAction("TipoCliente", "Admin");
        }
    }
}