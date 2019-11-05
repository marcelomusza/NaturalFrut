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
using NaturalFrut.Helpers;
using Rotativa;
using Rotativa.Options;
using System.Net.Mail;
using System.IO;
using System.Net;
using System.Configuration;
using log4net;

namespace NaturalFrut.Controllers
{
    [Authorize(Roles = RoleName.Administrator + "," + RoleName.User)]

    public class AdminController : Controller
    {

        private readonly ClienteLogic clienteBL;
        private readonly ProveedorLogic proveedorBL;
        private readonly CommonLogic commonBL;
        private readonly ProductoLogic productoBL;
        private readonly VendedorLogic vendedorBL;
        private readonly ListaPreciosLogic listaPreciosBL;
        private readonly StockLogic stockBL;

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AdminController(ClienteLogic ClienteLogic, CommonLogic CommonLogic, ProveedorLogic ProveedorLogic, ProductoLogic ProductoLogic, VendedorLogic VendedorLogic, ListaPreciosLogic ListaPreciosLogic, StockLogic StockLogic)
        {
            clienteBL = ClienteLogic;
            commonBL = CommonLogic;
            proveedorBL = ProveedorLogic;
            productoBL = ProductoLogic;
            vendedorBL = VendedorLogic;
            listaPreciosBL = ListaPreciosLogic;
            stockBL = StockLogic;
        }

        public ActionResult Index()
        {
            log.Info("Accediendo al panel administrativo...");
            return View();
        }

        #region Acciones Cliente
        public ActionResult Clientes()
        {
            log.Info("Accediendo a la lista de clientes...");

            var clientes = clienteBL.GetAllClientes();

            if(clientes == null)
            {
                log.Error("Error al traer lista de clientes.");
                return View("Error");
            }
                
            return View(clientes);
        }

        public ActionResult NuevoCliente()
        {
            log.Info("Accediendo a la creación de un nuevo cliente...");

            var condicionIva = clienteBL.GetCondicionIvaList();
            var tipoCliente = clienteBL.GetTipoClienteList();
            var lista = clienteBL.GetListaList();

            ClienteViewModel viewModel = new ClienteViewModel
            {
                CondicionIVA = condicionIva,
                TipoCliente = tipoCliente,
                Lista = lista
            };

            return View("ClienteForm", viewModel);
        }

        public ActionResult EditarCliente(int Id)
        {
            log.Info("Accediendo a la edición de un cliente...");

            var cliente = clienteBL.GetClienteById(Id);

            if (cliente == null)
            {
                log.Error("Se ha producido un error accediendo al cliente para su edición, cliente no encontrado...");
                return View("Error");
            }                

            ClienteViewModel viewModel = new ClienteViewModel(cliente)
            {
                CondicionIVA = clienteBL.GetCondicionIvaList(),
                TipoCliente = clienteBL.GetTipoClienteList(),
                Lista = clienteBL.GetListaList()
            };

            return View("ClienteForm", viewModel);
        }

        public ActionResult BorrarCliente(int Id)
        {
            var cliente = clienteBL.GetClienteById(Id);

            if (cliente != null)
                clienteBL.RemoveCliente(cliente);
            else
            {
                log.Error("Error al buscar cliente en la base de datos.");
                return View("Error");
            }
                

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
                    TipoCliente = clienteBL.GetTipoClienteList(),
                    Lista = clienteBL.GetListaList()
                };

                return View("ClienteForm", viewModel);
            }

            

            if (cliente.ID == 0)
            {
                //Agregamos nuevo Cliente
                log.Info("Salvando nuevo cliente en la base.");
                clienteBL.AddCliente(cliente);
            }
            else
            {
                //Edicion de Cliente Existente
                log.Info("Editando cliente existente en la base.");
                clienteBL.UpdateCliente(cliente);
            }

            return RedirectToAction("Clientes", "Admin");

        }
        #endregion

        #region Acciones Condicion IVA
        public ActionResult CondicionIVA()
        {

            var condicionIVA = commonBL.GetAllCondicionIVA();

            if (condicionIVA == null)
            {
                log.Error("Error al traer lista de condicion iva.");
                return View("Error");
            }


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
                log.Info("Guardando nueva Condición IVA");
                commonBL.AddCondicionIVA(condicionIVA);
            }
            else
            {
                //Edicion de Cliente Existente
                log.Info("Guardando Edición de Condición IVA");
                commonBL.UpdateCondicionIVA(condicionIVA);
            }

            return RedirectToAction("CondicionIVA", "Admin");

        }

        public ActionResult EditarCondicionIVA(int Id)
        {

            var condicionIVA = commonBL.GetCondicionIVAById(Id);

            if (condicionIVA == null)
            {
                log.Error("No se ha encontrado la condición IVA buscada");
                return View("Error");
            }                

            return View("CondicionIVAForm", condicionIVA);
        }

        public ActionResult BorrarCondicionIVA(int Id)
        {
            var condicionIVA = commonBL.GetCondicionIVAById(Id);

            if (condicionIVA != null)
                commonBL.RemoveCondicionIVA(condicionIVA);
            else
            {
                log.Error("No se ha encontrado la condición IVA buscada");
                return View("Error");
            }

            return RedirectToAction("CondicionIVA", "Admin");
        }
        #endregion

        #region Acciones Tipo Cliente
        public ActionResult TipoCliente()
        {

            var tipoCliente = commonBL.GetAllTipoCliente();

            if (tipoCliente == null)
            {
                log.Error("Error al traer lista de tipo Cliente.");
                return View("Error");
            }

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
                log.Info("Salvando nuevo Tipo Cliente");
                commonBL.AddTipoCliente(tipoCliente);
            }
            else
            {
                //Edicion de Tipo Cliente existente
                log.Info("Salvando nuevo Tipo Cliente");
                commonBL.UpdateTipoCliente(tipoCliente);
            }

            return RedirectToAction("TipoCliente", "Admin");

        }

        public ActionResult EditarTipoCliente(int Id)
        {

            var tipoCliente = commonBL.GetTipoClienteById(Id);


            if (tipoCliente == null)
            {
                log.Error("Error al traer lista de tipo Cliente.");
                return View("Error");
            }

            return View("TipoClienteForm", tipoCliente);
        }

        public ActionResult BorrarTipoCliente(int Id)
        {
            var tipoCliente = commonBL.GetTipoClienteById(Id);

            if (tipoCliente != null)
                commonBL.RemoveTipoCliente(tipoCliente);
            else
            {               
                log.Error("Error al traer tipo Cliente.");
                return View("Error");                
            }               

            return RedirectToAction("TipoCliente", "Admin");
        }
        #endregion

        #region Acciones Proveedor
        public ActionResult Proveedores()
        {

            var proveedores = proveedorBL.GetAllProveedores();

            if (proveedores == null)
            {
                log.Error("Error al traer lista de tipo Cliente.");
                return View("Error");
            }

            return View(proveedores);
        }

        public ActionResult NuevoProveedor()
        {
            ProveedorViewModel viewModel = new ProveedorViewModel();

            return View("ProveedorForm", viewModel);
        }

        public ActionResult EditarProveedor(int Id)
        {

            var proveedor = proveedorBL.GetProveedorById(Id);

            if (proveedor == null)
            {
                log.Error("Proveedor no encontrado...");
                return View("Error");
            }

            ProveedorViewModel viewModel = new ProveedorViewModel(proveedor);

            return View("ProveedorForm", viewModel);
        }

        public ActionResult BorrarProveedor(int Id)
        {
            var proveedor = proveedorBL.GetProveedorById(Id);

            if (proveedor != null)
                proveedorBL.RemoveProveedor(proveedor);
            else
            {
                log.Error("Proveedor no encontrado en la base.");
                return View("Error");
            }

            return RedirectToAction("Proveedores", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarProveedor(Proveedor proveedor)
        {

            if (!ModelState.IsValid)
            {

                ProveedorViewModel viewModel = new ProveedorViewModel(proveedor);


                return View("ProveedorForm", viewModel);
            }

            if (proveedor.ID == 0)
            {
                //Agregamos nuevo Proveedor
                log.Info("Guardando nuevo Proveedor.");
                proveedorBL.AddProveedor(proveedor);
            }
            else
            {
                //Edicion de Proveedor Existente
                log.Info("Editando Proveedor existente.");
                proveedorBL.UpdateProveedor(proveedor);
            }

            return RedirectToAction("Proveedores", "Admin");

        }
        #endregion

        #region Acciones Producto
        public ActionResult Productos()
        {

            var productos = productoBL.GetAllProducto();

            if (productos == null)
            {
                log.Error("Error al acceder a la lista de productos...");
                return View("Error");
            }

            return View(productos);
        }

        public ActionResult NuevoProducto()
        {

            var categoria = productoBL.GetCategoriaList();
            var marca = productoBL.GetMarcaList();

            ProductoViewModel viewModel = new ProductoViewModel
            {
                Categoria = categoria,
                Marca = marca
            };

            return View("ProductoForm", viewModel);
        }

        public ActionResult EditarProducto(int Id)
        {

            var producto = productoBL.GetProductoById(Id);

            if (producto == null)
            {
                log.Error("Error al acceder al producto seleccionado...");
                return View("Error");
            }

            ProductoViewModel viewModel = new ProductoViewModel(producto)
            {
                Categoria = productoBL.GetCategoriaList(),
                Marca = productoBL.GetMarcaList()
            };

            return View("ProductoForm", viewModel);
        }

        public ActionResult BorrarProducto(int Id)
        {
            var producto = productoBL.GetProductoById(Id);

            if (producto != null)
            {
                log.Info("Accediendo al proceso para borrar Producto: " + producto.Nombre);

                if (producto.EsMix)
                {
                    log.Info("Producto Mix: " + producto.Nombre + ". Se procederá a borrar sus productos asociados...");
                    var listaProductosDelMix = productoBL.GetListaProductosMixById(producto.ID);                    

                    if (listaProductosDelMix != null)
                    {
                        foreach (var productoMix in listaProductosDelMix)
                        {
                            log.Info("Producto del mix a borrar: " + productoMix.ProductoDelMix.Nombre);
                            productoBL.RemoveProductoMix(productoMix);
                        }
                    }

                    //Una vez borrados los productos relacionados del mix, borramos el producto principal
                    productoBL.RemoveProducto(producto);

                }

                else
                    productoBL.RemoveProducto(producto);
            }
            else
            {
                log.Error("Error al acceder al producto con ID: " + Id);
                return View("Error");
            }


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
                    Categoria = productoBL.GetCategoriaList(),
                    Marca = productoBL.GetMarcaList()
                };

                return View("ProductoForm", viewModel);
            }

            if (producto.ID == 0)
            {
                //Agregamos nuevo Proveedor
                log.Info("Guardando nuevo producto: " + producto.Nombre);
                productoBL.AddProducto(producto);
            }
            else
            {
                //Edicion de Proveedor Existente
                log.Info("Editando Producto existente: " + producto.Nombre);
                productoBL.UpdateProducto(producto);
            }

            return RedirectToAction("Productos", "Admin");

        }
        #endregion
        
        #region Acciones Producto Mix
        public ActionResult ProductosMix()
        {

            var productosMix = productoBL.GetAllProductoMix();

            if (productosMix == null)
            {
                log.Error("Error al acceder a la lista de productos...");
                return View("Error");
            }

            return View(productosMix);
        }

        public ActionResult NuevoProductoMix()
        {

            ProductoMix productoMix = new ProductoMix();

            return View("ProductoMixForm", productoMix);
        }

        public ActionResult EditarProductoMix(int Id)
        {

            var prodMix = productoBL.GetProductoById(Id);

            if (prodMix == null)
            {
                log.Error("Error al acceder al producto Mix seleccionado con ID: " + Id);
                return View("Error");
            }

            var productosMix = productoBL.GetListaProductosMixById(Id);

            if (productosMix == null)
            {
                log.Error("Error al acceder a la lista de Productos Mix asociados bajo el ID: " + Id);
                return View("Error");
            }

            ViewBag.ProdMixID = prodMix.ID;
            ViewBag.ProdMixNombre = prodMix.Nombre;

            return View("ProductoMixFormEdit", productosMix);
        }

        public ActionResult VerProductoMix(int Id)
        {

            var prodMix = productoBL.GetProductoById(Id);

            if (prodMix == null)
            {
                log.Error("Error al acceder al producto Mix seleccionado con ID: " + Id);
                return View("Error");
            }

            var productosMix = productoBL.GetListaProductosMixById(Id);

            if (productosMix == null)
            {
                log.Error("Error al acceder a la lista de Productos Mix asociados bajo el ID: " + Id);
                return View("Error");
            }

            ViewBag.ProdMixID = prodMix.ID;
            ViewBag.ProdMixNombre = prodMix.Nombre;

            return View("ProductoMixFormView", productosMix);
        }

        //public ActionResult BorrarProductoMix(int Id)
        //{
        //    var productoMix = productoBL.GetProductoMixById(Id);

        //    if (productoMix != null)
        //        productoBL.RemoveProductoMix(productoMix);
        //    else
        //        return HttpNotFound();

        //    return RedirectToAction("ProductosMix", "Admin");
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarProductoMix(ProductoMix productoMix)
        {

            if (!ModelState.IsValid)
            {
                return View("ProductoMixForm");
            }

            if (productoMix.ID == 0)
            {
                //Agregamos nuevo Prod mix
                log.Info("Guardando nuevo Producto Mix: " + productoMix.ProdMix.Nombre);
                productoBL.AddProductoMix(productoMix);
            }
            else
            {
                //Edicion de prod mix Existente
                log.Info("Editando Producto Mix existente: " + productoMix.ProdMix.Nombre + ". ID: " + productoMix.ID);
                productoBL.UpdateProductoMix(productoMix);
            }

            return RedirectToAction("ProductosMix", "Admin");

        } 
        #endregion
        
        #region Acciones Vendedor
        public ActionResult vendedores()
        {

            var vendedores = vendedorBL.GetAllVendedores();

            if (vendedores == null)
            {
                log.Error("Error al traer lista de vendedores.");
                return View("Error");
            }

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
            {
                log.Error("Error al traer vendedor con ID: " + Id);
                return View("Error");
            }

            return View("VendedorForm", vendedor);
        }

        public ActionResult BorrarVendedor(int Id)
        {
            var vendedor = vendedorBL.GetVendedorById(Id);

            if (vendedor != null)
            {
                log.Info("Borrando Vendedor: " + vendedor.Nombre);
                vendedorBL.RemoveVendedor(vendedor);
            }                
            else
            {
                log.Error("Vendedor no encontrado con ID: " + Id);
                return View("Error");
            }

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
                //Agregamos nuevo Vendedor
                log.Info("Guardando nuevo Vendedor: " + vendedor.Nombre);
                vendedorBL.AddVendedor(vendedor);
            }
            else
            {
                //Edicion de Proveedor Existente
                log.Info("Editando Vendedor Existente: " + vendedor.Nombre + ", bajo ID: " + vendedor.ID);
                vendedorBL.UpdateVendedor(vendedor);
            }

            return RedirectToAction("Vendedores", "Admin");

        }
        #endregion

        #region Acciones Categoria
        public ActionResult Categorias()
        {

            var categorias = commonBL.GetAllCategorias();

            if (categorias == null)
            {
                log.Error("Error al traer lista de Categorias");
                return View("Error");
            }

            return View(categorias);
        }

        public ActionResult NuevoCategoria()
        {

            Categoria categoria = new Categoria();

            return View("CategoriaForm", categoria);
        }

        public ActionResult EditarCategoria(int Id)
        {

            var categoria = commonBL.GetCategoriaById(Id);

            if (categoria == null)
            {
                log.Error("Error al traer Categoria con ID: " + Id);
                return View("Error");
            }

            return View("CategoriaForm", categoria);
        }

        public ActionResult BorrarCategoria(int Id)
        {

            var categoria = commonBL.GetCategoriaById(Id);

            if (categoria != null)
            {
                log.Info("Borrando Categoria: " + categoria.Nombre + ", ID: " + Id);
                commonBL.RemoveCategoria(categoria);
            }                
            else
            {
                log.Error("Error al Borrar Categoria con ID: " + Id);
                return View("Error");
            }

            return RedirectToAction("Categorias", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarCategoria(Categoria categoria)
        {

            if (!ModelState.IsValid)
            {
                return View("CategoriaForm");
            }

            if (categoria.ID == 0)
            {
                //Agregamos nuevo TipoCliente
                log.Info("Guardando nueva categoria: " + categoria.Nombre);
                commonBL.AddCategoria(categoria);
            }
            else
            {
                //Edicion de Tipo Cliente existente
                log.Info("Editando Categoria Existente: " + categoria.Nombre + ", ID: " + categoria.ID);
                commonBL.UpdateCategoria(categoria);
            }

            return RedirectToAction("Categorias", "Admin");

        }
        #endregion

        #region Acciones Marca
        public ActionResult Marcas()
        {
            var marcas = commonBL.GetAllMarcas();

            if (marcas == null)
            {
                log.Error("Error al traer lista de Marcas");
                return View("Error");
            }

            return View(marcas);
        }

        public ActionResult NuevoMarca()
        {

            Marca marca = new Marca();

            return View("MarcaForm", marca);
        }

        public ActionResult EditarMarca(int Id)
        {

            var marca = commonBL.GetMarcaById(Id);

            if (marca == null)
            {
                log.Error("Error al traer Marca con ID: " + Id);
                return View("Error");
            }

            return View("MarcaForm", marca);
        }

        public ActionResult BorrarMarca(int Id)
        {

            var marca = commonBL.GetMarcaById(Id);

            if (marca != null)
            {
                log.Info("Borrando Marca: " + marca.Nombre + ", ID: " + Id);
                commonBL.RemoveMarca(marca);
            }                
            else
            {
                log.Error("Error al borrar Marca con ID: " + Id);
                return View("Error");
            }

            return RedirectToAction("Marcas", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarMarca(Marca marca)
        {

            if (!ModelState.IsValid)
            {
                return View("MarcaForm");
            }

            if (marca.ID == 0)
            {
                //Agregamos nuevo TipoCliente
                log.Info("Guarando Marca nueva: " + marca.Nombre);
                commonBL.AddMarca(marca);
            }
            else
            {
                //Edicion de Tipo Cliente existente
                log.Info("Editando Marca existente: " + marca.Nombre + ", ID: " + marca.ID);
                commonBL.UpdateMarca(marca);
            }

            return RedirectToAction("Marcas", "Admin");

        }
        #endregion

        #region Acciones Tipo de Unidad
        public ActionResult TiposDeUnidad()
        {

            var tiposDeUnidad = commonBL.GetAllTiposDeUnidad();

            if (tiposDeUnidad == null)
            {
                log.Error("Error al traer Tipos de Unidad.");
                return View("Error");
            }

            return View(tiposDeUnidad);
        }

        public ActionResult NuevoTipoDeUnidad()
        {
            TipoDeUnidad tipoDeUnidad = new TipoDeUnidad();

            return View("TipoDeUnidadForm", tipoDeUnidad);
        }

        public ActionResult EditarTipoDeUnidad(int Id)
        {

            var tipoDeUnidad = commonBL.GetTipoDeUnidadById(Id);

            if (tipoDeUnidad == null)
            {
                log.Error("Error al traer tipo de unidad con iD: " + Id);
                return View("Error");
            }


            if (tipoDeUnidad == null)
                return HttpNotFound();

            return View("TipoDeUnidadForm", tipoDeUnidad);
        }

        public ActionResult BorrarTipoDeUnidad(int Id)
        {
            var tipoDeUnidad = commonBL.GetTipoDeUnidadById(Id);

            if (tipoDeUnidad != null)
            {
                log.Info("Borrando Tipo de Unidad: " + tipoDeUnidad.Nombre + ", ID: " + Id);
                commonBL.RemoveTipoDeUnidad(tipoDeUnidad);
            }               
            else
            {
                log.Error("Error al borrar tipo de unidad con iD: " + Id);
                return View("Error");
            }

            return RedirectToAction("TiposDeUnidad", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarTipoDeUnidad(TipoDeUnidad tipoDeUnidad)
        {

            if (!ModelState.IsValid)
            {
                return View("TipoDeUnidadForm");
            }

            if (tipoDeUnidad.ID == 0)
            {
                //Agregamos nuevo TipoCliente
                log.Info("Guardando nuevo tipo de unidad: " + tipoDeUnidad.Nombre);
                commonBL.AddTipoDeUnidad(tipoDeUnidad);
            }
            else
            {
                //Edicion de Tipo Cliente existente
                log.Info("Editando tipo de unidad: " + tipoDeUnidad.Nombre + ", ID: " + tipoDeUnidad.ID);
                commonBL.UpdateTipoDeUnidad(tipoDeUnidad);
            }

            return RedirectToAction("TiposDeUnidad", "Admin");

        }
        #endregion

        #region Acciones Lista
        public ActionResult Lista()
        {
            var lista = listaPreciosBL.GetAllLista();

            if (lista == null)
            {
                log.Error("Error al traer Lista.");
                return View("Error");
            }

            return View(lista);
        }

        public ActionResult NuevoLista()
        {
            Lista lista = new Lista();

            return View("ListaForm", lista);
        }

        public ActionResult EditarLista(int Id)
        {

            var lista = listaPreciosBL.GetListaById(Id);

            if (lista == null)
            {
                log.Error("Error al traer Lista con iD: " + Id);
                return View("Error");
            }

            return View("ListaForm", lista);
        }

        public ActionResult BorrarLista(int Id)
        {

            var lista = listaPreciosBL.GetListaById(Id);

            if (lista != null)
            {
                log.Info("Borrando Lista: " + lista.Nombre + ", ID: " + Id);
                listaPreciosBL.RemoveLista(lista);
            }                
            else
            {
                log.Error("Error al traer Lista con iD: " + Id);
                return View("Error");
            }

            return RedirectToAction("Lista", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarLista(Lista lista)
        {

            if (!ModelState.IsValid)
            {
                return View("ListaForm");
            }

            if (lista.ID == 0)
            {
                //Agregamos nuevo Lista de Precios
                log.Info("Guardando nueva Lista: " + lista.Nombre);
                listaPreciosBL.AddLista(lista);
            }
            else
            {
                //Edicion de Cliente Existente
                log.Info("Editando Lista: " + lista.Nombre + ", ID: " + lista.ID);
                listaPreciosBL.UpdateLista(lista);
            }

            return RedirectToAction("Lista", "Admin");



        }

        public ActionResult ActualizarListaPrecios(int id)
        {

            //var listaPrecios = listaPreciosBL.GetAllLista();

            //if (listaPrecios == null)
            //{
            //    log.Error("Error al traer Lista de Precios.");
            //    return View("Error");
            //}

            listaPreciosBL.ActualizarListaPrecios(id);

            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Acciones Lista Precios
        public ActionResult ListaPrecios()
        {
            var listaPrecio = listaPreciosBL.GetAllListaPrecio();
            ViewBag.ListaPrincipalID = Constants.LISTAPRINCIPAL;

            if (listaPrecio == null)
            {
                log.Error("Error al traer Lista Precios.");
                return View("Error");
            }

            return View(listaPrecio);
        }

        public ActionResult NuevoListaPrecios()
        {
            //var lista = listaPreciosBL.GetListaList();
            var cliente = listaPreciosBL.GetClienteList();
            var producto = listaPreciosBL.GetProductoList();
            //var tipoDeUnidad = listaPreciosBL.GetTipoDeUnidadList();

            ListaPreciosViewModel viewModel = new ListaPreciosViewModel
            {
                //Lista = lista,
                Producto = producto
            };

            return View("ListaPreciosForm", viewModel);
        }

        public ActionResult EditarListaPrecios(int Id)
        {

            var listaPrecio = listaPreciosBL.GetListaPrecioById(Id);

            if (listaPrecio == null)
            {
                log.Error("Error al traer Lista Precio con iD: " + Id);
                return View("Error");
            }

            ListaPreciosViewModel viewModel = new ListaPreciosViewModel(listaPrecio)
            {
                Lista = listaPreciosBL.GetListaList(),
                Producto = listaPreciosBL.GetProductoList()
            };            

            return View("ListaPreciosForm", viewModel);
        }

        public ActionResult BorrarListaPrecios(int Id)
        {

            var listaPrecio = listaPreciosBL.GetListaPrecioById(Id);

            if (listaPrecio != null)
            {
                log.Info("Borrando Lista Precios con ID: " + listaPrecio.ID);
                listaPreciosBL.RemoveListaPrecio(listaPrecio);
            }               
            else
            {
                log.Error("Error al traer Lista Precio con ID: " + Id);
                return View("Error");
            }

            return RedirectToAction("ListaPrecios", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarListaPrecios(ListaPrecio listaPrecio)
        {

            if (!ModelState.IsValid)
            {

                ListaPreciosViewModel viewModel = new ListaPreciosViewModel(listaPrecio)
                {
                    //Lista = listaPreciosBL.GetListaList(),
                    Producto = listaPreciosBL.GetProductoList()
                };

                return View("ListaPreciosForm", viewModel);
            }

            if (listaPrecio.ID == 0)
            {
                //Agregamos nuevo Lista de Precios
                log.Info("Guardando nueva Lista de Precios para Producto con ID: " + listaPrecio.ID);
                listaPreciosBL.AddListaPrecio(listaPrecio);
            }
            else
            {
                //Edicion de Cliente Existente
                log.Info("Editando Lista de Precios del Producto con ID: " + listaPrecio.ID);
                listaPreciosBL.UpdateListaPrecio(listaPrecio);
            }

            return RedirectToAction("ListaPrecios", "Admin");

        }


        #endregion

        #region Acciones Lista Precios Blister
        public ActionResult ListaPreciosBlister()
        {
            var listaPrecioBlister = listaPreciosBL.GetAllListaPrecioBlister();

            if (listaPrecioBlister == null)
            {
                log.Error("Error al traer Lista Precios Blister.");
                return View("Error");
            }

            return View(listaPrecioBlister);
        }

        public ActionResult NuevoListaPreciosBlister()
        {

            var producto = listaPreciosBL.GetProductoBlisterList();

            ListaPreciosBlisterViewModel viewModel = new ListaPreciosBlisterViewModel
            {                
                Producto = producto
            };

            return View("ListaPreciosBlisterForm", viewModel);
        }

        public ActionResult EditarListaPreciosBlister(int Id)
        {

            var listaPrecioBlister = listaPreciosBL.GetListaPrecioBlisterById(Id);

            if (listaPrecioBlister == null)
            {
                log.Error("Error al traer Lista Precio Blister con ID: " + Id);
                return View("Error");
            }

            ListaPreciosBlisterViewModel viewModel = new ListaPreciosBlisterViewModel(listaPrecioBlister)
            {
                Producto = listaPreciosBL.GetProductoBlisterList()
            };
            
            return View("ListaPreciosBlisterForm", viewModel);
        }

        public ActionResult BorrarListaPreciosBlister(int Id)
        {

            var listaPrecioBlister = listaPreciosBL.GetListaPrecioBlisterById(Id);

            if (listaPrecioBlister != null)
            {
                log.Info("Borrando lista de precios Blister con ID: " + listaPrecioBlister.ID);
                listaPreciosBL.RemoveListaPrecioBlister(listaPrecioBlister);
            }                
            else
            {
                log.Error("Error al traer Lista Precio Blister con ID: " + Id);
                return View("Error");
            }

            return RedirectToAction("ListaPreciosBlister", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarListaPreciosBlister(ListaPrecioBlister listaPrecioBlister)
        {

            if (!ModelState.IsValid)
            {

                ListaPreciosBlisterViewModel viewModel = new ListaPreciosBlisterViewModel(listaPrecioBlister)
                {
                    Producto = listaPreciosBL.GetProductoList()
                };

                return View("ListaPreciosBlisterForm");
            }

            if (listaPrecioBlister.ID == 0)
            {
                //Agregamos nuevo Lista de Precios
                log.Info("Guardando nueva Lista de Precios Blister" );
                listaPreciosBL.AddListaPrecioBlister(listaPrecioBlister);
            }
            else
            {
                //Edicion de Cliente Existente
                log.Info("Editando Lista de Precios Blister con ID: " + listaPrecioBlister.ID);
                listaPreciosBL.UpdateListaPrecioBlister(listaPrecioBlister);
            }

            return RedirectToAction("ListaPreciosBlister", "Admin");

        }

        #endregion               

        #region Acciones Clasificacion
        public ActionResult Clasificacion()
        {
            var clasificacion = commonBL.GetAllClasificacion();

            if (clasificacion == null)
            {
                log.Error("Error al traer Clasificacion.");
                return View("Error");
            }

            return View(clasificacion);
        }

        public ActionResult NuevaClasificacion()
        {

            Clasificacion clasificacion = new Clasificacion();

            return View("ClasificacionForm", clasificacion);
        }

        public ActionResult EditarClasificacion(int Id)
        {

            var clasificacion = commonBL.GetClasificacionById(Id);

            if (clasificacion == null)
            {
                log.Error("Error al traer Clasificacion con ID: " + Id);
                return View("Error");
            }

            return View("ClasificacionForm", clasificacion);
        }

        public ActionResult BorrarClasificacion(int Id)
        {

            var clasificacion = commonBL.GetClasificacionById(Id);

            if (clasificacion != null)
            {
                log.Info("Borrando Clasificacion con ID: " + clasificacion.ID + ", Nombre: " + clasificacion.Nombre);
                commonBL.RemoveClasificacion(clasificacion);
            }            
            else
            {
                log.Error("Error al traer Clasificacion con ID: " + Id);
                return View("Error");
            }

            return RedirectToAction("Clasificacion", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarClasificacion(Clasificacion clasificacion)
        {

            if (!ModelState.IsValid)
            {
                return View("ClasificacionForm");
            }

            if (clasificacion.ID == 0)
            {
                log.Info("Guardando nueva Clasificacion: " + clasificacion.Nombre);
                commonBL.AddClasificacion(clasificacion);
            }
            else
            {
                log.Info("Editando Clasificacion existente: " + clasificacion.Nombre);
                commonBL.UpdateClasificacion(clasificacion);
            }

            return RedirectToAction("Clasificacion", "Admin");

        }
        #endregion

        #region Acciones Descarga/Exportar Lista de Precios

        public ActionResult ExportarListaPrecios()
        {

            var listaPrecios = listaPreciosBL.GetAllLista();

            if (listaPrecios == null)
            {
                log.Error("Error al traer Lista de Precios.");
                return View("Error");
            }

            ViewBag.ListaPrecios = listaPrecios;

            return View("ListaPreciosExportIndex", new Lista());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenerarListaPreciosExport(Lista lista)
        {

            //Traemos la lista de precios seleccionada
            var lis = listaPreciosBL.GetListaById(lista.ID);
            var listaDePrecios = listaPreciosBL.GetListaPrecioExportByListaId(lista.ID);
            //var categorias = commonBL.GetAllCategorias();
            //var marcas = commonBL.GetAllMarcas();

            if (lis == null)
            {
                log.Error("Error al traer Lista de Precios.");
                return View("Error");
            }

            if (listaDePrecios == null)
            {
                log.Error("Error al traer Lista de Precios para exportar.");
                return View("Error");
            }

            List<Marca> marca = new List<Marca>();
            List<Categoria> categoria = new List<Categoria>();

            //Filtramos en base a las marcas y categorias que contengan productos Blister
            foreach (var item in listaDePrecios)
            {
                if (item.Producto.Categoria != null)
                {
                    var categoExistente = categoria.Find(c => c.Nombre == item.Producto.Categoria.Nombre);
                    if (categoExistente == null)
                        categoria.Add(item.Producto.Categoria);
                }
                    
                else if (item.Producto.Marca != null)
                {
                    var marcaExistente = marca.Find(c => c.Nombre == item.Producto.Marca.Nombre);
                    if (marcaExistente == null)
                        marca.Add(item.Producto.Marca);
                }
                   
            }

            ListaPreciosExportViewModel viewModel = new ListaPreciosExportViewModel()
            {
                ListaPrecios = listaDePrecios,
                Categorias = categoria,
                Marcas = marca,
                ListaId = lista.ID,
                Nombre = lis.Nombre
            };

            return View("ListaPreciosExport", viewModel);

        }

        public ActionResult DescargarListaPreciosPDF(int listaid)
        {

            //Traemos la lista de precios seleccionada
            var lis = listaPreciosBL.GetListaById(listaid);
            var listaDePrecios = listaPreciosBL.GetListaPrecioExportByListaId(listaid);
            //var categorias = commonBL.GetAllCategorias();
            //var marcas = commonBL.GetAllMarcas();

            if (lis == null)
            {
                log.Error("Error al traer Lista de Precios.");
                return View("Error");
            }

            if (listaDePrecios == null)
            {
                log.Error("Error al traer Lista de Precios para exportar.");
                return View("Error");
            }

            List<Marca> marca = new List<Marca>();
            List<Categoria> categoria = new List<Categoria>();

            //Filtramos en base a las marcas y categorias que contengan productos Blister
            foreach (var item in listaDePrecios)
            {
                if (item.Producto.Categoria != null)
                {
                    var categoExistente = categoria.Find(c => c.Nombre == item.Producto.Categoria.Nombre);
                    if (categoExistente == null)
                        categoria.Add(item.Producto.Categoria);
                }

                else if (item.Producto.Marca != null)
                {
                    var marcaExistente = marca.Find(c => c.Nombre == item.Producto.Marca.Nombre);
                    if (marcaExistente == null)
                        marca.Add(item.Producto.Marca);
                }
            }

            ListaPreciosExportViewModel viewModel = new ListaPreciosExportViewModel()
            {
                ListaPrecios = listaDePrecios,
                Categorias = categoria,
                Marcas = marca,
                ListaId = listaid,
                Nombre = lis.Nombre
            };

            var pdf = new ViewAsPdf("Export\\ListaPreciosPDF", viewModel)
            {                
                PageSize = Size.A4,
                PageOrientation = Orientation.Portrait,
                PageMargins = new Margins(15, 15, 15, 15),
            };

            return pdf;
        }

        public ActionResult MailListaPreciosForm(int listaid)
        {

            ViewBag.ListaID = listaid;
            ViewBag.Message = "nuevomail";

            return View("Mail\\EnviarMailListaPreciosForm");

        }

        public ActionResult EnviarMailListaPrecios(MailModel datosMail, int listaid)
        {

            MailModel datos = datosMail;
            string nombreArchivo = "ListaPrecios_" + DateTime.Now.ToString("ddMMyyyy");
            string from = Encryption.DecryptPassword(ConfigurationManager.AppSettings["MailFrom"]);
            string mailPass = Encryption.DecryptPassword(ConfigurationManager.AppSettings["MailPassword"]);            

            if (!ModelState.IsValid)
            {
                ViewBag.ListaID = listaid;
                return View("Mail\\EnviarMailListaPreciosForm", datos);
            }
            
            using (MailMessage mail = new MailMessage(from, datosMail.To))
            {

                mail.Subject = datosMail.Subject;
                mail.Body = datosMail.Body;         

                //Cargamos el adjunto en memoria
                MemoryStream stream = new MemoryStream(ListaPreciosPDFMail(listaid, nombreArchivo));
                Attachment att = new Attachment(stream, nombreArchivo + ".pdf", "application/pdf");
                mail.Attachments.Add(att);

                mail.IsBodyHtml = false;

                SmtpClient smtp = new SmtpClient();

                smtp.Host = ConfigurationManager.AppSettings["MailHost"];
                smtp.UseDefaultCredentials = false;

                NetworkCredential networkCredential = new NetworkCredential(from, mailPass);
                smtp.EnableSsl = false;
                smtp.Credentials = networkCredential;
                smtp.Port = int.Parse(ConfigurationManager.AppSettings["MailPort"]);

                try
                {
                    log.Info("Enviando mail, datos: Subject: " + mail.Subject + ", To: " + mail.To);
                    smtp.Send(mail);
                    log.Info("Mail Enviado satisfactoriamente");

                    ViewBag.Message = "Enviado";
                    ViewBag.ListaID = listaid;
                    
                }
                catch (Exception e)
                {
                    log.Error("Error al enviar el mail: " + e.Message);              
                }

                return View("Mail\\EnviarMailListaPreciosForm", datos);

            }
            
        }

        public Byte[] ListaPreciosPDFMail(int listaid, string filename)
        {
            //Traemos la lista de precios seleccionada
            var lis = listaPreciosBL.GetListaById(listaid);
            var listaDePrecios = listaPreciosBL.GetListaPrecioExportByListaId(listaid);
            //var categorias = commonBL.GetAllCategorias();
            //var marcas = commonBL.GetAllMarcas();
            List<Marca> marca = new List<Marca>();
            List<Categoria> categoria = new List<Categoria>();

            //Filtramos en base a las marcas y categorias que contengan productos Blister
            foreach (var item in listaDePrecios)
            {
                if (item.Producto.Categoria != null)
                {
                    var categoExistente = categoria.Find(c => c.Nombre == item.Producto.Categoria.Nombre);
                    if (categoExistente == null)
                        categoria.Add(item.Producto.Categoria);
                }

                else if (item.Producto.Marca != null)
                {
                    var marcaExistente = marca.Find(c => c.Nombre == item.Producto.Marca.Nombre);
                    if (marcaExistente == null)
                        marca.Add(item.Producto.Marca);
                }
            }

            ListaPreciosExportViewModel viewModel = new ListaPreciosExportViewModel()
            {
                ListaPrecios = listaDePrecios,
                Categorias = categoria,
                Marcas = marca,
                ListaId = listaid,
                Nombre = lis.Nombre
            };

            var pdf = new ViewAsPdf("Export\\ListaPreciosPDF", viewModel) 
            {
                FileName = filename,
                PageSize = Size.A4,
                PageOrientation = Orientation.Portrait,
                PageMargins = new Margins(15, 15, 15, 15),
            };

            log.Info("Exportando Lista de Precios en formato PDF: " + pdf.FileName);

            Byte[] PdfData = pdf.BuildFile(ControllerContext);

            if (PdfData == null)
                log.Error("Error al exportar el archivo PDF.");

            return PdfData;
        }

        #endregion

        #region Acciones Descarga/Exportar Lista de Precios Blister
        public ActionResult ExportarListaPreciosBlister()
        {

            var listaDePreciosBlister = listaPreciosBL.GetAllExportListaPrecioBlister();
            //var categorias = commonBL.GetAllCategorias();
            //var marcas = commonBL.GetAllMarcas();

            if (listaDePreciosBlister == null)
            {
                log.Error("No se pudo traer la lista de precios Blister para exportar");
                return View("Error");
            }

            List<Marca> marca = new List<Marca>();
            List<Categoria> categoria = new List<Categoria>();

            //Filtramos en base a las marcas y categorias que contengan productos Blister
            foreach (var item in listaDePreciosBlister)
            {
                if (item.Producto.Categoria != null)
                {
                    var categoExistente = categoria.Find(c => c.Nombre == item.Producto.Categoria.Nombre);
                    if (categoExistente == null)
                        categoria.Add(item.Producto.Categoria);
                }

                else if (item.Producto.Marca != null)
                {
                    var marcaExistente = marca.Find(c => c.Nombre == item.Producto.Marca.Nombre);
                    if (marcaExistente == null)
                        marca.Add(item.Producto.Marca);
                }
            }



            ListaPreciosBlisterExportViewModel viewModel = new ListaPreciosBlisterExportViewModel()
            {
                ListaPreciosBlister = listaDePreciosBlister,
                Categorias = categoria,
                Marcas = marca,
            };

            return View("ListaPreciosBlisterExport", viewModel);

        }

        public ActionResult DescargarListaPreciosBlisterPDF()
        {

            var listaDePreciosBlister = listaPreciosBL.GetAllExportListaPrecioBlister();
            //var categorias = commonBL.GetAllCategorias();
            //var marcas = commonBL.GetAllMarcas();

            if (listaDePreciosBlister == null)
            {
                log.Error("No se pudo traer la lista de precios Blister para exportar");
                return View("Error");
            }

            List<Marca> marca = new List<Marca>();
            List<Categoria> categoria = new List<Categoria>();

            //Filtramos en base a las marcas y categorias que contengan productos Blister
            foreach (var item in listaDePreciosBlister)
            {
                if (item.Producto.Categoria != null)
                {
                    var categoExistente = categoria.Find(c => c.Nombre == item.Producto.Categoria.Nombre);
                    if (categoExistente == null)
                        categoria.Add(item.Producto.Categoria);
                }

                else if (item.Producto.Marca != null)
                {
                    var marcaExistente = marca.Find(c => c.Nombre == item.Producto.Marca.Nombre);
                    if (marcaExistente == null)
                        marca.Add(item.Producto.Marca);
                }
            }

            ListaPreciosBlisterExportViewModel viewModel = new ListaPreciosBlisterExportViewModel()
            {
                ListaPreciosBlister = listaDePreciosBlister,
                Categorias = categoria,
                Marcas = marca,
            };

            var pdf = new ViewAsPdf("Export\\ListaPreciosBlisterPDF", viewModel)
            {
                PageSize = Size.A4,
                PageOrientation = Orientation.Portrait,
                PageMargins = new Margins(15, 15, 15, 15),
            };

            return pdf;
        }

        public ActionResult MailListaPreciosBlisterForm()
        {

            ViewBag.Message = "nuevomail";

            return View("Mail\\EnviarMailListaPreciosBlisterForm");

        }

        public ActionResult EnviarMailListaPreciosBlister(MailModel datosMail)
        {

            MailModel datos = datosMail;
            string nombreArchivo = "ListaPreciosBlister_" + DateTime.Now.ToString("ddMMyyyy");
            string from = Encryption.DecryptPassword(ConfigurationManager.AppSettings["MailFrom"]);
            string mailPass = Encryption.DecryptPassword(ConfigurationManager.AppSettings["MailPassword"]);

            if (!ModelState.IsValid)
            {
                return View("Mail\\EnviarMailListaPreciosForm", datos);
            }

            using (MailMessage mail = new MailMessage(from, datosMail.To))
            {

                mail.Subject = datosMail.Subject;
                mail.Body = datosMail.Body;

                //Cargamos el adjunto en memoria
                MemoryStream stream = new MemoryStream(ListaPreciosBlisterPDFMail(nombreArchivo));
                Attachment att = new Attachment(stream, nombreArchivo + ".pdf", "application/pdf");
                mail.Attachments.Add(att);

                mail.IsBodyHtml = false;

                SmtpClient smtp = new SmtpClient();

                smtp.Host = ConfigurationManager.AppSettings["MailHost"];
                smtp.UseDefaultCredentials = false;

                NetworkCredential networkCredential = new NetworkCredential(from, mailPass);
                smtp.EnableSsl = true;
                smtp.Credentials = networkCredential;
                smtp.Port = int.Parse(ConfigurationManager.AppSettings["MailPort"]);

                try
                {
                    log.Info("Enviando mail, datos: Subject: " + mail.Subject + ", To: " + mail.To);
                    smtp.Send(mail);
                    log.Info("Mail Enviado satisfactoriamente");

                    ViewBag.Message = "Enviado";

                }
                catch (Exception e)
                {
                    log.Error("Error al enviar el mail: " + e.Message);
                }

                ViewBag.Message = "Enviado";

                return View("Mail\\EnviarMailListaPreciosBlisterForm", datos);

            }

        }

        public Byte[] ListaPreciosBlisterPDFMail(string filename)
        {
            var listaDePreciosBlister = listaPreciosBL.GetAllExportListaPrecioBlister();
            //var categorias = commonBL.GetAllCategorias();
            //var marcas = commonBL.GetAllMarcas();

            List<Marca> marca = new List<Marca>();
            List<Categoria> categoria = new List<Categoria>();

            //Filtramos en base a las marcas y categorias que contengan productos Blister
            foreach (var item in listaDePreciosBlister)
            {
                if (item.Producto.Categoria != null)
                {
                    var categoExistente = categoria.Find(c => c.Nombre == item.Producto.Categoria.Nombre);
                    if (categoExistente == null)
                        categoria.Add(item.Producto.Categoria);
                }

                else if (item.Producto.Marca != null)
                {
                    var marcaExistente = marca.Find(c => c.Nombre == item.Producto.Marca.Nombre);
                    if (marcaExistente == null)
                        marca.Add(item.Producto.Marca);
                }
            }



            ListaPreciosBlisterExportViewModel viewModel = new ListaPreciosBlisterExportViewModel()
            {
                ListaPreciosBlister = listaDePreciosBlister,
                Categorias = categoria,
                Marcas = marca,
            };

            var pdf = new ViewAsPdf("Export\\ListaPreciosBlisterPDF", viewModel)
            {
                FileName = filename,
                PageSize = Size.A4,
                PageOrientation = Orientation.Portrait,
                PageMargins = new Margins(15, 15, 15, 15),
            };

            log.Info("Exportando Lista de Precios Blister en formato PDF: " + pdf.FileName);

            Byte[] PdfData = pdf.BuildFile(ControllerContext);

            if (PdfData == null)
                log.Error("Error al exportar el archivo PDF.");

            return PdfData;
        } 
        #endregion

        #region Acciones Stock
        public ActionResult Stock()
        {

            var stock = stockBL.GetAllStock();

            if (stock == null)
            {
                log.Error("Error al traer Stock.");
                return View("Error");
            }

            return View(stock);
        }

        public ActionResult AltaStock()
        {

            ViewBag.TipoUnidad = stockBL.GetTipoDeUnidadList();

            
            return View("StockForm");
        }

        public ActionResult EditarStock(int Id)
        {

            var stock = stockBL.GetStockById(Id);

            if (stock == null)
            {
                log.Error("Error al traer Stock con ID: " + Id);
                return View("Error");
            }

            ViewBag.StockID = Id;

            return View("StockFormEdit", stock);
        }

        public ActionResult BorrarStock(int Id)
        {
            var stock = stockBL.GetStockById(Id);

            if (stock != null)
            {
                log.Info("Borrando Elemento del Stock con ID: " + Id);
                stockBL.RemoveStock(stock);
            }               
            else
            {
                log.Error("Error al traer Stock con ID: " + Id);
                return View("Error");
            }

            return RedirectToAction("Stock", "Admin");
        }

        #endregion

    }
}