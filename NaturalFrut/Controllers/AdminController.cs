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

        private ApplicationDbContext _context;

        private readonly ClienteLogic clienteBL;

        public AdminController(ClienteLogic ClienteLogic)
        {
            _context = new ApplicationDbContext();
            clienteBL = ClienteLogic;
        }

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Clientes()
        {
                        
            var clientes = clienteBL.GetAllClientes();

            return View(clientes);
        }

        public ActionResult Nuevo()
        {
            ClienteViewModel viewModel = new ClienteViewModel();

            return View("ClienteForm", viewModel);
        }

        public ActionResult Editar(int Id)
        {

            var cliente = clienteBL.GetClienteById(Id);

            ClienteViewModel viewModel = new ClienteViewModel(cliente);

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

                ClienteViewModel viewModel = new ClienteViewModel(cliente);

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
    }
}