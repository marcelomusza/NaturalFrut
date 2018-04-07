using NaturalFrut.Models;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;

namespace NaturalFrut.Controllers
{
    public class AdminController : Controller
    {

        private ApplicationDbContext _context; 

        public AdminController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Clientes()
        {

            var clientes = _context.Clientes.ToList();

            return View(clientes);
        }

        public ActionResult Nuevo()
        {

            return View("ClienteForm");
        }

        public ActionResult Editar(int Id)
        {

            var cliente = _context.Clientes.SingleOrDefault(c => c.ID == Id);

            if (cliente == null)
                return HttpNotFound();

            return View("ClienteForm", cliente);
        }

        public ActionResult Borrar(int Id)
        {

            var cliente = _context.Clientes.SingleOrDefault(c => c.ID == Id);
             _context.Clientes.Remove(cliente);

            _context.SaveChanges();

            

            return RedirectToAction("Clientes", "Admin");
        }

        [HttpPost]
        public ActionResult Guardar(Cliente cliente)
        {
            
            try
            {
                if (cliente.ID == 0)
                {
                    //Agregamos nuevo Cliente
                    _context.Clientes.Add(cliente);
                }
                else
                {
                    //Edicion de Cliente Existente
                    var clienteEnBD = _context.Clientes.Single(c => c.ID == cliente.ID);

                    clienteEnBD.Nombre = cliente.Nombre;


                }

                _context.SaveChanges();
                
                return RedirectToAction("Clientes", "Admin");
            }
            catch (DbEntityValidationException ex)
            {
                //Manejamos error de base de datos para Insertar o Editar
                throw;
            }
            
                       
        }
    }
}