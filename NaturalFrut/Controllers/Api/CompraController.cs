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

            //Cliente cliente = clienteBL.GetClienteById(ventaMayoristaDTO.ClienteID);

            /*if (cliente == null)
                return BadRequest();*/

            var compra = Mapper.Map<CompraDTO, Compra>(compraDTO);

            compraBL.AddCompra(compra);

            //Actualizamos el Saldo en base a la Entrega de Efectivo            
            /* cliente.Saldo = ventaMayorista.SumaTotal - ventaMayorista.EntregaEfectivo;
             clienteBL.UpdateCliente(cliente);  */

            if (!compra.NoConcretado)
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
                        stock.Cantidad = item.Cantidad;

                        stockBL.AddStock(stockNuevo);


                    }
                    
                }
            }
            return Ok();
        }


        //PUT /api/compra
        [HttpPut]
        public IHttpActionResult UpdateCompra(CompraUpdateDTO compraUpdateDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            //var proveedor = proveedorBL.GetProveedorById(compraUpdateDTO.Compra.ProveedorID);

            //if (proveedor == null)
            //    return BadRequest();

            var compraInDB = compraBL.GetCompraById(compraUpdateDTO.Compra.ID);

            List<float> cantProds = new List<float>();

            foreach (var prod in compraInDB.ProductosXCompra)
            {
                cantProds.Add(prod.Cantidad);
            }


            if (compraInDB == null)
                return NotFound();



            //Update para los campos de VentaMayorista
            compraInDB.Factura = compraUpdateDTO.Compra.Factura;
            compraInDB.TipoFactura = compraUpdateDTO.Compra.TipoFactura;
            compraInDB.SumaTotal = compraUpdateDTO.Compra.SumaTotal;
            compraInDB.DescuentoPorc = compraUpdateDTO.Compra.DescuentoPorc;
            compraInDB.Descuento = compraUpdateDTO.Compra.Descuento;
            compraInDB.Subtotal = compraUpdateDTO.Compra.Subtotal;
            compraInDB.ImporteNoGravado = compraUpdateDTO.Compra.ImporteNoGravado;
            compraInDB.Iva = compraUpdateDTO.Compra.Iva;
            compraInDB.ImporteIva = compraUpdateDTO.Compra.ImporteIva;
            compraInDB.Iibbbsas = compraUpdateDTO.Compra.Iibbbsas;
            compraInDB.ImporteIibbbsas = compraUpdateDTO.Compra.ImporteIibbbsas;
            compraInDB.Iibbcaba = compraUpdateDTO.Compra.Iibbcaba;
            compraInDB.ImporteIibbcaba = compraUpdateDTO.Compra.ImporteIibbcaba;
            compraInDB.PercIva = compraUpdateDTO.Compra.PercIva;
            compraInDB.ImportePercIva = compraUpdateDTO.Compra.ImportePercIva;
            compraInDB.Total = compraUpdateDTO.Compra.Total;

            //Update para los campos de sus ProductosXVenta asociados
            for (int i = 0; i < compraInDB.ProductosXCompra.Count; i++)
            {
                compraInDB.ProductosXCompra[i].Cantidad = compraUpdateDTO.Compra.ProductosXCompra[i].Cantidad;
                compraInDB.ProductosXCompra[i].Importe = compraUpdateDTO.Compra.ProductosXCompra[i].Importe;
                compraInDB.ProductosXCompra[i].Descuento = compraUpdateDTO.Compra.ProductosXCompra[i].Descuento;
                compraInDB.ProductosXCompra[i].ImporteDescuento = compraUpdateDTO.Compra.ProductosXCompra[i].ImporteDescuento;
                compraInDB.ProductosXCompra[i].Iibbbsas = compraUpdateDTO.Compra.ProductosXCompra[i].Iibbbsas;
                compraInDB.ProductosXCompra[i].Iibbcaba = compraUpdateDTO.Compra.ProductosXCompra[i].Iibbcaba;
                compraInDB.ProductosXCompra[i].Iva = compraUpdateDTO.Compra.ProductosXCompra[i].Iva;
                compraInDB.ProductosXCompra[i].PrecioUnitario = compraUpdateDTO.Compra.ProductosXCompra[i].PrecioUnitario;
                compraInDB.ProductosXCompra[i].Total = compraUpdateDTO.Compra.ProductosXCompra[i].Total;
            }

            compraBL.UpdateCompra(compraInDB);

            //Actualizamos el Saldo en base a la Entrega de Efectivo            
            //cliente.Saldo = ventaMayoristaInDB.SumaTotal - ventaMayoristaInDB.EntregaEfectivo;
            //clienteBL.UpdateCliente(cliente);

            //if (ventaMayoristaInDB.NoConcretado)
            //{
            //    //Logica para Ventas No Concretadas -- Devolución de Stock
            //}
            //else
            //{
            //    //Una vez actualizada la venta, actualizamos Stock

            //    for (int i = 0; i < ventaMayoristaInDB.ProductosXVenta.Count; i++)
            //    {

            //        //Verificamos si se trata de un producto MIX o no
            //        if (ventaMayoristaInDB.ProductosXVenta[i].Producto.EsMix)
            //        {
            //            //Producto Mix
            //            var productosMixStock = stockBL.GetListaProductosMixById(ventaMayoristaInDB.ProductosXVenta[i].ProductoID);

            //            if (productosMixStock == null)
            //                return BadRequest();

            //            foreach (var prod in productosMixStock)
            //            {
            //                Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), ventaMayoristaInDB.ProductosXVenta[i].TipoDeUnidadID);

            //                if (stockProdMix == null)
            //                    return BadRequest();

            //                //Realizo operacion inversa para convertir valor neto en parcial para cada producto
            //                var diferencia = cantProds[i] - ventaMayoristaInDB.ProductosXVenta[i].Cantidad;
            //                if (diferencia < 0)
            //                    diferencia = diferencia * (-1);

            //                var diferenciaMix = prod.Cantidad * diferencia;


            //                if (cantProds[i] < ventaMayoristaInDB.ProductosXVenta[i].Cantidad)
            //                {
            //                    //Cantidad de productos actual es superior a la venta original, restar stock
            //                    stockProdMix.Cantidad = stockProdMix.Cantidad - diferenciaMix;

            //                    stockBL.UpdateStock(stockProdMix);

            //                }
            //                if (cantProds[i] > ventaMayoristaInDB.ProductosXVenta[i].Cantidad)
            //                {
            //                    //Devolver stock                       
            //                    stockProdMix.Cantidad = stockProdMix.Cantidad + diferenciaMix;

            //                    stockBL.UpdateStock(stockProdMix);
            //                }
            //            }


            //        }
            //        else
            //        {
            //            //Producto Comun
            //            Stock stock = stockBL.ValidarStockProducto(ventaMayoristaInDB.ProductosXVenta[i].ProductoID, ventaMayoristaInDB.ProductosXVenta[i].TipoDeUnidadID);

            //            var diferencia = cantProds[i] - ventaMayoristaInDB.ProductosXVenta[i].Cantidad;
            //            if (diferencia < 0)
            //                diferencia = diferencia * (-1);

            //            if (cantProds[i] < ventaMayoristaInDB.ProductosXVenta[i].Cantidad)
            //            {
            //                //Cantidad de productos actual es superior a la venta original, restar stock
            //                //Actualizamos dependiendo si es un producto Blister o Comun                       

            //                if (ventaMayoristaInDB.ProductosXVenta[i].TipoDeUnidadID == Constants.TIPODEUNIDAD_BLISTER)
            //                {
            //                    ListaPrecioBlister productoBlisterSegunLista = ventaMayoristaBL.CalcularImporteBlisterSegunCliente(ventaMayoristaInDB.ProductosXVenta[i].ProductoID);

            //                    double cantidadEnKG = (Convert.ToDouble(productoBlisterSegunLista.Gramos) * Convert.ToDouble(diferencia) / 1000); //Convierto a KG
            //                    stock.Cantidad = stock.Cantidad - cantidadEnKG;

            //                }
            //                else
            //                    stock.Cantidad = stock.Cantidad - diferencia;

            //                stockBL.UpdateStock(stock);

            //            }
            //            if (cantProds[i] > ventaMayoristaInDB.ProductosXVenta[i].Cantidad)
            //            {
            //                //Devolver stock                       
            //                //Actualizamos dependiendo si es un producto Blister o Comun
            //                if (ventaMayoristaInDB.ProductosXVenta[i].TipoDeUnidadID == Constants.TIPODEUNIDAD_BLISTER)
            //                {
            //                    ListaPrecioBlister productoBlisterSegunLista = ventaMayoristaBL.CalcularImporteBlisterSegunCliente(ventaMayoristaInDB.ProductosXVenta[i].ProductoID);

            //                    double cantidadEnKG = (Convert.ToDouble(productoBlisterSegunLista.Gramos) * Convert.ToDouble(diferencia)) / 1000; //Convierto a KG
            //                    stock.Cantidad = stock.Cantidad + cantidadEnKG;

            //                }
            //                else
            //                    stock.Cantidad = stock.Cantidad + diferencia;

            //                stockBL.UpdateStock(stock);
            //            }
            //        }


             //   }

            //}


            //Paso siguiente - Agregamos productos nuevos a la venta, si los hay
            if (compraUpdateDTO.ProductosXCompra != null)
            {
                foreach (var prod in compraUpdateDTO.ProductosXCompra)
                {
                    ProductoXCompra nuevoProd = new ProductoXCompra()
                    {
                        ProductoID = prod.ProductoID,
                        Cantidad = prod.Cantidad,
                        TipoDeUnidadID = prod.TipoDeUnidadID,
                        Importe = prod.Importe,
                        Descuento = prod.Descuento,
                        ImporteDescuento = prod.ImporteDescuento,
                        Iibbbsas = prod.Iibbbsas,
                        Iibbcaba = prod.Iibbcaba,
                        Iva = prod.Iva,
                        PrecioUnitario = prod.PrecioUnitario,                       
                        Total = prod.Total,                       
                        CompraID = prod.CompraID
                    };

                    productoXCompraBL.AddProductoXCompra(nuevoProd);
                }

                //if (ventaMayoristaInDB.NoConcretado)
                //{
                //    //Logica para Ventas No Concretadas -- Devolución de Stock
                //}
                //else
                //{
                //    //Una vez cargada la venta, actualizamos Stock
                //    foreach (var item in ventaUpdateDTO.ProductosXVenta)
                //    {

                //        Producto producto = productoBL.GetProductoById(item.ProductoID);

                //        //Actualizamos dependiendo si es un producto Blister o Comun
                //        if (item.TipoDeUnidadID == Constants.TIPODEUNIDAD_BLISTER)
                //        {
                //            Stock stock = stockBL.ValidarStockProducto(item.ProductoID, item.TipoDeUnidadID);

                //            ListaPrecioBlister productoBlisterSegunLista = ventaMayoristaBL.CalcularImporteBlisterSegunCliente(item.ProductoID);

                //            double cantidadEnKG = (Convert.ToDouble(productoBlisterSegunLista.Gramos) * Convert.ToDouble(item.Cantidad)) / 1000; //Convierto a KG
                //            stock.Cantidad = stock.Cantidad - cantidadEnKG;

                //            stockBL.UpdateStock(stock);

                //        }
                //        if (producto.EsMix)
                //        {
                //            //Producto Mix - Actualizar Stock para sus productos asociados
                //            var productosMixStock = stockBL.GetListaProductosMixById(producto.ID);

                //            if (productosMixStock == null)
                //                return BadRequest();

                //            foreach (var prod in productosMixStock)
                //            {
                //                Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), item.TipoDeUnidadID);

                //                if (stockProdMix == null)
                //                    return BadRequest();

                //                double cantidadEnKG = Convert.ToDouble(prod.Cantidad) * Convert.ToDouble(item.Cantidad);
                //                stockProdMix.Cantidad = stockProdMix.Cantidad - cantidadEnKG;

                //                stockBL.UpdateStock(stockProdMix);
                //            }
                //        }
                //        else
                //        {
                //            Stock stock = stockBL.ValidarStockProducto(item.ProductoID, item.TipoDeUnidadID);

                //            stock.Cantidad = stock.Cantidad - item.Cantidad;
                //            stockBL.UpdateStock(stock);
                //        }


                //    }
                //}
            }




            return Ok();
        }



    }
}