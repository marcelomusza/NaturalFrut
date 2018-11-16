using AutoMapper;
using NaturalFrut.App_BLL;
using NaturalFrut.App_BLL.Interfaces;
using NaturalFrut.App_DAL;
using NaturalFrut.DTOs;
using NaturalFrut.Helpers;
using NaturalFrut.Models;
using System;
using System.Collections.Generic;
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
                return BadRequest();

            Proveedor proveedor = proveedorBL.GetProveedorById(compraDTO.ProveedorID);

            if (proveedor == null)
                return BadRequest();

            var compra = Mapper.Map<CompraDTO, Compra>(compraDTO);

            //compraBL.AddCompra(compra);
            _UOWCompra.CompraRepository.Add(compra);

            //Actualizamos el Saldo en base a la Entrega de Efectivo            
             proveedor.Debe = compraDTO.Debe;
            //proveedorBL.UpdateProveedor(proveedor);  
            _UOWCompra.ProveedorRepository.Update(proveedor);

            if (compra.ProductosXCompra != null)
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

                    }
                    else
                    {
                        Stock stockNuevo = new Stock();

                        stockNuevo.ProductoID = item.ProductoID;
                        stockNuevo.TipoDeUnidadID = item.TipoDeUnidadID;
                        stockNuevo.Cantidad = item.Cantidad;

                        //stockBL.AddStock(stockNuevo);
                        _UOWCompra.StockRepository.Add(stockNuevo);


                    }
                    
                }
            }

            //Actualizamos valores
            _UOWCompra.Save();

            return Ok();
        }


        //PUT /api/compra
        [HttpPut]
        public IHttpActionResult UpdateCompra(CompraDTO compraDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var compraInDB = compraBL.GetCompraById(compraDTO.ID);

            if (compraInDB == null)
                return NotFound();

            Proveedor proveedor = proveedorBL.GetProveedorById(compraDTO.ProveedorID);

            //Actualizamos el Saldo en base a la Entrega de Efectivo            
            proveedor.Debe = compraDTO.Debe;
            proveedor.SaldoAfavor = compraDTO.SaldoAfavor;
            //proveedorBL.UpdateProveedor(proveedor);
            _UOWCompra.ProveedorRepository.Update(proveedor);

            //actualizo stock

            if (compraDTO.ProductosXCompra != null)
            {
                foreach (var item in compraDTO.ProductosXCompra)
                {

                    var prdXcompra = productoXCompraBL.GetProductoXCompraById(item.ID);

                    Stock stock = stockBL.ValidarStockProducto(item.ProductoID, item.TipoDeUnidadID);

                    if (prdXcompra.Cantidad > item.Cantidad)
                    {
                        var cantidad = prdXcompra.Cantidad - item.Cantidad;
                        stock.Cantidad = stock.Cantidad - cantidad;
                        //stockBL.UpdateStock(stock);
                        _UOWCompra.StockRepository.Update(stock);
                    }

                    if (item.Cantidad > prdXcompra.Cantidad)
                    {
                        var cantidad = item.Cantidad - prdXcompra.Cantidad;
                        stock.Cantidad = stock.Cantidad + cantidad;
                        //stockBL.UpdateStock(stock);
                        _UOWCompra.StockRepository.Update(stock);
                    }
                }

                foreach (var item in compraDTO.ProductosXCompra)
                {

                    foreach (var item2 in compraInDB.ProductosXCompra)
                    {
                        if (item.ID == item2.ID)
                        {
                            ProductoXCompra prodAActualizar = _UOWCompra.ProductosXCompraRepository.GetByID(item.ID);

                            prodAActualizar.Cantidad = item.Cantidad;
                            prodAActualizar.Importe = item.Importe;
                            prodAActualizar.Descuento = item.Descuento;
                            prodAActualizar.ImporteDescuento = item.ImporteDescuento;
                            prodAActualizar.Iibbbsas = item.Iibbbsas;
                            prodAActualizar.Iibbcaba = item.Iibbcaba;
                            prodAActualizar.Iva = item.Iva;
                            prodAActualizar.PrecioUnitario = item.PrecioUnitario;
                            prodAActualizar.Total = item.Total;
                            prodAActualizar.ProductoID = item.ProductoID;
                            prodAActualizar.CompraID = item.CompraID;
                            prodAActualizar.TipoDeUnidadID = item.TipoDeUnidadID;

                            _UOWCompra.ProductosXCompraRepository.Update(prodAActualizar);

                            //item2.Cantidad = item.Cantidad;
                            //item2.Importe = item.Importe;
                            //item2.Descuento = item.Descuento;
                            //item2.ImporteDescuento = item.ImporteDescuento;
                            //item2.Iibbbsas = item.Iibbbsas;
                            //item2.Iibbcaba = item.Iibbcaba;
                            //item2.Iva = item.Iva;
                            //item2.PrecioUnitario = item.PrecioUnitario;
                            //item2.Total = item.Total;
                            //item2.ProductoID = item.ProductoID;
                            //item2.CompraID = item.CompraID;
                            //item2.TipoDeUnidadID = item.TipoDeUnidadID;

                        }
                    }
                }
            }

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

            _UOWCompra.CompraRepository.Update(compraAActualizar);

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

            return Ok();
        }

        //DELETE /api/compra/1
        [HttpDelete]
        public IHttpActionResult DeleteProductoCompra(BorrarProdCompraDTO prodCompra)
        {
                        
            var productoInDB = productoXCompraBL.GetProductoXCompraIndividualById(prodCompra);
            

            if (productoInDB == null)
                return NotFound();

            //Referenciamos producto que borraremos con UOW
            var prodABorrar = _UOWCompra.ProductosXCompraRepository.GetByID(productoInDB.ID);

            var importeTotalProducto = productoInDB.Total;

            

            //restamos stock

            Producto producto = productoBL.GetProductoById(prodCompra.ProductoID);            
            Stock stock = stockBL.ValidarStockProducto(prodCompra.ProductoID, prodCompra.TipoDeUnidadID);

            if (stock != null)
            {
                stock.Cantidad = stock.Cantidad - prodCompra.Cantidad;
                //stockBL.UpdateStock(stock);
                _UOWCompra.StockRepository.Update(stock);

            }

            //Actualizamos el total de la venta
            //var compraInDB = compraBL.GetCompraById(prodCompra.CompraID);
            Compra compraInDB = _UOWCompra.CompraRepository.GetByID(prodCompra.CompraID);

            compraInDB.TotalGastos = compraInDB.TotalGastos - importeTotalProducto;

            //compraBL.UpdateCompra(compraInDB);
            _UOWCompra.CompraRepository.Update(compraInDB);

            //productoXCompraBL.RemoveProductoXCompra(productoInDB);
            _UOWCompra.ProductosXCompraRepository.Delete(prodABorrar);


            _UOWCompra.Save();

            return Ok();

        }

        //DELETE /api/ventasMayorista/1
        [HttpDelete]
        [Route("api/compra/deletecompra/{Id}")]
        public IHttpActionResult DeleteCompra(int Id)
        {

            var compraInDB = compraBL.GetCompraById(Id);

            if (compraInDB == null)
                return NotFound();

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
                    }
                }


                //BORRAMOS PRODUCTOS ASOCIADOS Y LA VENTA MAYORISTA                
                //Borramos Productos asociados
                foreach (var item in compraInDB.ProductosXCompra)
                {
                    var productoInDB = _UOWCompra.ProductosXCompraRepository.GetByID(item.ID);
                    _UOWCompra.ProductosXCompraRepository.Delete(productoInDB);
                }
            }


        
           

            ////Borramos Venta Mayorista
            var compraABorrar = _UOWCompra.CompraRepository.GetByID(Id);
            _UOWCompra.CompraRepository.Delete(compraABorrar);


            //Concretamos la operacion
            _UOWCompra.Save();

            return Ok();

        }

        [HttpGet]
        [Route("api/compra/reporteprodporproveedor/{Id}")]
        public IEnumerable<ProductoXCompraDTO> GetAllProductosPorProveedor(int Id)
        {

            var prodXProv = productoXCompraBL.GetProductoXCompraByIdProveedor(Id);

            //List<ProductoVendido> prods = new List<ProductoVendido>();

            return prodXProv.Select(Mapper.Map<ProductoXCompra, ProductoXCompraDTO>);

        }

    }
}