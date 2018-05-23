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
    [Authorize(Roles = RoleName.Administrator)]
    public class AdminController : Controller
    {
        
        private readonly ClienteLogic clienteBL;
        private readonly ProveedorLogic proveedorBL;
        private readonly CommonLogic commonBL;
        private readonly ProductoLogic productoBL;
        private readonly VendedorLogic vendedorBL;

        public AdminController(ClienteLogic ClienteLogic, CommonLogic CommonLogic, ProveedorLogic ProveedorLogic, ProductoLogic ProductoLogic, VendedorLogic VendedorLogic)
        {            
            clienteBL = ClienteLogic;
            commonBL = CommonLogic;
            proveedorBL = ProveedorLogic;
            productoBL = ProductoLogic;
            vendedorBL = VendedorLogic;
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

        public ActionResult NuevoCliente()
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

        public ActionResult EditarCliente(int Id)
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

        public ActionResult BorrarCliente(int Id)
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
        public ActionResult GuardarCliente(Cliente cliente)
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

        #region Acciones Tipo Cliente
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
        #endregion

        #region Acciones Proveedor
        public ActionResult Proveedores()
        {

            var proveedores = proveedorBL.GetAllProveedores();

            return View(proveedores);
        }

        public ActionResult NuevoProveedor()
        {

            var condicionIva = proveedorBL.GetCondicionIvaList();

            ProveedorViewModel viewModel = new ProveedorViewModel
            {
                CondicionIVA = condicionIva
            };

            return View("ProveedorForm", viewModel);
        }

        public ActionResult EditarProveedor(int Id)
        {

            var proveedor = proveedorBL.GetProveedorById(Id);

            ProveedorViewModel viewModel = new ProveedorViewModel(proveedor)
            {
                CondicionIVA = proveedorBL.GetCondicionIvaList()
            };

            if (proveedor == null)
                return HttpNotFound();

            return View("ProveedorForm", viewModel);
        }

        public ActionResult BorrarProveedor(int Id)
        {
            var proveedor = proveedorBL.GetProveedorById(Id);

            if (proveedor != null)
                proveedorBL.RemoveProveedor(proveedor);
            else
                return HttpNotFound();

            return RedirectToAction("Proveedores", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarProveedor(Proveedor proveedor)
        {

            if (!ModelState.IsValid)
            {

                ProveedorViewModel viewModel = new ProveedorViewModel(proveedor)
                {
                    CondicionIVA = proveedorBL.GetCondicionIvaList()
                };

                return View("ProveedorForm", viewModel);
            }

            if (proveedor.ID == 0)
            {
                //Agregamos nuevo Proveedor
                proveedorBL.AddProveedor(proveedor);
            }
            else
            {
                //Edicion de Proveedor Existente
                proveedorBL.UpdateProveedor(proveedor);
            }

            return RedirectToAction("Proveedores", "Admin");

        }
        #endregion

        #region Acciones Producto
        public ActionResult Productos()
        {

            var productos = productoBL.GetAllProducto();

            return View(productos);
        }

        public ActionResult NuevoProducto()
        {

            var categoria = productoBL.GetCategoriaList();

            ProductoViewModel viewModel = new ProductoViewModel
            {
                Categoria = categoria
            };


            Producto producto = new Producto();

            return View("ProductoForm", viewModel);
        }

        public ActionResult EditarProducto(int Id)
        {

            var producto = productoBL.GetProductoById(Id);

            ProductoViewModel viewModel = new ProductoViewModel(producto)
            {
                Categoria = productoBL.GetCategoriaList()
            };

            if (producto == null)
                return HttpNotFound();

            return View("ProductoForm", viewModel);
        }

        public ActionResult BorrarProducto(int Id)
        {
            var producto = productoBL.GetProductoById(Id);

            if (producto != null)
                productoBL.RemoveProducto(producto);
            else
                return HttpNotFound();

            return RedirectToAction("Productos", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarProducto(Producto producto)
        {

            if (!ModelState.IsValid)
            {

                ProductoViewModel viewModel = new ProductoViewModel(producto)
                {
                    Categoria = productoBL.GetCategoriaList()
                };

                return View("ProductoForm", viewModel);
            }

            if (producto.ID == 0)
            {
                //Agregamos nuevo Proveedor
                productoBL.AddProducto(producto);
            }
            else
            {
                //Edicion de Proveedor Existente
                productoBL.UpdateProducto(producto);
            }

            return RedirectToAction("Productos", "Admin");

        }
        #endregion

        #region Acciones Vendedor
        public ActionResult vendedores()
        {

            var vendedores = vendedorBL.GetAllProducto();

            return View(vendedores);
        }

        public ActionResult NuevoVendedor()
        {

            Vendedor vendedor = new Vendedor();

            return View("VendedorForm", vendedor);
        }

        public ActionResult EditarVendedor(int Id)
        {

            var vendedor = vendedorBL.GetVendedorById(Id);

            if (vendedor == null)
                return HttpNotFound();

            return View("VendedorForm", vendedor);
        }

        public ActionResult BorrarVendedor(int Id)
        {
            var vendedor = vendedorBL.GetVendedorById(Id);

            if (vendedor != null)
                vendedorBL.RemoveVendedor(vendedor);
            else
                return HttpNotFound();

            return RedirectToAction("Vendedores", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarVendedor(Vendedor vendedor)
        {

            if (!ModelState.IsValid)
            {

                return View("VendedorForm");
            }

            if (vendedor.ID == 0)
            {
                //Agregamos nuevo Proveedor
                vendedorBL.AddVendedor(vendedor);
            }
            else
            {
                //Edicion de Proveedor Existente
                vendedorBL.UpdateVendedor(vendedor);
            }

            return RedirectToAction("Vendedores", "Admin");

        }
        #endregion
    }
}