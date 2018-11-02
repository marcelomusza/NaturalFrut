using AutoMapper;
using NaturalFrut.App_BLL;
using NaturalFrut.App_BLL.Interfaces;
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

            compraBL.AddCompra(compra);

            //Actualizamos el Saldo en base a la Entrega de Efectivo            
             proveedor.Debe = compraDTO.Debe;
             proveedorBL.UpdateProveedor(proveedor);  

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
                        stockBL.UpdateStock(stock);

                    }
                    else
                    {
                        Stock stockNuevo = new Stock();

                        stockNuevo.ProductoID = item.ProductoID;
                        stockNuevo.TipoDeUnidadID = item.TipoDeUnidadID;
                        stockNuevo.Cantidad = item.Cantidad;

                        stockBL.AddStock(stockNuevo);


                    }
                    
                }
            }
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
            proveedorBL.UpdateProveedor(proveedor);

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
                        stockBL.UpdateStock(stock);
                    }

                    if (item.Cantidad > prdXcompra.Cantidad)
                    {
                        var cantidad = item.Cantidad - prdXcompra.Cantidad;
                        stock.Cantidad = stock.Cantidad + cantidad;
                        stockBL.UpdateStock(stock);
                    }
                }

                foreach (var item in compraDTO.ProductosXCompra)
                {

                    foreach (var item2 in compraInDB.ProductosXCompra)
                    {
                        if (item.ID == item2.ID)
                        {
                            item2.Cantidad = item.Cantidad;
                            item2.Importe = item.Importe;
                            item2.Descuento = item.Descuento;
                            item2.ImporteDescuento = item.ImporteDescuento;
                            item2.Iibbbsas = item.Iibbbsas;
                            item2.Iibbcaba = item.Iibbcaba;
                            item2.Iva = item.Iva;
                            item2.PrecioUnitario = item.PrecioUnitario;
                            item2.Total = item.Total;
                            item2.ProductoID = item.ProductoID;
                            item2.CompraID = item.CompraID;
                            item2.TipoDeUnidadID = item.TipoDeUnidadID;

                        }
                    }
                }
            }          

            

            compraInDB.Factura = compraDTO.Factura;
            compraInDB.NoConcretado = compraDTO.NoConcretado;
            compraInDB.TipoFactura = compraDTO.TipoFactura;
            compraInDB.SumaTotal = compraDTO.SumaTotal;
            compraInDB.DescuentoPorc = compraDTO.DescuentoPorc;
            compraInDB.Descuento = compraDTO.Descuento;
            compraInDB.Subtotal = compraDTO.Subtotal;
            compraInDB.ImporteNoGravado = compraDTO.ImporteNoGravado;
            compraInDB.Iva = compraDTO.Iva;
            compraInDB.ImporteIva = compraDTO.ImporteIva;
            compraInDB.Iibbbsas = compraDTO.Iibbbsas;
            compraInDB.ImporteIibbbsas = compraDTO.ImporteIibbbsas;
            compraInDB.Iibbcaba = compraDTO.Iibbcaba;
            compraInDB.ImporteIibbcaba = compraDTO.ImporteIibbcaba;
            compraInDB.PercIva = compraDTO.PercIva;
            compraInDB.ImportePercIva = compraDTO.ImportePercIva;
            compraInDB.Total = compraDTO.Total;
            compraInDB.TotalGastos = compraDTO.TotalGastos;

            compraBL.UpdateCompra(compraInDB);

            return Ok();
        }

        //DELETE /api/compra/1
        [HttpDelete]
        public IHttpActionResult DeleteProductoCompra(BorrarProdCompraDTO prodCompra)
        {
                        
            var productoInDB = productoXCompraBL.GetProductoXCompraIndividualById(prodCompra);            

            if (productoInDB == null)
                return NotFound();

            var importeTotalProducto = productoInDB.Total;

            

            //restamos stock

            Producto producto = productoBL.GetProductoById(prodCompra.ProductoID);
            Stock stock = stockBL.ValidarStockProducto(prodCompra.ProductoID, prodCompra.TipoDeUnidadID);

            if (stock != null)
            {
                stock.Cantidad = stock.Cantidad - prodCompra.Cantidad;
                stockBL.UpdateStock(stock);

            }
            productoXCompraBL.RemoveProductoXCompra(productoInDB);
            //Actualizamos el total de la venta
            var compraInDB = compraBL.GetCompraById(prodCompra.CompraID);

            compraInDB.TotalGastos = compraInDB.TotalGastos - importeTotalProducto;

            compraBL.UpdateCompra(compraInDB);

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