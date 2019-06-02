using AutoMapper;
using log4net;
using NaturalFrut.App_BLL;
using NaturalFrut.App_BLL.Interfaces;
using NaturalFrut.App_DAL;
using NaturalFrut.DTOs;
using NaturalFrut.Helpers;
using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace NaturalFrut.Controllers.Api
{
    public class VentasMayoristaController : ApiController
    {

        private readonly VentaMayoristaLogic ventaMayoristaBL;
        private readonly StockLogic stockBL;
        private readonly ClienteLogic clienteBL;
        private readonly VendedorLogic vendedorBL;
        private readonly ProductoLogic productoBL;
        private readonly ProductoXVentaLogic productoXVentaBL;

        private UOWVentaMayorista _UOWVentaMayorista = new UOWVentaMayorista();

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public VentasMayoristaController(IRepository<VentaMayorista> VentaMayoristaRepo,
            IRepository<Stock> StockRepo,
            IRepository<Cliente> ClienteRepo,
            IRepository<Vendedor> VendedorRepo,
            IRepository<Producto> ProductoRepo,
            IRepository<ListaPrecioBlister> ListaPrecioBlisterRepo,
            IRepository<ProductoXVenta> ProductoXVentaRepo,
            IRepository<ProductoMix> ProductoMixRepo)
        {
            ventaMayoristaBL = new VentaMayoristaLogic(VentaMayoristaRepo, ListaPrecioBlisterRepo);
            stockBL = new StockLogic(StockRepo, ProductoMixRepo);
            clienteBL = new ClienteLogic(ClienteRepo);
            vendedorBL = new VendedorLogic(VendedorRepo);
            productoBL = new ProductoLogic(ProductoRepo);
            productoXVentaBL = new ProductoXVentaLogic(ProductoXVentaRepo);
        }

        //GET /api/ventasMayorista
        public IEnumerable<VentaMayoristaDTO> GetVentasMayorista()
        {
            var ventasMayorista = ventaMayoristaBL.GetAllVentaMayorista();

            return ventasMayorista.Select(Mapper.Map<VentaMayorista, VentaMayoristaDTO>);
        }


        //POST /api/ventasMayorista
        [HttpPost]
        public IHttpActionResult CreateVentasMayorista(VentaMayoristaDTO ventaMayoristaDTO)
        {
            if (!ModelState.IsValid)
            {
                log.Error("Formulario con datos incorrectos o insuficientes.");
                return BadRequest();
            }

            Cliente cliente = clienteBL.GetClienteById(ventaMayoristaDTO.ClienteID);

            if (cliente == null)
            {
                log.Error("No se ha encontrado cliente en la base de datos con ID: " + ventaMayoristaDTO.ClienteID);
                return BadRequest();
            }

           
            var ventaMayorista = Mapper.Map<VentaMayoristaDTO, VentaMayorista>(ventaMayoristaDTO);

            //ventaMayoristaBL.AddVentaMayorista(ventaMayorista);
            _UOWVentaMayorista.VentaMayoristaRepository.Add(ventaMayorista);
           

            if(ventaMayoristaDTO.NoConcretado)
            {
                log.Info("Venta Concretada. Saldo anterior del cliente: " + cliente.Debe);
                //Actualizamos el Saldo en base a la Entrega de Efectivo   
                cliente.Debe = ventaMayorista.Debe;
                cliente.SaldoAfavor = ventaMayorista.SaldoAFavor;

                //clienteBL.UpdateCliente(cliente);
                _UOWVentaMayorista.ClienteRepository.Update(cliente);

                log.Info("Nuevo Saldo del cliente: " + cliente.Debe);

            }
           
            //Una vez cargada la venta, actualizamos Stock
            foreach (var item in ventaMayorista.ProductosXVenta)
            {

                Producto producto = productoBL.GetProductoById(item.ProductoID);

                //Actualizamos dependiendo si es un producto Blister o Comun
                if(producto.EsMix && producto.EsBlister)
                {

                    //Para casos particulares de productos mix y blister a la vez
                    if(item.TipoDeUnidadID == Constants.TIPODEUNIDAD_MIX)
                    {
                        //Producto Mix - Actualizar Stock para sus productos asociados
                        var productosMixStock = stockBL.GetListaProductosMixById(producto.ID);

                        if (productosMixStock == null)
                        {
                            log.Error("No se ha encontrado lista de Productos Mix del Producto con ID: " + producto.ID);
                            return BadRequest();
                        }

                        foreach (var prod in productosMixStock)
                        {

                            int prodDelMixID = prod.ProductoDelMixId.GetValueOrDefault();
                            //Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), item.TipoDeUnidadID);
                            Stock stockProdMix = _UOWVentaMayorista.StockRepository
                                .GetAll()
                                .Where(s => s.ProductoID == prodDelMixID && s.TipoDeUnidadID == item.TipoDeUnidadID)
                                .SingleOrDefault();


                            if (stockProdMix == null)
                            {
                                log.Error("No se ha encontrado Stock para producto del Producto Mix con ID: " + prodDelMixID);
                                return BadRequest();
                            }

                            double cantidadEnKG = Convert.ToDouble(prod.Cantidad) * Convert.ToDouble(item.Cantidad);
                            stockProdMix.Cantidad = stockProdMix.Cantidad - cantidadEnKG;

                            if (stockProdMix.Cantidad < 0)
                                stockProdMix.Cantidad = 0;

                            //stockBL.UpdateStock(stockProdMix);
                            _UOWVentaMayorista.StockRepository.Update(stockProdMix);

                            log.Info("Stock Actualizado para Producto del mix con ID: " + prodDelMixID + ". Stock: " + stockProdMix.Cantidad);
                            
                        }
                    }
                    else if(item.TipoDeUnidadID == Constants.TIPODEUNIDAD_BLISTER)
                    {
                        //Caso particular para descontar stock de Blister
                        var productosBlisterMixStock = stockBL.GetListaProductosMixById(producto.ID);

                        if (productosBlisterMixStock == null)
                        {
                            log.Error("No se ha encontrado Stock para el producto blister/mix con ID: " + producto.ID);
                            return BadRequest();
                        }

                        foreach (var prod in productosBlisterMixStock)
                        {
                            int prodDelMixID = prod.ProductoDelMixId.GetValueOrDefault();
                            //Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), Constants.TIPODEUNIDAD_MIX);
                            Stock stockProdMix = _UOWVentaMayorista.StockRepository
                                .GetAll()
                                .Where(s => s.ProductoID == prodDelMixID && s.TipoDeUnidadID == Constants.TIPODEUNIDAD_MIX)
                                .SingleOrDefault();

                            if (stockProdMix == null)
                            {
                                log.Error("No se ha encontrado Stock para producto del Producto Blister/Mix con ID: " + prodDelMixID);
                                return BadRequest();
                            }

                            //Convertimos la cantidad a gramos (en formato kg)
                            double cantidadEnGramos = (Convert.ToDouble(prod.Cantidad) / 10) * item.Cantidad;
                            stockProdMix.Cantidad = stockProdMix.Cantidad - cantidadEnGramos;

                            if (stockProdMix.Cantidad < 0)
                                stockProdMix.Cantidad = 0;

                            //stockBL.UpdateStock(stockProdMix);
                            _UOWVentaMayorista.StockRepository.Update(stockProdMix);

                            log.Info("Stock Actualizado para Producto Blister/Mix con ID: " + prodDelMixID + ". Stock: " + stockProdMix.Cantidad);
                        }

                    }

                }
                //else if (item.TipoDeUnidadID == Constants.TIPODEUNIDAD_BLISTER)
                //{
                //    Stock stock = stockBL.ValidarStockProducto(item.ProductoID, item.TipoDeUnidadID);

                //    ListaPrecioBlister productoBlisterSegunLista = ventaMayoristaBL.CalcularImporteBlisterSegunCliente(item.ProductoID);

                //    double cantidadEnKG = (Convert.ToDouble(productoBlisterSegunLista.Gramos) * Convert.ToDouble(item.Cantidad)) / 1000; //Convierto a KG
                //    stock.Cantidad = stock.Cantidad - cantidadEnKG;

                //    stockBL.UpdateStock(stock);

                //}
                else if (producto.EsMix)
                {
                    //Producto Mix - Actualizar Stock para sus productos asociados
                    var productosMixStock = stockBL.GetListaProductosMixById(producto.ID);

                    if (productosMixStock == null)
                    {
                        log.Error("No se ha encontrado Stock para el Producto Mix con ID: " + producto.ID);
                        return BadRequest();
                    }

                    foreach (var prod in productosMixStock)
                    {
                        int prodDelMixID = prod.ProductoDelMixId.GetValueOrDefault();
                        //Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), item.TipoDeUnidadID);
                        Stock stockProdMix = _UOWVentaMayorista.StockRepository
                            .GetAll()
                            .Where(s => s.ProductoID == prodDelMixID && s.TipoDeUnidadID == item.TipoDeUnidadID)
                            .SingleOrDefault();

                        if (stockProdMix == null)
                        {
                            log.Error("No se ha encontrado Stock para producto del Producto Mix con ID: " + prodDelMixID);
                            return BadRequest();
                        }

                        //Convertimos la cantidad a gramos (en formato kg)
                        double cantidadEnKG = Convert.ToDouble(prod.Cantidad) * Convert.ToDouble(item.Cantidad);
                        stockProdMix.Cantidad = stockProdMix.Cantidad - cantidadEnKG;

                        if (stockProdMix.Cantidad < 0)
                            stockProdMix.Cantidad = 0;

                        //stockBL.UpdateStock(stockProdMix);
                        _UOWVentaMayorista.StockRepository.Update(stockProdMix);

                        log.Info("Stock Actualizado para Producto del mix con ID: " + prodDelMixID + ". Stock: " + stockProdMix.Cantidad);
                    }
                }
                else
                {
                    //Stock para el productos comunes y blisters
                    if (item.TipoDeUnidadID == Constants.TIPODEUNIDAD_BLISTER)
                    {
                        //Producto Blister
                        //Stock stock = stockBL.ValidarStockProducto(item.ProductoID, Constants.PRECIO_X_KG);
                        Stock stock = _UOWVentaMayorista.StockRepository
                                .GetAll()
                                .Where(s => s.ProductoID == item.ProductoID && s.TipoDeUnidadID == Constants.PRECIO_X_KG)
                                .SingleOrDefault();

                        ListaPrecioBlister productoBlisterSegunLista = ventaMayoristaBL.CalcularImporteBlisterSegunCliente(item.ProductoID);
                       
                        double cantidadEnKG = (Convert.ToDouble(item.Cantidad) * (Convert.ToDouble(productoBlisterSegunLista.Gramos) / 1000)); //Convierto a KG

                        stock.Cantidad = stock.Cantidad - cantidadEnKG;

                        if (stock.Cantidad < 0)
                            stock.Cantidad  = 0;

                        //stockBL.UpdateStock(stock);
                        _UOWVentaMayorista.StockRepository.Update(stock);

                        log.Info("Stock Actualizado para Producto Blister con ID: " + item.ProductoID + ". Stock: " + stock.Cantidad);
                    }
                    else
                    {
                        //Producto Comun
                        //Stock stock = stockBL.ValidarStockProducto(item.ProductoID, item.TipoDeUnidadID);
                        Stock stock = _UOWVentaMayorista.StockRepository
                               .GetAll()
                               .Where(s => s.ProductoID == item.ProductoID && s.TipoDeUnidadID == item.TipoDeUnidadID)
                               .SingleOrDefault();

                        stock.Cantidad = stock.Cantidad - item.Cantidad;

                        if (stock.Cantidad < 0)
                            stock.Cantidad = 0;

                        //stockBL.UpdateStock(stock);
                        _UOWVentaMayorista.StockRepository.Update(stock);

                        log.Info("Stock Actualizado para Producto Comun con ID: " + item.ProductoID + ". Stock: " + stock.Cantidad);
                    }
                }

            }

            //Salvamos los cambios
            _UOWVentaMayorista.Save();

            log.Info("Venta Mayorista creada satisfactoriamente.");

            return Ok();
        }


        //PUT /api/ventasMayorista
        [HttpPut]
        public IHttpActionResult UpdateVentasMayorista(VentaMayoristaDTO ventaMayoristaDTO)
        {
            if (!ModelState.IsValid)
            {
                log.Error("Formulario con datos incorrectos o insuficientes.");
                return BadRequest();
            }

            //var cliente = clienteBL.GetClienteById(ventaMayoristaDTO.ClienteID);
            var cliente = _UOWVentaMayorista.ClienteRepository.GetByID(ventaMayoristaDTO.ClienteID);

            if (cliente == null)
            {
                log.Error("No se ha encontrado cliente en la base de datos con ID: " + ventaMayoristaDTO.ClienteID);
                return BadRequest();
            }

            //var ventaMayoristaInDB = ventaMayoristaBL.GetVentaMayoristaById(ventaMayoristaDTO.ID);
            var ventaMayoristaInDB = _UOWVentaMayorista.VentaMayoristaRepository.GetByID(ventaMayoristaDTO.ID);

            //List<float> cantProds = new List<float>();
            int ventaId = ventaMayoristaInDB.ID;

            //foreach (var prod in ventaMayoristaInDB.ProductosXVenta)
            //{
            //    cantProds.Add(prod.Cantidad);
            //}


            if (ventaMayoristaInDB == null)
            {
                log.Error("No se ha encontrado Venta Mayorista en la base de datos con ID: " + ventaMayoristaDTO.ID);
                return NotFound();
            }


            VentaMayorista ventaUpdate = _UOWVentaMayorista.VentaMayoristaRepository.GetByID(ventaMayoristaDTO.ID);

            //Update para los campos de VentaMayorista
            ventaUpdate.Descuento = ventaMayoristaDTO.Descuento;
            ventaUpdate.EntregaEfectivo = ventaMayoristaDTO.EntregaEfectivo;
            ventaUpdate.Impreso = ventaMayoristaDTO.Impreso;
            ventaUpdate.NoConcretado = ventaMayoristaDTO.NoConcretado;
            ventaUpdate.Facturado = ventaMayoristaDTO.Facturado;
            ventaUpdate.IVA = ventaMayoristaDTO.IVA;
            ventaUpdate.SumaTotal = ventaMayoristaDTO.SumaTotal;
            ventaUpdate.Debe = ventaMayoristaDTO.Debe;
            ventaUpdate.SaldoAFavor = ventaMayoristaDTO.SaldoAFavor;

            //UPDATE VENTA MAYORISTA
            //ventaMayoristaBL.UpdateVentaMayorista(ventaMayoristaInDB);
            _UOWVentaMayorista.VentaMayoristaRepository.Update(ventaUpdate);

                        
            if (ventaMayoristaDTO.NoConcretado)
            {

                //Actualizamos el Saldo en base a la Entrega de Efectivo   
                cliente.Debe = ventaMayoristaDTO.Debe;
                cliente.SaldoAfavor = ventaMayoristaDTO.SaldoAFavor;

                //clienteBL.UpdateCliente(cliente);
                _UOWVentaMayorista.ClienteRepository.Update(cliente);

            }

            //Borramos todos los productos de la venta, para luego agregarlos nuevamente junto con sus actualizados
            bool borrado = false;
            borrado = DeleteProductosParaUpdate(ventaMayoristaInDB);

            if(!borrado)
            {
                log.Error("Se ha producido un error intentando borrar los productos de la venta al momento de actualizar. Venta ID: " + ventaMayoristaInDB.ID);
                return BadRequest();
            }

            if(ventaMayoristaDTO.ProductosXVenta != null)
            {
                //Agregamos nuevamente los productos a la venta, actualizados
                foreach (var prodVenta in ventaMayoristaDTO.ProductosXVenta)
                {
                    //UPDATE PRODUCTOS DE VENTA MAYORISTA
                    _UOWVentaMayorista.ProductosXVentaRepository.Add(prodVenta);
                }

                //Una vez actualizada la venta, actualizamos Stock
                foreach (var item in ventaMayoristaDTO.ProductosXVenta)
                {

                    Producto producto = productoBL.GetProductoById(item.ProductoID);

                    //Actualizamos dependiendo si es un producto Blister o Comun
                    if (producto.EsMix && producto.EsBlister)
                    {

                        //Para casos particulares de productos mix y blister a la vez
                        if (item.TipoDeUnidadID == Constants.TIPODEUNIDAD_MIX)
                        {
                            //Producto Mix - Actualizar Stock para sus productos asociados
                            var productosMixStock = stockBL.GetListaProductosMixById(producto.ID);

                            if (productosMixStock == null)
                            {
                                log.Error("No se ha encontrado lista de Productos Mix del Producto con ID: " + producto.ID);
                                return BadRequest();
                            }

                            foreach (var prod in productosMixStock)
                            {

                                int prodDelMixID = prod.ProductoDelMixId.GetValueOrDefault();
                                //Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), item.TipoDeUnidadID);
                                Stock stockProdMix = _UOWVentaMayorista.StockRepository
                                    .GetAll()
                                    .Where(s => s.ProductoID == prodDelMixID && s.TipoDeUnidadID == item.TipoDeUnidadID)
                                    .SingleOrDefault();


                                if (stockProdMix == null)
                                {
                                    log.Error("No se ha encontrado Stock para producto del Producto Mix con ID: " + prodDelMixID);
                                    return BadRequest();
                                }

                                double cantidadEnKG = Convert.ToDouble(prod.Cantidad) * Convert.ToDouble(item.Cantidad);
                                stockProdMix.Cantidad = stockProdMix.Cantidad - cantidadEnKG;

                                if (stockProdMix.Cantidad < 0)
                                    stockProdMix.Cantidad = 0;

                                //stockBL.UpdateStock(stockProdMix);
                                _UOWVentaMayorista.StockRepository.Update(stockProdMix);

                                log.Info("Stock Actualizado para Producto del mix con ID: " + prodDelMixID + ". Stock: " + stockProdMix.Cantidad);

                            }
                        }
                        else if (item.TipoDeUnidadID == Constants.TIPODEUNIDAD_BLISTER)
                        {
                            //Caso particular para descontar stock de Blister
                            var productosBlisterMixStock = stockBL.GetListaProductosMixById(producto.ID);

                            if (productosBlisterMixStock == null)
                            {
                                log.Error("No se ha encontrado Stock para el producto blister/mix con ID: " + producto.ID);
                                return BadRequest();
                            }

                            foreach (var prod in productosBlisterMixStock)
                            {
                                int prodDelMixID = prod.ProductoDelMixId.GetValueOrDefault();
                                //Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), Constants.TIPODEUNIDAD_MIX);
                                Stock stockProdMix = _UOWVentaMayorista.StockRepository
                                    .GetAll()
                                    .Where(s => s.ProductoID == prodDelMixID && s.TipoDeUnidadID == Constants.TIPODEUNIDAD_MIX)
                                    .SingleOrDefault();

                                if (stockProdMix == null)
                                {
                                    log.Error("No se ha encontrado Stock para producto del Producto Blister/Mix con ID: " + prodDelMixID);
                                    return BadRequest();
                                }

                                //Convertimos la cantidad a gramos (en formato kg)
                                double cantidadEnGramos = (Convert.ToDouble(prod.Cantidad) / 10) * item.Cantidad;
                                stockProdMix.Cantidad = stockProdMix.Cantidad - cantidadEnGramos;

                                if (stockProdMix.Cantidad < 0)
                                    stockProdMix.Cantidad = 0;

                                //stockBL.UpdateStock(stockProdMix);
                                _UOWVentaMayorista.StockRepository.Update(stockProdMix);

                                log.Info("Stock Actualizado para Producto Blister/Mix con ID: " + prodDelMixID + ". Stock: " + stockProdMix.Cantidad);
                            }

                        }

                    }
                    //else if (item.TipoDeUnidadID == Constants.TIPODEUNIDAD_BLISTER)
                    //{
                    //    Stock stock = stockBL.ValidarStockProducto(item.ProductoID, item.TipoDeUnidadID);

                    //    ListaPrecioBlister productoBlisterSegunLista = ventaMayoristaBL.CalcularImporteBlisterSegunCliente(item.ProductoID);

                    //    double cantidadEnKG = (Convert.ToDouble(productoBlisterSegunLista.Gramos) * Convert.ToDouble(item.Cantidad)) / 1000; //Convierto a KG
                    //    stock.Cantidad = stock.Cantidad - cantidadEnKG;

                    //    stockBL.UpdateStock(stock);

                    //}
                    else if (producto.EsMix)
                    {
                        //Producto Mix - Actualizar Stock para sus productos asociados
                        var productosMixStock = stockBL.GetListaProductosMixById(producto.ID);

                        if (productosMixStock == null)
                        {
                            log.Error("No se ha encontrado Stock para el Producto Mix con ID: " + producto.ID);
                            return BadRequest();
                        }

                        foreach (var prod in productosMixStock)
                        {
                            int prodDelMixID = prod.ProductoDelMixId.GetValueOrDefault();
                            //Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), item.TipoDeUnidadID);
                            Stock stockProdMix = _UOWVentaMayorista.StockRepository
                                .GetAll()
                                .Where(s => s.ProductoID == prodDelMixID && s.TipoDeUnidadID == item.TipoDeUnidadID)
                                .SingleOrDefault();

                            if (stockProdMix == null)
                            {
                                log.Error("No se ha encontrado Stock para producto del Producto Mix con ID: " + prodDelMixID);
                                return BadRequest();
                            }

                            //Convertimos la cantidad a gramos (en formato kg)
                            double cantidadEnKG = Convert.ToDouble(prod.Cantidad) * Convert.ToDouble(item.Cantidad);
                            stockProdMix.Cantidad = stockProdMix.Cantidad - cantidadEnKG;

                            if (stockProdMix.Cantidad < 0)
                                stockProdMix.Cantidad = 0;

                            //stockBL.UpdateStock(stockProdMix);
                            _UOWVentaMayorista.StockRepository.Update(stockProdMix);

                            log.Info("Stock Actualizado para Producto del mix con ID: " + prodDelMixID + ". Stock: " + stockProdMix.Cantidad);
                        }
                    }
                    else
                    {
                        //Stock para el productos comunes y blisters
                        if (item.TipoDeUnidadID == Constants.TIPODEUNIDAD_BLISTER)
                        {
                            //Producto Blister
                            //Stock stock = stockBL.ValidarStockProducto(item.ProductoID, Constants.PRECIO_X_KG);
                            Stock stock = _UOWVentaMayorista.StockRepository
                                    .GetAll()
                                    .Where(s => s.ProductoID == item.ProductoID && s.TipoDeUnidadID == Constants.PRECIO_X_KG)
                                    .SingleOrDefault();

                            ListaPrecioBlister productoBlisterSegunLista = ventaMayoristaBL.CalcularImporteBlisterSegunCliente(item.ProductoID);

                            double cantidadEnKG = (Convert.ToDouble(item.Cantidad) * (Convert.ToDouble(productoBlisterSegunLista.Gramos) / 1000)); //Convierto a KG

                            stock.Cantidad = stock.Cantidad - cantidadEnKG;

                            if (stock.Cantidad < 0)
                                stock.Cantidad = 0;

                            //stockBL.UpdateStock(stock);
                            _UOWVentaMayorista.StockRepository.Update(stock);

                            log.Info("Stock Actualizado para Producto Blister con ID: " + item.ProductoID + ". Stock: " + stock.Cantidad);
                        }
                        else
                        {
                            //Producto Comun
                            //Stock stock = stockBL.ValidarStockProducto(item.ProductoID, item.TipoDeUnidadID);
                            Stock stock = _UOWVentaMayorista.StockRepository
                                   .GetAll()
                                   .Where(s => s.ProductoID == item.ProductoID && s.TipoDeUnidadID == item.TipoDeUnidadID)
                                   .SingleOrDefault();

                            stock.Cantidad = stock.Cantidad - item.Cantidad;

                            if (stock.Cantidad < 0)
                                stock.Cantidad = 0;

                            //stockBL.UpdateStock(stock);
                            _UOWVentaMayorista.StockRepository.Update(stock);

                            log.Info("Stock Actualizado para Producto Comun con ID: " + item.ProductoID + ". Stock: " + stock.Cantidad);
                        }
                    }

                }
            }
           


            //Actualizamos los valores
            _UOWVentaMayorista.Save();

            log.Info("Venta Mayorista actualizada satisfactoriamente.");

            return Ok();
        }

        public bool DeleteProductosParaUpdate(VentaMayorista ventaMayorista)
        {


            var productosVenta = _UOWVentaMayorista.ProductosXVentaRepository.GetAll().Where(p => p.VentaID == ventaMayorista.ID).ToList();

            //Iteramos todos los productos que vamos a borrar
            foreach (var prodVenta in productosVenta)
            {

                BorrarProdVtaMayDTO idsDeVenta = new BorrarProdVtaMayDTO
                {
                    ProductoID = prodVenta.ProductoID,
                    VentaID = ventaMayorista.ID,
                    TipoDeUnidadID = prodVenta.TipoDeUnidadID
                };

                //var productoInDB = productoXVentaBL.GetProductoXVentaIndividualById(idsDeVenta);
                var productoInDB = _UOWVentaMayorista.ProductosXVentaRepository.GetAll()
                    .Include(p => p.Producto)
                    .Include(t => t.TipoDeUnidad)
                    .Include(v => v.Venta)
                    .Where(c => c.VentaID == prodVenta.VentaID && c.ProductoID == prodVenta.ProductoID && c.TipoDeUnidadID == prodVenta.TipoDeUnidadID).SingleOrDefault();

                if (productoInDB == null)
                {
                    log.Error("No se ha encontrado Producto en la base de datos con ID:" + productoInDB);
                    return false;
                }

                //Referenciamos producto que borraremos con UOW
                var prodABorrar = _UOWVentaMayorista.ProductosXVentaRepository.GetByID(productoInDB.ID);

                //Devolvemos Stock
                //Verificamos si se trata de un producto MIX o no
                if (productoInDB.Producto.EsMix && productoInDB.Producto.EsBlister)
                {
                    //Para casos particulares en productos que son Mix y Blister a la vez

                    if (productoInDB.TipoDeUnidadID == Constants.TIPODEUNIDAD_MIX)
                    {
                        //Producto Mix - Devolver Stock
                        var productosMixStock = stockBL.GetListaProductosMixById(productoInDB.ProductoID);

                        if (productosMixStock == null)
                        {
                            log.Error("No se ha encontrado Stock para el producto mix con ID: " + productoInDB.ProductoID);
                            return false;
                        }

                        foreach (var prod in productosMixStock)
                        {

                            int prodDelMixId = prod.ProductoDelMixId.GetValueOrDefault();

                            //Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), productoInDB.TipoDeUnidadID);
                            Stock stockProdMix = _UOWVentaMayorista.StockRepository.GetAll().Where(s => s.ProductoID == prodDelMixId && s.TipoDeUnidadID == productoInDB.TipoDeUnidadID).SingleOrDefault();

                            if (stockProdMix == null)
                            {
                                log.Error("No se ha encontrado Stock para el producto del producto mix con ID: " + prod.ID);
                                return false;
                            }

                            double cantidadEnKG = Convert.ToDouble(prod.Cantidad) * Convert.ToDouble(productoInDB.Cantidad);
                            stockProdMix.Cantidad = stockProdMix.Cantidad + cantidadEnKG;

                            //stockBL.UpdateStock(stockProdMix);
                            _UOWVentaMayorista.StockRepository.Update(stockProdMix);

                            log.Info("Stock Actualizado satisfactoriamente en Producto del Producto Mix con ID: " + prod.ID + ". Stock: " + stockProdMix.Cantidad);
                        }


                    }
                    else if (productoInDB.TipoDeUnidadID == Constants.TIPODEUNIDAD_BLISTER)
                    {

                        //Caso particular para descontar stock de Blister
                        var productosBlisterMixStock = stockBL.GetListaProductosMixById(productoInDB.ProductoID);

                        if (productosBlisterMixStock == null)
                        {
                            log.Error("No se ha encontrado Stock ara el Producto Blister/Mix con ID: " + productoInDB.ProductoID);
                            return false;
                        }

                        foreach (var prod in productosBlisterMixStock)
                        {
                            //Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), Constants.TIPODEUNIDAD_MIX);
                            Stock stockProdMix = _UOWVentaMayorista.StockRepository.GetAll()
                                .Where(s => s.ProductoID == prod.ProductoDelMixId.GetValueOrDefault() && s.TipoDeUnidadID == Constants.TIPODEUNIDAD_MIX)
                                .SingleOrDefault();

                            if (stockProdMix == null)
                            {
                                log.Error("No se ha encontrado Stock para el Producto del Producto Mix/Blister con ID: " + prod.ID);
                                return false;
                            }

                            ListaPrecioBlister productoBlisterSegunLista = ventaMayoristaBL.CalcularImporteBlisterSegunCliente(productoInDB.ProductoID);
                            double cantidadEnKG = (Convert.ToDouble(productoInDB.Cantidad) * (Convert.ToDouble(productoBlisterSegunLista.Gramos) / 1000)); //Convierto a KG

                            stockProdMix.Cantidad = stockProdMix.Cantidad + cantidadEnKG;

                            //stockBL.UpdateStock(stockProdMix);
                            _UOWVentaMayorista.StockRepository.Update(stockProdMix);

                            log.Info("Stock Actualizado satisfactoriamente para el producto del producto mix / blister con ID: " + prod.ID + ". Stock: " + stockProdMix.Cantidad);
                        }

                    }

                }
                else if (productoInDB.Producto.EsMix)
                {
                    //Producto Mix - Devolver Stock
                    var productosMixStock = stockBL.GetListaProductosMixById(productoInDB.ProductoID);

                    if (productosMixStock == null)
                    {
                        log.Error("Stock no encontrado para el Producto Mix con ID: " + productoInDB.ProductoID);
                        return false;
                    }

                    foreach (var prod in productosMixStock)
                    {
                        //Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), productoInDB.TipoDeUnidadID);
                        Stock stockProdMix = _UOWVentaMayorista.StockRepository.GetAll()
                            .Where(s => s.ProductoID == prod.ProductoDelMixId.GetValueOrDefault() && s.TipoDeUnidadID == productoInDB.TipoDeUnidadID)
                            .SingleOrDefault();

                        if (stockProdMix == null)
                        {
                            log.Error("No se ha encontrado Stock para el Producto del Producto Mix con ID: " + prod.ID);
                            return false;
                        }

                        double cantidadEnKG = Convert.ToDouble(prod.Cantidad) * Convert.ToDouble(productoInDB.Cantidad);
                        stockProdMix.Cantidad = stockProdMix.Cantidad + cantidadEnKG;

                        //stockBL.UpdateStock(stockProdMix);
                        _UOWVentaMayorista.StockRepository.Update(stockProdMix);

                        log.Info("Stock Actualizado satisfactoriamente para el Producto del Producto Mix con ID: " + prod.ID + ". Stock: " + stockProdMix.Cantidad);
                    }
                }
                else
                {

                    //Stock para el productos comunes y blisters
                    if (productoInDB.TipoDeUnidadID == Constants.TIPODEUNIDAD_BLISTER)
                    {
                        //Producto Blister
                        //Stock stock = stockBL.ValidarStockProducto(productoInDB.ProductoID, Constants.PRECIO_X_KG);
                        Stock stock = _UOWVentaMayorista.StockRepository.GetAll()
                            .Where(s => s.ProductoID == productoInDB.ProductoID && s.TipoDeUnidadID == Constants.PRECIO_X_KG)
                            .SingleOrDefault();

                        ListaPrecioBlister productoBlisterSegunLista = ventaMayoristaBL.CalcularImporteBlisterSegunCliente(productoInDB.ProductoID);

                        double cantidadEnKG = (Convert.ToDouble(productoInDB.Cantidad) * (Convert.ToDouble(productoBlisterSegunLista.Gramos) / 1000)); //Convierto a KG

                        stock.Cantidad = stock.Cantidad + cantidadEnKG;

                        //stockBL.UpdateStock(stock);
                        _UOWVentaMayorista.StockRepository.Update(stock);

                        log.Info("Stock Actualizado satisfactoriamente para producto Blister con ID: " + productoInDB.ProductoID + ". Stock: " + stock.Cantidad);
                    }
                    else
                    {
                        //Producto Comun
                        //Stock stock = stockBL.ValidarStockProducto(productoInDB.ProductoID, productoInDB.TipoDeUnidadID);
                        Stock stock = _UOWVentaMayorista.StockRepository.GetAll()
                            .Where(s => s.ProductoID == productoInDB.ProductoID && s.TipoDeUnidadID == productoInDB.TipoDeUnidadID)
                            .SingleOrDefault();

                        stock.Cantidad = stock.Cantidad + productoInDB.Cantidad;

                        //stockBL.UpdateStock(stock);
                        _UOWVentaMayorista.StockRepository.Update(stock);

                        log.Info("Stock Actualizado satisfactoriamente para producto Comun con ID: " + productoInDB.ProductoID + ". Stock: " + stock.Cantidad);

                    }
                }

                _UOWVentaMayorista.ProductosXVentaRepository.Delete(prodABorrar);
                //_UOWVentaMayorista.Save();

                log.Info("Producto de la venta borrado exitosamente.");

            }

            //Si el borrado total de productos fue exitoso, devolvemos true
            return true;
        }

        //DELETE /api/ventasMayorista/1
        [HttpDelete]
        [Route("api/ventasmayorista/deleteproductoventamayorista/")]
        public IHttpActionResult DeleteProductoVentaMayorista(BorrarProdVtaMayDTO prodVenta)
        {

            var productoInDB = productoXVentaBL.GetProductoXVentaIndividualById(prodVenta);

            if (productoInDB == null)
            {
                log.Error("No se ha encontrado Producto en la base de datos con ID:" + productoInDB);
                return NotFound();
            }

            var importeTotalProducto = productoInDB.Total;

            //Referenciamos producto que borraremos con UOW
            var prodABorrar = _UOWVentaMayorista.ProductosXVentaRepository.GetByID(productoInDB.ID);
                      

            //Devolvemos Stock
            //Verificamos si se trata de un producto MIX o no
            if (productoInDB.Producto.EsMix && productoInDB.Producto.EsBlister)
            {
                //Para casos particulares en productos que son Mix y Blister a la vez

                if (productoInDB.TipoDeUnidadID == Constants.TIPODEUNIDAD_MIX)
                {
                    //Producto Mix - Devolver Stock
                    var productosMixStock = stockBL.GetListaProductosMixById(productoInDB.ProductoID);

                    if (productosMixStock == null)
                    {
                        log.Error("No se ha encontrado Stock para el producto mix con ID: " + productoInDB.ProductoID);
                        return BadRequest();
                    }

                    foreach (var prod in productosMixStock)
                    {
                        //Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), productoInDB.TipoDeUnidadID);
                        int prodDelMixID = prod.ProductoDelMixId.GetValueOrDefault();
                        //Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), item.TipoDeUnidadID);
                        Stock stockProdMix = _UOWVentaMayorista.StockRepository
                            .GetAll()
                            .Where(s => s.ProductoID == prodDelMixID && s.TipoDeUnidadID == productoInDB.TipoDeUnidadID)
                            .SingleOrDefault();

                        if (stockProdMix == null)
                        {
                            log.Error("No se ha encontrado Stock para el producto del producto mix con ID: " + prod.ID);
                            return BadRequest();
                        }

                        double cantidadEnKG = Convert.ToDouble(prod.Cantidad) * Convert.ToDouble(productoInDB.Cantidad);
                        stockProdMix.Cantidad = stockProdMix.Cantidad + cantidadEnKG;

                        //stockBL.UpdateStock(stockProdMix);
                        _UOWVentaMayorista.StockRepository.Update(stockProdMix);

                        log.Info("Stock Actualizado satisfactoriamente en Producto del Producto Mix con ID: " + prod.ID + ". Stock: " + stockProdMix.Cantidad);
                    }


                }
                else if (productoInDB.TipoDeUnidadID == Constants.TIPODEUNIDAD_BLISTER)
                {

                    //Caso particular para descontar stock de Blister
                    var productosBlisterMixStock = stockBL.GetListaProductosMixById(productoInDB.ProductoID);

                    if (productosBlisterMixStock == null)
                    {
                        log.Error("No se ha encontrado Stock ara el Producto Blister/Mix con ID: " + productoInDB.ProductoID);
                        return BadRequest();
                    }

                    foreach (var prod in productosBlisterMixStock)
                    {
                        //Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), Constants.TIPODEUNIDAD_MIX);
                        Stock stockProdMix = _UOWVentaMayorista.StockRepository
                                .GetAll()
                                .Where(s => s.ProductoID == prod.ProductoDelMixId.GetValueOrDefault() && s.TipoDeUnidadID == Constants.TIPODEUNIDAD_MIX)
                                .SingleOrDefault();

                        if (stockProdMix == null)
                        {
                            log.Error("No se ha encontrado Stock para el Producto del Producto Mix/Blister con ID: " + prod.ID);
                            return BadRequest();
                        }

                        //ListaPrecioBlister productoBlisterSegunLista = ventaMayoristaBL.CalcularImporteBlisterSegunCliente(productoInDB.ProductoID);
                        
                        //Convertimos la cantidad a gramos (en formato kg)
                        double cantidadEnKG = (Convert.ToDouble(prod.Cantidad) / 10) * productoInDB.Cantidad;
                        //double cantidadEnKG = (Convert.ToDouble(productoInDB.Cantidad) * (Convert.ToDouble(productoBlisterSegunLista.Gramos) / 1000)); //Convierto a KG

                        stockProdMix.Cantidad = stockProdMix.Cantidad + cantidadEnKG;

                        //stockBL.UpdateStock(stockProdMix);
                        _UOWVentaMayorista.StockRepository.Update(stockProdMix);

                        log.Info("Stock Actualizado satisfactoriamente para el producto del producto mix / blister con ID: " + prod.ID + ". Stock: " + stockProdMix.Cantidad);
                    }

                }

            }
            else if (productoInDB.Producto.EsMix)
            {
                //Producto Mix - Devolver Stock
                var productosMixStock = stockBL.GetListaProductosMixById(productoInDB.ProductoID);

                if (productosMixStock == null)
                {
                    log.Error("Stock no encontrado para el Producto Mix con ID: " + productoInDB.ProductoID);
                    return BadRequest();
                }

                foreach (var prod in productosMixStock)
                {
                    //Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), productoInDB.TipoDeUnidadID);
                    int prodDelMixID = prod.ProductoDelMixId.GetValueOrDefault();
                    //Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), item.TipoDeUnidadID);
                    Stock stockProdMix = _UOWVentaMayorista.StockRepository
                        .GetAll()
                        .Where(s => s.ProductoID == prodDelMixID && s.TipoDeUnidadID == productoInDB.TipoDeUnidadID)
                        .SingleOrDefault();

                    if (stockProdMix == null)
                    {
                        log.Error("No se ha encontrado Stock para el Producto del Producto Mix con ID: " + prod.ID);
                        return BadRequest();
                    }

                    double cantidadEnKG = Convert.ToDouble(prod.Cantidad) * Convert.ToDouble(productoInDB.Cantidad);
                    stockProdMix.Cantidad = stockProdMix.Cantidad + cantidadEnKG;

                    //stockBL.UpdateStock(stockProdMix);
                    _UOWVentaMayorista.StockRepository.Update(stockProdMix);

                    log.Info("Stock Actualizado satisfactoriamente para el Producto del Producto Mix con ID: " + prod.ID + ". Stock: " + stockProdMix.Cantidad);
                }
            }
            else
            {

                //Stock para el productos comunes y blisters
                if (productoInDB.TipoDeUnidadID == Constants.TIPODEUNIDAD_BLISTER)
                {
                    //Producto Blister
                    //Stock stock = stockBL.ValidarStockProducto(productoInDB.ProductoID, Constants.PRECIO_X_KG);
                    Stock stock = _UOWVentaMayorista.StockRepository
                                .GetAll()
                                .Where(s => s.ProductoID == productoInDB.ProductoID && s.TipoDeUnidadID == Constants.PRECIO_X_KG)
                                .SingleOrDefault();

                    ListaPrecioBlister productoBlisterSegunLista = ventaMayoristaBL.CalcularImporteBlisterSegunCliente(productoInDB.ProductoID);

                    double cantidadEnKG = (Convert.ToDouble(productoInDB.Cantidad) * (Convert.ToDouble(productoBlisterSegunLista.Gramos) / 1000)); //Convierto a KG

                    stock.Cantidad = stock.Cantidad + cantidadEnKG;

                    //stockBL.UpdateStock(stock);
                    _UOWVentaMayorista.StockRepository.Update(stock);

                    log.Info("Stock Actualizado satisfactoriamente para producto Blister con ID: " + productoInDB.ProductoID + ". Stock: " + stock.Cantidad);
                }
                else
                {
                    //Producto Comun
                    //Stock stock = stockBL.ValidarStockProducto(productoInDB.ProductoID, productoInDB.TipoDeUnidadID);
                    Stock stock = _UOWVentaMayorista.StockRepository
                               .GetAll()
                               .Where(s => s.ProductoID == productoInDB.ProductoID && s.TipoDeUnidadID == productoInDB.TipoDeUnidadID)
                               .SingleOrDefault();

                    stock.Cantidad = stock.Cantidad + productoInDB.Cantidad;

                    //stockBL.UpdateStock(stock);
                    _UOWVentaMayorista.StockRepository.Update(stock);

                    log.Info("Stock Actualizado satisfactoriamente para producto Comun con ID: " + productoInDB.ProductoID + ". Stock: " + stock.Cantidad);

                }
            }

            //Actualizamos el total de la venta
            var ventaMayoristaInDB = _UOWVentaMayorista.VentaMayoristaRepository.GetByID(prodVenta.VentaID);
            ventaMayoristaInDB.SumaTotal = ventaMayoristaInDB.SumaTotal - importeTotalProducto;

            //ventaMayoristaBL.UpdateVentaMayorista(ventaMayoristaInDB);
            _UOWVentaMayorista.VentaMayoristaRepository.Update(ventaMayoristaInDB);
            _UOWVentaMayorista.ProductosXVentaRepository.Delete(prodABorrar);

            

            //Removemos el producto
            //productoXVentaBL.RemoveProductoXVenta(productoInDB);

            _UOWVentaMayorista.Save();

            log.Info("Producto de la venta borrado exitosamente.");

            return Ok();

        }

        //DELETE /api/ventasMayorista/1
        [HttpDelete]
        [Route("api/ventasmayorista/deleteventamayorista/{Id}")]
        public IHttpActionResult DeleteVentaMayorista(int Id)
        {
                        
            var ventaMayoristaInDB = ventaMayoristaBL.GetVentaMayoristaById(Id);
            
            if (ventaMayoristaInDB == null)
            {
                log.Error("No se ha encontrado venta mayorista en la base de datos con ID: " + Id);
                return NotFound();
            }

            //if (ventaMayoristaInDB.NoConcretado)
            //{
            //DEVOLVEMOS STOCK
            for (int i = 0; i < ventaMayoristaInDB.ProductosXVenta.Count; i++)
            {

                int prodId = ventaMayoristaInDB.ProductosXVenta[i].ProductoID;
                int tipoUnidadId = ventaMayoristaInDB.ProductosXVenta[i].TipoDeUnidadID;

                //Verificamos si se trata de un producto MIX o no
                if (ventaMayoristaInDB.ProductosXVenta[i].Producto.EsMix && ventaMayoristaInDB.ProductosXVenta[i].Producto.EsBlister)
                {
                    //Para casos particulares en productos que son Mix y Blister a la vez

                    if (tipoUnidadId == Constants.TIPODEUNIDAD_MIX)
                    {
                        //Producto Mix
                        var productosMixStock = stockBL.GetListaProductosMixById(prodId);

                        if (productosMixStock == null)
                        {
                            log.Error("No se ha encontrado Stock para el producto mix con ID: " + prodId);
                            return BadRequest();
                        }

                        foreach (var prod in productosMixStock)
                        {

                            int prodDelMixId = prod.ProductoDelMixId.GetValueOrDefault();

                            Stock stockProdMix = _UOWVentaMayorista.StockRepository.GetAll().Where(s => s.ProductoID == prodDelMixId && s.TipoDeUnidadID == tipoUnidadId).SingleOrDefault();
                            //Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), ventaMayoristaInDB.ProductosXVenta[i].TipoDeUnidadID);

                            if (stockProdMix == null)
                            {
                                log.Error("No se ha encontrado stock para el producto del producto mix con ID: " + prodDelMixId);
                                return BadRequest();
                            }

                            double cantidadEnKG = Convert.ToDouble(prod.Cantidad) * Convert.ToDouble(ventaMayoristaInDB.ProductosXVenta[i].Cantidad);
                            stockProdMix.Cantidad = stockProdMix.Cantidad + cantidadEnKG;

                            //stockBL.UpdateStock(stockProdMix);
                            _UOWVentaMayorista.StockRepository.Update(stockProdMix);

                            log.Info("Stock actualizado satisfactoriamente para producto del producto mix con ID: " + prodDelMixId + ". Stock: " + stockProdMix.Cantidad);
                            
                        }

                    }
                    else if (tipoUnidadId == Constants.TIPODEUNIDAD_BLISTER)
                    {

                        //Producto Blister caso particular mix
                        var productosBlisterMixStock = stockBL.GetListaProductosMixById(prodId);

                        if (productosBlisterMixStock == null)
                        {
                            log.Error("No se ha encontrado Stock para el producto blister/mix con ID: " + prodId);
                            return BadRequest();
                        }

                        foreach (var prod in productosBlisterMixStock)
                        {
                            int prodDelMixId = prod.ProductoDelMixId.GetValueOrDefault();

                            Stock stockProdMix = _UOWVentaMayorista.StockRepository.GetAll().Where(s => s.ProductoID == prodDelMixId && s.TipoDeUnidadID == Constants.TIPODEUNIDAD_MIX).SingleOrDefault();
                            //Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), Constants.TIPODEUNIDAD_MIX);

                            if (stockProdMix == null)
                            {
                                log.Error("No se ha encontrado stock para el producto del producto mix/Blister con ID: " + prodDelMixId);
                                return BadRequest();
                            }

                            //ListaPrecioBlister productoBlisterSegunLista = ventaMayoristaBL.CalcularImporteBlisterSegunCliente(ventaMayoristaInDB.ProductosXVenta[i].ProductoID);

                            //Convertimos la cantidad a gramos (en formato kg)
                            double cantidadEnKG = (Convert.ToDouble(prod.Cantidad) / 10) * ventaMayoristaInDB.ProductosXVenta[i].Cantidad;
                            //double cantidadEnKG = (Convert.ToDouble(ventaMayoristaInDB.ProductosXVenta[i].Cantidad) * (Convert.ToDouble(productoBlisterSegunLista.Gramos) / 1000)); //Convierto a KG
                            stockProdMix.Cantidad = stockProdMix.Cantidad + cantidadEnKG;

                           
                            //stockBL.UpdateStock(stockProdMix);
                            _UOWVentaMayorista.StockRepository.Update(stockProdMix);

                            log.Info("Stock actualizado satisfactoriamente para producto del producto mix/blister con ID: " + prodDelMixId + ". Stock: " + stockProdMix.Cantidad);


                        }

                    }

                }
                else if (ventaMayoristaInDB.ProductosXVenta[i].Producto.EsMix)
                {
                    //Producto Mix
                    var productosMixStock = stockBL.GetListaProductosMixById(prodId);

                    if (productosMixStock == null)
                    {
                        log.Error("No se ha encontrado Stock para el producto mix con ID: " + prodId);
                        return BadRequest();
                    }

                    foreach (var prod in productosMixStock)
                    {

                        int prodDelMixId = prod.ProductoDelMixId.GetValueOrDefault();

                        Stock stockProdMix = _UOWVentaMayorista.StockRepository.GetAll().Where(s => s.ProductoID == prodDelMixId && s.TipoDeUnidadID == tipoUnidadId).SingleOrDefault();
                        //Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), ventaMayoristaInDB.ProductosXVenta[i].TipoDeUnidadID);

                        if (stockProdMix == null)
                        {
                            log.Error("No se ha encontrado stock para el producto del producto mix con ID: " + prodDelMixId);
                            return BadRequest();
                        }

                        double cantidadEnKG = Convert.ToDouble(prod.Cantidad) * Convert.ToDouble(ventaMayoristaInDB.ProductosXVenta[i].Cantidad);
                        stockProdMix.Cantidad = stockProdMix.Cantidad + cantidadEnKG;

                        //stockBL.UpdateStock(stockProdMix);
                        _UOWVentaMayorista.StockRepository.Update(stockProdMix);

                        log.Info("Stock actualizado satisfactoriamente para producto del producto mix con ID: " + prodDelMixId + ". Stock: " + stockProdMix.Cantidad);


                    }

                }
                else
                {

                    if (ventaMayoristaInDB.ProductosXVenta[i].TipoDeUnidadID == Constants.TIPODEUNIDAD_BLISTER)
                    {
                        //Producto Blister
                        Stock stock = _UOWVentaMayorista.StockRepository.GetAll().Where(s => s.ProductoID == prodId && s.TipoDeUnidadID == Constants.PRECIO_X_KG).SingleOrDefault();
                        //Stock stock = stockBL.ValidarStockProducto(ventaMayoristaInDB.ProductosXVenta[i].ProductoID, Constants.PRECIO_X_KG);

                        ListaPrecioBlister productoBlisterSegunLista = ventaMayoristaBL.CalcularImporteBlisterSegunCliente(prodId);

                        double cantidadEnKG = (Convert.ToDouble(productoBlisterSegunLista.Gramos) * Convert.ToDouble(ventaMayoristaInDB.ProductosXVenta[i].Cantidad)) / 1000; //Convierto a KG
                        stock.Cantidad = stock.Cantidad + cantidadEnKG;

                        //stockBL.UpdateStock(stock);
                        _UOWVentaMayorista.StockRepository.Update(stock);

                        log.Info("Stock Actualizado satisfactoriamente para producto blister con ID: " + prodId + ". Stock: " + stock.Cantidad);

                    }
                    else
                    {
                        Stock stock = _UOWVentaMayorista.StockRepository.GetAll().Where(s => s.ProductoID == prodId && s.TipoDeUnidadID == tipoUnidadId).SingleOrDefault();
                        //Stock stock = stockBL.ValidarStockProducto(ventaMayoristaInDB.ProductosXVenta[i].ProductoID, ventaMayoristaInDB.ProductosXVenta[i].TipoDeUnidadID);
                        stock.Cantidad = stock.Cantidad + ventaMayoristaInDB.ProductosXVenta[i].Cantidad;

                        //stockBL.UpdateStock(stock);
                        _UOWVentaMayorista.StockRepository.Update(stock);

                        log.Info("Stock Actualizado satisfactoriamente para producto comun con ID: " + prodId + ". Stock: " + stock.Cantidad);
                    }
                }
            }

            //BORRAMOS PRODUCTOS ASOCIADOS Y LA VENTA MAYORISTA                
            //Borramos Productos asociados
            foreach (var item in ventaMayoristaInDB.ProductosXVenta)
                {                   
                    var productoInDB = _UOWVentaMayorista.ProductosXVentaRepository.GetByID(item.ID);
                    _UOWVentaMayorista.ProductosXVentaRepository.Delete(productoInDB);
                }

            log.Info("Productos de la venta mayorista borrados satisfactoriamente.");

            


            ////Borramos Venta Mayorista
            var ventaABorrar = _UOWVentaMayorista.VentaMayoristaRepository.GetByID(Id);

            //Devolvemos Saldo
            Cliente cliente = _UOWVentaMayorista.ClienteRepository.GetByID(ventaABorrar.ClienteID);

            cliente.Debe = cliente.Debe - ventaABorrar.SumaTotal;

            _UOWVentaMayorista.ClienteRepository.Update(cliente);

            _UOWVentaMayorista.VentaMayoristaRepository.Delete(ventaABorrar);
                

            //}
            //else
            //{
                //Venta No Concretada - No se devuelve stock

                //Borramos Productos asociados
                //foreach (var item in ventaMayoristaInDB.ProductosXVenta)
                //{
                //    var productoInDB = _UOWVentaMayorista.ProductosXVentaRepository.GetByID(item.ID);
                //    _UOWVentaMayorista.ProductosXVentaRepository.Delete(productoInDB);
                //}

                //////Borramos Venta Mayorista
                //var ventaABorrar = _UOWVentaMayorista.VentaMayoristaRepository.GetByID(Id);
                //_UOWVentaMayorista.VentaMayoristaRepository.Delete(ventaABorrar);


            //}

            //Concretamos la operacion
            _UOWVentaMayorista.Save();

            log.Info("Venta Mayorista borrada satisfactoriamente.");




            return Ok();

        }

        [HttpGet]
        [Route("api/ventasmayorista/reporteventasporcliente/{Id}")]
        public IEnumerable<VentaMayoristaDTO> GetAllVentasMayoristaReporte(int Id)
        {

            var ventasMayoristaPorCliente = ventaMayoristaBL.GetAllVentaMayoristaPorCliente(Id);

            return ventasMayoristaPorCliente.Select(Mapper.Map<VentaMayorista, VentaMayoristaDTO>);

        }

        [HttpGet]
        [Route("api/ventasmayorista/reporteproductosvendidos/{Id}")]
        public IEnumerable<ProductoXVentaDTO> GetAllProductosPorVenta (int Id)
        {

            var productosVendidos = productoXVentaBL.GetProductoXVentaByIdProducto(Id);

            //List<ProductoVendido> prods = new List<ProductoVendido>();

            return productosVendidos.Select(Mapper.Map<ProductoXVenta, ProductoXVentaDTO>);

        }

    }
}