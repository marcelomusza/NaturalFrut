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
    public class CompraController : ApiController
    {

        private readonly CompraLogic compraBL;
        private readonly StockLogic stockBL;
        //private readonly ClienteLogic clienteBL;
        private readonly ProveedorLogic proveedorBL;
        private readonly CommonLogic clasificacionBL;
        private readonly ProductoLogic productoBL;
        private readonly ProductoXCompraLogic productoXCompraBL;

        private UOWCompra _UOWCompra = new UOWCompra();

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public CompraController(IRepository<Compra> CompraRepo,
            IRepository<Stock> StockRepo,
            //IRepository<Cliente> ClienteRepo,
            IRepository<Proveedor> ProveedorRepo,
            IRepository<Clasificacion> ClasificacionRepo,
            IRepository<Producto> ProductoRepo,
            //IRepository<ListaPrecioBlister> ListaPrecioBlisterRepo,
            IRepository<ProductoXCompra> ProductoXCompraRepo)
            //IRepository<ProductoMix> ProductoMixRepo)
        {
            compraBL = new CompraLogic(CompraRepo);
            stockBL = new StockLogic(StockRepo);
            //clienteBL = new ClienteLogic(ClienteRepo);
            proveedorBL = new ProveedorLogic(ProveedorRepo);
            clasificacionBL = new CommonLogic(ClasificacionRepo);
            productoBL = new ProductoLogic(ProductoRepo);
            productoXCompraBL = new ProductoXCompraLogic(ProductoXCompraRepo);
        }
        
        //GET /api/compra
        public IEnumerable<CompraDTO> GetCompra()
        {
            var compra = compraBL.GetAllCompra();

            return compra.Select(Mapper.Map<Compra, CompraDTO>);
        }


        //POST /api/compra
        [HttpPost]
        public IHttpActionResult Compra(CompraDTO compraDTO)
        {
            if (!ModelState.IsValid)
            {
                log.Error("Formulario con datos incorrectos o insuficientes.");
                return BadRequest();
            }

            Proveedor proveedor = proveedorBL.GetProveedorById(compraDTO.ProveedorID);

            if (proveedor == null)
            {
                log.Error("No se ha encontrado Proveedor con el ID: " + compraDTO.ProveedorID);
                return BadRequest();
            }
                

            var compra = Mapper.Map<CompraDTO, Compra>(compraDTO);

            //compraBL.AddCompra(compra);
            _UOWCompra.CompraRepository.Add(compra);

            //log.Info("Compra. Viejo Saldo Proveedor: " + compraDTO.Debe);

            //Actualizamos el Saldo en base a la Entrega de Efectivo            
            // proveedor.Debe = compraDTO.Debe;
            //proveedorBL.UpdateProveedor(proveedor); 
            proveedor.Debe = compraDTO.Debe;
            proveedor.SaldoAfavor = compraDTO.SaldoAfavor;
            _UOWCompra.ProveedorRepository.Update(proveedor);

            log.Info("Compra. Nuevo Saldo Proveedor: " + proveedor.Debe);

            

            if (compra.ProductosXCompra.Count != 0)
            {

                //Una vez cargada la venta, actualizamos Stock
                foreach (var item in compra.ProductosXCompra)
                {

                    Producto producto = productoBL.GetProductoById(item.ProductoID);


                    Stock stock = stockBL.ValidarStockProducto(item.ProductoID, item.TipoDeUnidadID);

                    if(stock != null)
                    {
                        stock.Cantidad = stock.Cantidad + item.Cantidad;
                        //stockBL.UpdateStock(stock);
                        _UOWCompra.StockRepository.Update(stock);

                        log.Info("Stock actualizado o creado para producto con ID: " + item.ProductoID + ". Nueva Cantidad: " + stock.Cantidad);

                    }
                    else
                    {
                        Stock stockNuevo = new Stock();

                        stockNuevo.ProductoID = item.ProductoID;
                        stockNuevo.TipoDeUnidadID = item.TipoDeUnidadID;
                        stockNuevo.Cantidad = item.Cantidad;

                        //stockBL.AddStock(stockNuevo);
                        _UOWCompra.StockRepository.Add(stockNuevo);

                        log.Info("Stock actualizado o creado para producto con ID: " + item.ProductoID + ". Nueva Cantidad: " + stockNuevo.Cantidad);


                    }

                    

                }
            }

            //Actualizamos valores
            _UOWCompra.Save();

            log.Info("Compra generada satisfactoriamente.");

            return Ok();
        }


        //PUT /api/compra
        [HttpPut]
        public IHttpActionResult UpdateCompra(CompraDTO compraDTO)
        {
            if (!ModelState.IsValid)
            {
                log.Error("Formulario con datos incorrectos o insuficientes.");
                return BadRequest();
            }

            var compraInDB = compraBL.GetCompraById(compraDTO.ID);

            if (compraInDB == null)
            {
                log.Error("No se encontró compra existente con el ID: " + compraDTO.ID);
                return NotFound();
            }

            Proveedor proveedor = proveedorBL.GetProveedorById(compraDTO.ProveedorID);

            log.Info("Compra. Viejo Saldo Proveedor: " + compraDTO.Debe);

            //Actualizamos el Saldo en base a la Entrega de Efectivo            
            proveedor.Debe = compraDTO.Debe;
            proveedor.SaldoAfavor = compraDTO.SaldoAfavor;
            //proveedorBL.UpdateProveedor(proveedor);
            _UOWCompra.ProveedorRepository.Update(proveedor);

            log.Info("Compra. Nuevo Saldo Proveedor: " + proveedor.Debe);

            ////actualizo stock
            //if (compraDTO.ProductosXCompra != null)
            //{
            //    foreach (var item in compraDTO.ProductosXCompra)
            //    {

            //        var prdXcompra = productoXCompraBL.GetProductoXCompraById(item.ID);

            //        Stock stock = stockBL.ValidarStockProducto(item.ProductoID, item.TipoDeUnidadID);

            //        if (prdXcompra.Cantidad > item.Cantidad)
            //        {
            //            var cantidad = prdXcompra.Cantidad - item.Cantidad;
            //            stock.Cantidad = stock.Cantidad - cantidad;
            //            //stockBL.UpdateStock(stock);
            //            _UOWCompra.StockRepository.Update(stock);
            //        }

            //        if (item.Cantidad > prdXcompra.Cantidad)
            //        {
            //            var cantidad = item.Cantidad - prdXcompra.Cantidad;
            //            stock.Cantidad = stock.Cantidad + cantidad;
            //            //stockBL.UpdateStock(stock);
            //            _UOWCompra.StockRepository.Update(stock);
            //        }

            //        log.Info("Stock actualizado para producto con ID: " + item.ProductoID + ". Nueva Cantidad: " + stock.Cantidad);
            //    }

            //    foreach (var item in compraDTO.ProductosXCompra)
            //    {

            //        foreach (var item2 in compraInDB.ProductosXCompra)
            //        {
            //            if (item.ID == item2.ID)
            //            {
            //                ProductoXCompra prodAActualizar = _UOWCompra.ProductosXCompraRepository.GetByID(item.ID);

            //                prodAActualizar.Cantidad = item.Cantidad;
            //                prodAActualizar.Importe = item.Importe;
            //                prodAActualizar.Descuento = item.Descuento;
            //                prodAActualizar.ImporteDescuento = item.ImporteDescuento;
            //                prodAActualizar.Iibbbsas = item.Iibbbsas;
            //                prodAActualizar.Iibbcaba = item.Iibbcaba;
            //                prodAActualizar.Iva = item.Iva;
            //                prodAActualizar.PrecioUnitario = item.PrecioUnitario;
            //                prodAActualizar.Total = item.Total;
            //                prodAActualizar.ProductoID = item.ProductoID;
            //                prodAActualizar.CompraID = item.CompraID;
            //                prodAActualizar.TipoDeUnidadID = item.TipoDeUnidadID;

            //                _UOWCompra.ProductosXCompraRepository.Update(prodAActualizar);

            //                log.Info("Datos actualizados para producto con ID: " + item2.ProductoID);

            //                //item2.Cantidad = item.Cantidad;
            //                //item2.Importe = item.Importe;
            //                //item2.Descuento = item.Descuento;
            //                //item2.ImporteDescuento = item.ImporteDescuento;
            //                //item2.Iibbbsas = item.Iibbbsas;
            //                //item2.Iibbcaba = item.Iibbcaba;
            //                //item2.Iva = item.Iva;
            //                //item2.PrecioUnitario = item.PrecioUnitario;
            //                //item2.Total = item.Total;
            //                //item2.ProductoID = item.ProductoID;
            //                //item2.CompraID = item.CompraID;
            //                //item2.TipoDeUnidadID = item.TipoDeUnidadID;

            //            }
            //        }
            //    }
            //}

            Compra compraAActualizar = _UOWCompra.CompraRepository.GetByID(compraInDB.ID);

            compraAActualizar.Factura = compraDTO.Factura;
            compraAActualizar.NoConcretado = compraDTO.NoConcretado;
            compraAActualizar.TipoFactura = compraDTO.TipoFactura;
            compraAActualizar.Local = compraDTO.Local;
            compraAActualizar.SumaTotal = compraDTO.SumaTotal;
            compraAActualizar.DescuentoPorc = compraDTO.DescuentoPorc;
            compraAActualizar.Descuento = compraDTO.Descuento;
            compraAActualizar.Subtotal = compraDTO.Subtotal;
            compraAActualizar.ImporteNoGravado = compraDTO.ImporteNoGravado;
            compraAActualizar.Iva = compraDTO.Iva;
            compraAActualizar.ImporteIva = compraDTO.ImporteIva;
            compraAActualizar.Iibbbsas = compraDTO.Iibbbsas;
            compraAActualizar.ImporteIibbbsas = compraDTO.ImporteIibbbsas;
            compraAActualizar.Iibbcaba = compraDTO.Iibbcaba;
            compraAActualizar.ImporteIibbcaba = compraDTO.ImporteIibbcaba;
            compraAActualizar.PercIva = compraDTO.PercIva;
            compraAActualizar.ImportePercIva = compraDTO.ImportePercIva;
            compraAActualizar.Total = compraDTO.Total;
            compraAActualizar.TotalGastos = compraDTO.TotalGastos;
            compraAActualizar.Fecha = DateTime.Parse(compraDTO.Fecha);

            _UOWCompra.CompraRepository.Update(compraAActualizar);



            //Borramos todos los productos de la compra, para luego agregarlos nuevamente junto con sus actualizados
            bool borrado = false;
            borrado = DeleteProductosParaUpdate(compraInDB);

            if (!borrado)
            {
                log.Error("Se ha producido un error intentando borrar los productos de la venta al momento de actualizar. Venta ID: " + compraInDB.ID);
                return BadRequest();
            }

            //Agregamos nuevamente los productos a la compra, actualizados

            if(compraDTO.ProductosXCompra != null)
            {
                foreach (var prodCompra in compraDTO.ProductosXCompra)
                {
                    //UPDATE PRODUCTOS DE VENTA MAYORISTA
                    _UOWCompra.ProductosXCompraRepository.Add(prodCompra);
                }

                //Una vez cargada la venta, actualizamos Stock
                foreach (var item in compraDTO.ProductosXCompra)
                {

                    //Producto producto = productoBL.GetProductoById(item.ProductoID);


                    //Stock stock = stockBL.ValidarStockProducto(item.ProductoID, item.TipoDeUnidadID);
                    Stock stock = _UOWCompra.StockRepository
                                    .GetAll()
                                    .Where(s => s.ProductoID == item.ProductoID && s.TipoDeUnidadID == item.TipoDeUnidadID)
                                    .SingleOrDefault();

                    if (stock != null)
                    {
                        stock.Cantidad = stock.Cantidad + item.Cantidad;
                        //stockBL.UpdateStock(stock);
                        _UOWCompra.StockRepository.Update(stock);

                        log.Info("Stock actualizado o creado para producto con ID: " + item.ProductoID + ". Nueva Cantidad: " + stock.Cantidad);

                    }
                    else
                    {
                        Stock stockNuevo = new Stock();

                        stockNuevo.ProductoID = item.ProductoID;
                        stockNuevo.TipoDeUnidadID = item.TipoDeUnidadID;
                        stockNuevo.Cantidad = item.Cantidad;

                        //stockBL.AddStock(stockNuevo);
                        _UOWCompra.StockRepository.Add(stockNuevo);

                        log.Info("Stock actualizado o creado para producto con ID: " + item.ProductoID + ". Nueva Cantidad: " + stockNuevo.Cantidad);


                    }



                }

            }




            //compraInDB.Factura = compraDTO.Factura;
            //compraInDB.NoConcretado = compraDTO.NoConcretado;
            //compraInDB.TipoFactura = compraDTO.TipoFactura;
            //compraInDB.SumaTotal = compraDTO.SumaTotal;
            //compraInDB.DescuentoPorc = compraDTO.DescuentoPorc;
            //compraInDB.Descuento = compraDTO.Descuento;
            //compraInDB.Subtotal = compraDTO.Subtotal;
            //compraInDB.ImporteNoGravado = compraDTO.ImporteNoGravado;
            //compraInDB.Iva = compraDTO.Iva;
            //compraInDB.ImporteIva = compraDTO.ImporteIva;
            //compraInDB.Iibbbsas = compraDTO.Iibbbsas;
            //compraInDB.ImporteIibbbsas = compraDTO.ImporteIibbbsas;
            //compraInDB.Iibbcaba = compraDTO.Iibbcaba;
            //compraInDB.ImporteIibbcaba = compraDTO.ImporteIibbcaba;
            //compraInDB.PercIva = compraDTO.PercIva;
            //compraInDB.ImportePercIva = compraDTO.ImportePercIva;
            //compraInDB.Total = compraDTO.Total;
            //compraInDB.TotalGastos = compraDTO.TotalGastos;

            //compraBL.UpdateCompra(compraInDB);

            //Actualizamos la operación
            _UOWCompra.Save();

            log.Info("Compra actualizada satisfactoriamente. ID: " + compraAActualizar.ID);

            return Ok();
        }
        [HttpGet]
        [Route("api/productos/DeleteProductosParaUpdate(")]
        public bool DeleteProductosParaUpdate(Compra compra)
        {
            var productosCompra = _UOWCompra.ProductosXCompraRepository.GetAll().Where(p => p.CompraID == compra.ID).ToList();

            //Iteramos todos los productos que vamos a borrar
            foreach (var prodCompra in productosCompra)
            {

                BorrarProdCompraDTO idsDeCompra = new BorrarProdCompraDTO
                {
                    ProductoID = prodCompra.ProductoID,
                    CompraID = compra.ID
                };

                //var productoInDB = productoXVentaBL.GetProductoXVentaIndividualById(idsDeVenta);
                var productoInDB = _UOWCompra.ProductosXCompraRepository.GetAll()
                    .Include(p => p.Producto)
                    .Include(t => t.TipoDeUnidad)
                    .Include(v => v.Compra)
                    .Where(c => c.CompraID == prodCompra.CompraID && c.ProductoID == prodCompra.ProductoID).SingleOrDefault();

                if (productoInDB == null)
                {
                    log.Error("No se ha encontrado Producto en la base de datos con ID:" + productoInDB);
                    return false;
                }

                //Referenciamos producto que borraremos con UOW
                var prodABorrar = _UOWCompra.ProductosXCompraRepository.GetByID(productoInDB.ID);

                //Devolvemos Stock
                //Producto producto = productoBL.GetProductoById(prodCompra.ProductoID);
                //Stock stock = stockBL.ValidarStockProducto(prodCompra.ProductoID, prodCompra.TipoDeUnidadID);
                Stock stock = _UOWCompra.StockRepository
                                .GetAll()
                                .Where(s => s.ProductoID == prodCompra.ProductoID && s.TipoDeUnidadID == prodCompra.TipoDeUnidadID)
                                .SingleOrDefault();

                log.Info("Producto a Borrar con ID: " + productoInDB.ID);

                if (stock != null)
                {
                    log.Info("Stock Producto a Eliminar: " + stock.Cantidad);

                    stock.Cantidad = stock.Cantidad - prodCompra.Cantidad;
                    //stockBL.UpdateStock(stock);
                    _UOWCompra.StockRepository.Update(stock);

                    log.Info("Stock Producto Actualizado: " + stock.Cantidad);

                }

                _UOWCompra.ProductosXCompraRepository.Delete(prodABorrar);
                //_UOWVentaMayorista.Save();

                log.Info("Producto de la compra borrado exitosamente.");


            }

            //Si el borrado total de productos fue exitoso, devolvemos true
            return true;

        }



        //DELETE /api/compra/1
        [HttpDelete]
        public IHttpActionResult DeleteProductoCompra(BorrarProdCompraDTO prodCompra)
        {
                        
            var productoInDB = productoXCompraBL.GetProductoXCompraIndividualById(prodCompra);
            

            if (productoInDB == null)
            {
                log.Error("Producto no encontrado en la base de datos con ID: " + prodCompra.ProductoID);
                return NotFound();
            }

            //Referenciamos producto que borraremos con UOW
            var prodABorrar = _UOWCompra.ProductosXCompraRepository.GetByID(productoInDB.ID);

            var importeTotalProducto = productoInDB.Total;

            //restamos stock

            Producto producto = productoBL.GetProductoById(prodCompra.ProductoID);            
            Stock stock = stockBL.ValidarStockProducto(prodCompra.ProductoID, prodCompra.TipoDeUnidadID);

            log.Info("Producto a Borrar con ID: " + producto.ID);

            if (stock != null)
            {
                log.Info("Stock Producto a Eliminar: " + stock.Cantidad);

                stock.Cantidad = stock.Cantidad - prodCompra.Cantidad;
                //stockBL.UpdateStock(stock);
                _UOWCompra.StockRepository.Update(stock);

                log.Info("Stock Producto Actualizado: " + stock.Cantidad);

            }

            //Actualizamos el total de la venta
            //var compraInDB = compraBL.GetCompraById(prodCompra.CompraID);
            Compra compraInDB = _UOWCompra.CompraRepository.GetByID(prodCompra.CompraID);

            compraInDB.TotalGastos = compraInDB.TotalGastos - importeTotalProducto;

            log.Info("Total gastos de Compra ID " + compraInDB.ID + " actualizado. Nuevo valor: " + compraInDB.TotalGastos);

            //compraBL.UpdateCompra(compraInDB);
            _UOWCompra.CompraRepository.Update(compraInDB);

            //productoXCompraBL.RemoveProductoXCompra(productoInDB);
            _UOWCompra.ProductosXCompraRepository.Delete(prodABorrar);

            _UOWCompra.Save();

            log.Info("Producto eliminado de la compra satisfactoriamente.");

            return Ok();

        }

        //DELETE /api/ventasMayorista/1
        [HttpDelete]
        [Route("api/compra/deletecompra/{Id}")]
        public IHttpActionResult DeleteCompra(int Id)
        {

            var compraInDB = compraBL.GetCompraById(Id);

            if (compraInDB == null)
            {
                log.Error("Compra no encontrada en la base de datos con ID: " + Id);
                return NotFound();
            }

            if(compraInDB.ProductosXCompra != null)
            {
                //RESTAMOS STOCK
                for (int i = 0; i < compraInDB.ProductosXCompra.Count; i++)
                {

                    int prodId = compraInDB.ProductosXCompra[i].ProductoID;
                    int tipoUnidadId = compraInDB.ProductosXCompra[i].TipoDeUnidadID;


                    {
                        Stock stock = _UOWCompra.StockRepository.GetAll().Where(s => s.ProductoID == prodId && s.TipoDeUnidadID == tipoUnidadId).SingleOrDefault();
                        //Stock stock = stockBL.ValidarStockProducto(ventaMayoristaInDB.ProductosXVenta[i].ProductoID, ventaMayoristaInDB.ProductosXVenta[i].TipoDeUnidadID);
                        stock.Cantidad = stock.Cantidad - compraInDB.ProductosXCompra[i].Cantidad;

                        //stockBL.UpdateStock(stock);
                        _UOWCompra.StockRepository.Update(stock);

                        log.Info("Stock actualizado para Producto con ID: " + prodId + ". Nuevo valor: " + stock.Cantidad);
                    }
                }


                //BORRAMOS PRODUCTOS ASOCIADOS Y LA VENTA MAYORISTA                
                //Borramos Productos asociados
                foreach (var item in compraInDB.ProductosXCompra)
                {
                    var productoInDB = _UOWCompra.ProductosXCompraRepository.GetByID(item.ID);
                    _UOWCompra.ProductosXCompraRepository.Delete(productoInDB);

                    log.Info("Producto borrado de la compra con ID: " + item.ID);
                }
            }


        
           

            ////Borramos Venta Mayorista
            var compraABorrar = _UOWCompra.CompraRepository.GetByID(Id);
            _UOWCompra.CompraRepository.Delete(compraABorrar);


            //Concretamos la operacion
            _UOWCompra.Save();

            log.Info("Compra eliminada satisfactoriamente.");

            return Ok();

        }

        [HttpGet]
        [Route("api/compra/reportecomprasporproveedor/{Id}")]
        public IEnumerable<ProductoXCompraDTO> GetAllComprasPorProveedor(int Id)
        {

            var prodXProv = productoXCompraBL.GetProductoXCompraByIdProveedor(Id);

            //List<ProductoVendido> prods = new List<ProductoVendido>();

            return prodXProv.Select(Mapper.Map<ProductoXCompra, ProductoXCompraDTO>);

        }

        [HttpGet]
        [Route("api/compra/reporteprodporproveedor/{Id}")]
        public IEnumerable<ProductoXCompraDTO> GetAllProductosPorProveedor(int Id)
        {

            var prodXProv = productoXCompraBL.GetProductoXCompraByIdProducto(Id);

            //List<ProductoVendido> prods = new List<ProductoVendido>();

            return prodXProv.Select(Mapper.Map<ProductoXCompra, ProductoXCompraDTO>);

        }

    }
}