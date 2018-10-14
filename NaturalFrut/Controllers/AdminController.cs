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
        private readonly ListaPreciosLogic listaPreciosBL;

        public AdminController(ClienteLogic ClienteLogic, CommonLogic CommonLogic, ProveedorLogic ProveedorLogic, ProductoLogic ProductoLogic, VendedorLogic VendedorLogic, ListaPreciosLogic ListaPreciosLogic)
        {
            clienteBL = ClienteLogic;
            commonBL = CommonLogic;
            proveedorBL = ProveedorLogic;
            productoBL = ProductoLogic;
            vendedorBL = VendedorLogic;
            listaPreciosBL = ListaPreciosLogic;
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

            var cliente = clienteBL.GetClienteById(Id);

            if (cliente == null)
                return HttpNotFound();

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
                    TipoCliente = clienteBL.GetTipoClienteList(),
                    Lista = clienteBL.GetListaList()
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
            ProveedorViewModel viewModel = new ProveedorViewModel();

            return View("ProveedorForm", viewModel);
        }

        public ActionResult EditarProveedor(int Id)
        {

            var proveedor = proveedorBL.GetProveedorById(Id);

            ProveedorViewModel viewModel = new ProveedorViewModel(proveedor)
 ;

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

                ProveedorViewModel viewModel = new ProveedorViewModel(proveedor);


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

            ProductoViewModel viewModel = new ProductoViewModel(producto)
            {
                Categoria = productoBL.GetCategoriaList(),
                Marca = productoBL.GetMarcaList()
            };

            if (producto == null)
                return HttpNotFound();

            return View("ProductoForm", viewModel);
        }

        public ActionResult BorrarProducto(int Id)
        {
            var producto = productoBL.GetProductoById(Id);

            if (producto != null)
            {
                if (producto.EsMix)
                {

                    var listaProductosDelMix = productoBL.GetListaProductosMixById(producto.ID);

                    if(listaProductosDelMix != null)
                    {
                        foreach (var productoMix in listaProductosDelMix)
                        {
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
                    Categoria = productoBL.GetCategoriaList(),
                    Marca = productoBL.GetMarcaList()
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


        #region Acciones Producto Mix
        public ActionResult ProductosMix()
        {

            var productosMix = productoBL.GetAllProductoMix();

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
                return HttpNotFound();

            var productosMix = productoBL.GetListaProductosMixById(Id);

            if (productosMix == null)
                return HttpNotFound();

            ViewBag.ProdMixID = prodMix.ID;
            ViewBag.ProdMixNombre = prodMix.Nombre;

            return View("ProductoMixFormEdit", productosMix);
        }

        public ActionResult VerProductoMix(int Id)
        {

            var prodMix = productoBL.GetProductoById(Id);

            if (prodMix == null)
                return HttpNotFound();

            var productosMix = productoBL.GetListaProductosMixById(Id);

            if (productosMix == null)
                return HttpNotFound();

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
                //Agregamos nuevo Proveedor
                productoBL.AddProductoMix(productoMix);
            }
            else
            {
                //Edicion de Proveedor Existente
                productoBL.UpdateProductoMix(productoMix);
            }

            return RedirectToAction("ProductosMix", "Admin");

        } 
        #endregion


        #region Acciones Vendedor
        public ActionResult vendedores()
        {

            var vendedores = vendedorBL.GetAllVendedores();

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

        #region Acciones Categoria
        public ActionResult Categorias()
        {

            var categorias = commonBL.GetAllCategorias();

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
                return HttpNotFound();

            return View("CategoriaForm", categoria);
        }

        public ActionResult BorrarCategoria(int Id)
        {

            var categoria = commonBL.GetCategoriaById(Id);

            if (categoria != null)
                commonBL.RemoveCategoria(categoria);
            else
                return HttpNotFound();

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
                commonBL.AddCategoria(categoria);
            }
            else
            {
                //Edicion de Tipo Cliente existente
                commonBL.UpdateCategoria(categoria);
            }

            return RedirectToAction("Categorias", "Admin");

        }
        #endregion

        #region Acciones Marca
        public ActionResult Marcas()
        {
            var marcas = commonBL.GetAllMarcas();

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
                return HttpNotFound();

            return View("MarcaForm", marca);
        }

        public ActionResult BorrarMarca(int Id)
        {

            var marca = commonBL.GetMarcaById(Id);

            if (marca != null)
                commonBL.RemoveMarca(marca);
            else
                return HttpNotFound();

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
                commonBL.AddMarca(marca);
            }
            else
            {
                //Edicion de Tipo Cliente existente
                commonBL.UpdateMarca(marca);
            }

            return RedirectToAction("Marcas", "Admin");

        }
        #endregion

        #region Acciones Tipo de Unidad
        public ActionResult TiposDeUnidad()
        {

            var tiposDeUnidad = commonBL.GetAllTiposDeUnidad();

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
                return HttpNotFound();

            return View("TipoDeUnidadForm", tipoDeUnidad);
        }

        public ActionResult BorrarTipoDeUnidad(int Id)
        {
            var tipoDeUnidad = commonBL.GetTipoDeUnidadById(Id);

            if (tipoDeUnidad != null)
                commonBL.RemoveTipoDeUnidad(tipoDeUnidad);
            else
                return HttpNotFound();

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
                commonBL.AddTipoDeUnidad(tipoDeUnidad);
            }
            else
            {
                //Edicion de Tipo Cliente existente
                commonBL.UpdateTipoDeUnidad(tipoDeUnidad);
            }

            return RedirectToAction("TiposDeUnidad", "Admin");

        }
        #endregion

        #region Acciones Lista
        public ActionResult Lista()
        {
            var lista = listaPreciosBL.GetAllLista();

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
                return HttpNotFound();

            return View("ListaForm", lista);
        }

        public ActionResult BorrarLista(int Id)
        {

            var lista = listaPreciosBL.GetListaById(Id);

            if (lista != null)
                listaPreciosBL.RemoveLista(lista);
            else
                return HttpNotFound();

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
                listaPreciosBL.AddLista(lista);
            }
            else
            {
                //Edicion de Cliente Existente
                listaPreciosBL.UpdateLista(lista);
            }

            return RedirectToAction("Lista", "Admin");

        } 
        #endregion

        #region Acciones Lista Precios
        public ActionResult ListaPrecios()
        {
            var listaPrecio = listaPreciosBL.GetAllListaPrecio();
            ViewBag.ListaPrincipalID = Constants.LISTAPRINCIPAL;

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

            ListaPreciosViewModel viewModel = new ListaPreciosViewModel(listaPrecio)
            {
                Lista = listaPreciosBL.GetListaList(),
                Producto = listaPreciosBL.GetProductoList()
            };

            if (listaPrecio == null)
                return HttpNotFound();

            return View("ListaPreciosForm", viewModel);
        }

        public ActionResult BorrarListaPrecios(int Id)
        {

            var listaPrecio = listaPreciosBL.GetListaPrecioById(Id);

            if (listaPrecio != null)
                listaPreciosBL.RemoveListaPrecio(listaPrecio);
            else
                return HttpNotFound();

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
                listaPreciosBL.AddListaPrecio(listaPrecio);
            }
            else
            {
                //Edicion de Cliente Existente
                listaPreciosBL.UpdateListaPrecio(listaPrecio);
            }

            return RedirectToAction("ListaPrecios", "Admin");

        }

        

        #endregion


        #region Acciones Lista Precios Blister
        public ActionResult ListaPreciosBlister()
        {
            var listaPrecioBlister = listaPreciosBL.GetAllListaPrecioBlister();

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

            ListaPreciosBlisterViewModel viewModel = new ListaPreciosBlisterViewModel(listaPrecioBlister)
            {
                Producto = listaPreciosBL.GetProductoList()
            };

            if (listaPrecioBlister == null)
                return HttpNotFound();

            return View("ListaPreciosBlisterForm", viewModel);
        }

        public ActionResult BorrarListaPreciosBlister(int Id)
        {

            var listaPrecioBlister = listaPreciosBL.GetListaPrecioBlisterById(Id);

            if (listaPrecioBlister != null)
                listaPreciosBL.RemoveListaPrecioBlister(listaPrecioBlister);
            else
                return HttpNotFound();

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
                listaPreciosBL.AddListaPrecioBlister(listaPrecioBlister);
            }
            else
            {
                //Edicion de Cliente Existente
                listaPreciosBL.UpdateListaPrecioBlister(listaPrecioBlister);
            }

            return RedirectToAction("ListaPreciosBlister", "Admin");

        }

        #endregion




        #region Acciones Clasificacion
        public ActionResult Clasificacion()
        {
            var clasificacion = commonBL.GetAllClasificacion();

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
                return HttpNotFound();

            return View("ClasificacionForm", clasificacion);
        }

        public ActionResult BorrarClasificacion(int Id)
        {

            var clasificacion = commonBL.GetClasificacionById(Id);

            if (clasificacion != null)
                commonBL.RemoveClasificacion(clasificacion);
            else
                return HttpNotFound();

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
                
                commonBL.AddClasificacion(clasificacion);
            }
            else
            {
                
                commonBL.UpdateClasificacion(clasificacion);
            }

            return RedirectToAction("Clasificacion", "Admin");

        }
        #endregion

        #region Acciones Descarga/Exportar Lista de Precios

        public ActionResult ExportarListaPrecios()
        {

            var listaPrecios = listaPreciosBL.GetAllLista();

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
            string from = "marcelomusza@gmail.com";

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

                smtp.Host = "smtp.gmail.com";
                smtp.UseDefaultCredentials = false;

                NetworkCredential networkCredential = new NetworkCredential(from, "MaRce1000%!");
                smtp.EnableSsl = true;
                smtp.Credentials = networkCredential;
                smtp.Port = 25;

                smtp.Send(mail);

                ViewBag.Message = "Enviado";
                ViewBag.ListaID = listaid;

                return View("Mail\\EnviarMailListaPreciosForm", datos);

            }
            
        }

        public Byte[] ListaPreciosPDFMail(int listaid, string filename)
        {
            //Traemos la lista de precios seleccionada
            var lis = listaPreciosBL.GetListaById(listaid);
            var listaDePrecios = listaPreciosBL.GetListaPrecioByListaId(listaid);
            var categorias = commonBL.GetAllCategorias();
            var marcas = commonBL.GetAllMarcas();

            ListaPreciosExportViewModel viewModel = new ListaPreciosExportViewModel()
            {
                ListaPrecios = listaDePrecios,
                Categorias = categorias,
                Marcas = marcas,
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

            Byte[] PdfData = pdf.BuildFile(ControllerContext);
            return PdfData;
        }

        #endregion

        public ActionResult ExportarListaPreciosBlister()
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

            return View("ListaPreciosBlisterExport", viewModel);

        }

        public ActionResult DescargarListaPreciosBlisterPDF()
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
            string from = "marcelomusza@gmail.com";

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

                smtp.Host = "smtp.gmail.com";
                smtp.UseDefaultCredentials = false;

                NetworkCredential networkCredential = new NetworkCredential(from, "MaRce1000%!");
                smtp.EnableSsl = true;
                smtp.Credentials = networkCredential;
                smtp.Port = 25;

                smtp.Send(mail);

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

            Byte[] PdfData = pdf.BuildFile(ControllerContext);
            return PdfData;
        }

    }
}