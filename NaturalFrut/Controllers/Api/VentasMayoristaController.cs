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
    public class VentasMayoristaController : ApiController
    {

        private readonly VentaMayoristaLogic ventaMayoristaBL;
        private readonly StockLogic stockBL;
        private readonly ClienteLogic clienteBL;
        private readonly VendedorLogic vendedorBL;
        private readonly ProductoLogic productoBL;
        private readonly ProductoXVentaLogic productoXVentaBL;

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
                return BadRequest();

            Cliente cliente = clienteBL.GetClienteById(ventaMayoristaDTO.ClienteID);

            if (cliente == null)
                return BadRequest();

            var ventaMayorista = Mapper.Map<VentaMayoristaDTO, VentaMayorista>(ventaMayoristaDTO);

            ventaMayoristaBL.AddVentaMayorista(ventaMayorista);


            if(ventaMayoristaDTO.NoConcretado)
            {

                //Actualizamos el Saldo en base a la Entrega de Efectivo   
                cliente.Saldo = ventaMayorista.SaldoParcial;
                cliente.SaldoParcial = ventaMayorista.SaldoParcial;
                clienteBL.UpdateCliente(cliente);

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
                            return BadRequest();

                        foreach (var prod in productosMixStock)
                        {
                            Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), item.TipoDeUnidadID);

                            if (stockProdMix == null)
                                return BadRequest();

                            double cantidadEnKG = Convert.ToDouble(prod.Cantidad) * Convert.ToDouble(item.Cantidad);
                            stockProdMix.Cantidad = stockProdMix.Cantidad - cantidadEnKG;

                            stockBL.UpdateStock(stockProdMix);
                        }
                    }
                    else if(item.TipoDeUnidadID == Constants.TIPODEUNIDAD_BLISTER)
                    {
                        //Caso particular para descontar stock de Blister
                        var productosBlisterMixStock = stockBL.GetListaProductosMixById(producto.ID);

                        if (productosBlisterMixStock == null)
                            return BadRequest();

                        foreach (var prod in productosBlisterMixStock)
                        {
                            Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), Constants.TIPODEUNIDAD_MIX);

                            if (stockProdMix == null)
                                return BadRequest();

                            //Convertimos la cantidad a gramos (en formato kg)
                            double cantidadEnGramos = (Convert.ToDouble(prod.Cantidad) / 10) * item.Cantidad;
                            stockProdMix.Cantidad = stockProdMix.Cantidad - cantidadEnGramos;

                            stockBL.UpdateStock(stockProdMix);
                        }

                    }

                }
                else if (item.TipoDeUnidadID == Constants.TIPODEUNIDAD_BLISTER)
                {
                    Stock stock = stockBL.ValidarStockProducto(item.ProductoID, item.TipoDeUnidadID);

                    ListaPrecioBlister productoBlisterSegunLista = ventaMayoristaBL.CalcularImporteBlisterSegunCliente(item.ProductoID);

                    double cantidadEnKG = (Convert.ToDouble(productoBlisterSegunLista.Gramos) * Convert.ToDouble(item.Cantidad)) / 1000; //Convierto a KG
                    stock.Cantidad = stock.Cantidad - cantidadEnKG;

                    stockBL.UpdateStock(stock);

                }
                else if (producto.EsMix)
                {
                    //Producto Mix - Actualizar Stock para sus productos asociados
                    var productosMixStock = stockBL.GetListaProductosMixById(producto.ID);

                    if (productosMixStock == null)
                        return BadRequest();

                    foreach (var prod in productosMixStock)
                    {
                        Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), item.TipoDeUnidadID);

                        if (stockProdMix == null)
                            return BadRequest();

                        //Convertimos la cantidad a gramos (en formato kg)
                        double cantidadEnKG = Convert.ToDouble(prod.Cantidad) * Convert.ToDouble(item.Cantidad);
                        stockProdMix.Cantidad = stockProdMix.Cantidad - cantidadEnKG;

                        stockBL.UpdateStock(stockProdMix);
                    }
                }
                else
                {
                    Stock stock = stockBL.ValidarStockProducto(item.ProductoID, item.TipoDeUnidadID);

                    stock.Cantidad = stock.Cantidad - item.Cantidad;
                    stockBL.UpdateStock(stock);
                }

            }

            return Ok();
        }


        //PUT /api/ventasMayorista
        [HttpPut]
        public IHttpActionResult UpdateVentasMayorista(VentaMayoristaDTO ventaMayoristaDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var cliente = clienteBL.GetClienteById(ventaMayoristaDTO.ClienteID);

            if (cliente == null)
                return BadRequest();

            var ventaMayoristaInDB = ventaMayoristaBL.GetVentaMayoristaById(ventaMayoristaDTO.ID);

            List<float> cantProds = new List<float>();

            foreach (var prod in ventaMayoristaInDB.ProductosXVenta)
            {
                cantProds.Add(prod.Cantidad);
            }


            if (ventaMayoristaInDB == null)
                return NotFound();
            
            //Update para los campos de VentaMayorista
            ventaMayoristaInDB.Descuento = ventaMayoristaDTO.Descuento;
            ventaMayoristaInDB.EntregaEfectivo = ventaMayoristaDTO.EntregaEfectivo;
            ventaMayoristaInDB.Impreso = ventaMayoristaDTO.Impreso;
            ventaMayoristaInDB.NoConcretado = ventaMayoristaDTO.NoConcretado;
            ventaMayoristaInDB.Facturado = ventaMayoristaDTO.Facturado;
            //ventaMayoristaInDB.Saldo = ventaMayoristaDTO.SaldoParcial;
            //ventaMayoristaInDB.SaldoParcial = ventaMayoristaDTO.SaldoParcial;
            ventaMayoristaInDB.SumaTotal = ventaMayoristaDTO.SumaTotal;

            //Update para los campos de sus ProductosXVenta asociados
            for (int i = 0; i < ventaMayoristaInDB.ProductosXVenta.Count; i++)
            {
                ventaMayoristaInDB.ProductosXVenta[i].Cantidad = ventaMayoristaDTO.ProductosXVenta[i].Cantidad;
                ventaMayoristaInDB.ProductosXVenta[i].Descuento = ventaMayoristaDTO.ProductosXVenta[i].Descuento;
                ventaMayoristaInDB.ProductosXVenta[i].Importe = ventaMayoristaDTO.ProductosXVenta[i].Importe;
                ventaMayoristaInDB.ProductosXVenta[i].Total = ventaMayoristaDTO.ProductosXVenta[i].Total;
            }

            ventaMayoristaBL.UpdateVentaMayorista(ventaMayoristaInDB);


            if (ventaMayoristaDTO.NoConcretado)
            {

                //Actualizamos el Saldo en base a la Entrega de Efectivo   
                cliente.Saldo = ventaMayoristaDTO.SaldoParcial;
                cliente.SaldoParcial = ventaMayoristaDTO.SaldoParcial;
                clienteBL.UpdateCliente(cliente);

            }


            //Una vez actualizada la venta, actualizamos Stock
            for (int i = 0; i < ventaMayoristaInDB.ProductosXVenta.Count; i++)
            {

                //Verificamos si se trata de un producto MIX o no
                if(ventaMayoristaInDB.ProductosXVenta[i].Producto.EsMix && ventaMayoristaInDB.ProductosXVenta[i].Producto.EsBlister)
                {
                    //Para casos particulares en productos que son Mix y Blister a la vez

                    if (ventaMayoristaInDB.ProductosXVenta[i].TipoDeUnidadID == Constants.TIPODEUNIDAD_MIX)
                    {
                        //Producto Mix
                        var productosMixStock = stockBL.GetListaProductosMixById(ventaMayoristaInDB.ProductosXVenta[i].ProductoID);

                        if (productosMixStock == null)
                            return BadRequest();

                        foreach (var prod in productosMixStock)
                        {
                            Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), ventaMayoristaInDB.ProductosXVenta[i].TipoDeUnidadID);

                            if (stockProdMix == null)
                                return BadRequest();

                            //Realizo operacion inversa para convertir valor neto en parcial para cada producto
                            var diferencia = cantProds[i] - ventaMayoristaInDB.ProductosXVenta[i].Cantidad;
                            if (diferencia < 0)
                                diferencia = diferencia * (-1);

                            var diferenciaMix = prod.Cantidad * diferencia;

                            if (diferenciaMix != 0)
                            {
                                if (cantProds[i] < ventaMayoristaInDB.ProductosXVenta[i].Cantidad)
                                {
                                    //Cantidad de productos actual es superior a la venta original, restar stock
                                    stockProdMix.Cantidad = stockProdMix.Cantidad - diferenciaMix;

                                    stockBL.UpdateStock(stockProdMix);

                                }
                                else if (cantProds[i] > ventaMayoristaInDB.ProductosXVenta[i].Cantidad)
                                {
                                    //Devolver stock                       
                                    stockProdMix.Cantidad = stockProdMix.Cantidad + diferenciaMix;

                                    stockBL.UpdateStock(stockProdMix);
                                }
                            }
                        }


                    }
                    else if (ventaMayoristaInDB.ProductosXVenta[i].TipoDeUnidadID == Constants.TIPODEUNIDAD_BLISTER)
                    {
                        
                        //Producto Blister caso particular mix
                        var productosBlisterMixStock = stockBL.GetListaProductosMixById(ventaMayoristaInDB.ProductosXVenta[i].ProductoID);

                        if (productosBlisterMixStock == null)
                            return BadRequest();

                        foreach (var prod in productosBlisterMixStock)
                        {
                            Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), Constants.TIPODEUNIDAD_MIX);

                            if (stockProdMix == null)
                                return BadRequest();

                            //Realizo operacion inversa para convertir valor neto en parcial para cada producto
                            var diferencia = cantProds[i] - ventaMayoristaInDB.ProductosXVenta[i].Cantidad;
                            if (diferencia < 0)
                                diferencia = diferencia * (-1);

                            var diferenciaMixBlister = prod.Cantidad * diferencia;

                            if (diferenciaMixBlister != 0)
                            {
                                if (cantProds[i] < ventaMayoristaInDB.ProductosXVenta[i].Cantidad)
                                {
                                    //Cantidad de productos actual es superior a la venta original, restar stock
                                    stockProdMix.Cantidad = stockProdMix.Cantidad - (diferenciaMixBlister / 10);

                                    stockBL.UpdateStock(stockProdMix);

                                }
                                else if (cantProds[i] > ventaMayoristaInDB.ProductosXVenta[i].Cantidad)
                                {
                                    //Devolver stock                       
                                    stockProdMix.Cantidad = stockProdMix.Cantidad + (diferenciaMixBlister / 10);

                                    stockBL.UpdateStock(stockProdMix);
                                }
                            }
                        }

                    }

                }
                else if (ventaMayoristaInDB.ProductosXVenta[i].Producto.EsMix)
                {
                    //Producto Mix
                    var productosMixStock = stockBL.GetListaProductosMixById(ventaMayoristaInDB.ProductosXVenta[i].ProductoID);

                    if (productosMixStock == null)
                        return BadRequest();

                    foreach (var prod in productosMixStock)
                    {
                        Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), ventaMayoristaInDB.ProductosXVenta[i].TipoDeUnidadID);

                        if (stockProdMix == null)
                            return BadRequest();

                        //Realizo operacion inversa para convertir valor neto en parcial para cada producto
                        var diferencia = cantProds[i] - ventaMayoristaInDB.ProductosXVenta[i].Cantidad;
                        if (diferencia < 0)
                            diferencia = diferencia * (-1);

                        var diferenciaMix = prod.Cantidad * diferencia;

                        if (diferenciaMix != 0)
                        {
                            if (cantProds[i] < ventaMayoristaInDB.ProductosXVenta[i].Cantidad)
                            {
                                //Cantidad de productos actual es superior a la venta original, restar stock
                                stockProdMix.Cantidad = stockProdMix.Cantidad - diferenciaMix;

                                stockBL.UpdateStock(stockProdMix);

                            }
                            else if (cantProds[i] > ventaMayoristaInDB.ProductosXVenta[i].Cantidad)
                            {
                                //Devolver stock                       
                                stockProdMix.Cantidad = stockProdMix.Cantidad + diferenciaMix;

                                stockBL.UpdateStock(stockProdMix);
                            }
                        }                        
                    }
                }
                else
                {
                    //Producto Comun
                    Stock stock = stockBL.ValidarStockProducto(ventaMayoristaInDB.ProductosXVenta[i].ProductoID, ventaMayoristaInDB.ProductosXVenta[i].TipoDeUnidadID);

                    var diferencia = cantProds[i] - ventaMayoristaInDB.ProductosXVenta[i].Cantidad;
                    if (diferencia < 0)
                        diferencia = diferencia * (-1);

                    if (diferencia != 0)
                    {
                        if (cantProds[i] < ventaMayoristaInDB.ProductosXVenta[i].Cantidad)
                        {
                            //Cantidad de productos actual es superior a la venta original, restar stock
                            //Actualizamos dependiendo si es un producto Blister o Comun                       

                            if (ventaMayoristaInDB.ProductosXVenta[i].TipoDeUnidadID == Constants.TIPODEUNIDAD_BLISTER)
                            {
                                ListaPrecioBlister productoBlisterSegunLista = ventaMayoristaBL.CalcularImporteBlisterSegunCliente(ventaMayoristaInDB.ProductosXVenta[i].ProductoID);

                                double cantidadEnKG = (Convert.ToDouble(productoBlisterSegunLista.Gramos) * Convert.ToDouble(diferencia) / 1000); //Convierto a KG
                                stock.Cantidad = stock.Cantidad - cantidadEnKG;

                            }
                            else
                                stock.Cantidad = stock.Cantidad - diferencia;

                            stockBL.UpdateStock(stock);

                        }
                        else if (cantProds[i] > ventaMayoristaInDB.ProductosXVenta[i].Cantidad)
                        {
                            //Devolver stock                       
                            //Actualizamos dependiendo si es un producto Blister o Comun
                            if (ventaMayoristaInDB.ProductosXVenta[i].TipoDeUnidadID == Constants.TIPODEUNIDAD_BLISTER)
                            {
                                ListaPrecioBlister productoBlisterSegunLista = ventaMayoristaBL.CalcularImporteBlisterSegunCliente(ventaMayoristaInDB.ProductosXVenta[i].ProductoID);

                                double cantidadEnKG = (Convert.ToDouble(productoBlisterSegunLista.Gramos) * Convert.ToDouble(diferencia)) / 1000; //Convierto a KG
                                stock.Cantidad = stock.Cantidad + cantidadEnKG;

                            }
                            else
                                stock.Cantidad = stock.Cantidad + diferencia;

                            stockBL.UpdateStock(stock);
                        }
                    }
                }
            }

            return Ok();
        }

        //DELETE /api/ventasMayorista/1
        [HttpDelete]
        public IHttpActionResult DeleteProductoVentaMayorista(BorrarProdVtaMayDTO prodVenta)
        {

            var productoInDB = productoXVentaBL.GetProductoXVentaIndividualById(prodVenta);

            if (productoInDB == null)
                return NotFound();

            var importeTotalProducto = productoInDB.Total;

            productoXVentaBL.RemoveProductoXVenta(productoInDB);

            //Actualizamos el total de la venta
            var ventaMayoristaInDB = ventaMayoristaBL.GetVentaMayoristaById(prodVenta.VentaID);

            ventaMayoristaInDB.SumaTotal = ventaMayoristaInDB.SumaTotal - importeTotalProducto;

            ventaMayoristaBL.UpdateVentaMayorista(ventaMayoristaInDB);

            return Ok();

        }

        //DELETE /api/ventasMayorista/1
        [HttpDelete]
        [Route("api/ventasmayorista/deleteventamayorista/{Id}")]
        public IHttpActionResult DeleteVentaMayorista(int Id)
        {

            var ventaMayoristaInDB = ventaMayoristaBL.GetVentaMayoristaById(Id);

            if (ventaMayoristaInDB == null)
                return NotFound();

            if (ventaMayoristaInDB.NoConcretado)
            {
                //Venta concretada, devolvemos stock 
                for (int i = 0; i < ventaMayoristaInDB.ProductosXVenta.Count; i++)
                {

                    //Verificamos si se trata de un producto MIX o no
                    if (ventaMayoristaInDB.ProductosXVenta[i].Producto.EsMix)
                    {
                        //Producto Mix
                        var productosMixStock = stockBL.GetListaProductosMixById(ventaMayoristaInDB.ProductosXVenta[i].ProductoID);

                        if (productosMixStock == null)
                            return BadRequest();

                        foreach (var prod in productosMixStock)
                        {
                            Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), ventaMayoristaInDB.ProductosXVenta[i].TipoDeUnidadID);

                            if (stockProdMix == null)
                                return BadRequest();

                            //Realizo operacion inversa para convertir valor neto en parcial para cada producto
                            var cantidad = ventaMayoristaInDB.ProductosXVenta[i].Cantidad;

                            var cantidadMix = prod.Cantidad * cantidad;

                            //Devolver stock                       
                            stockProdMix.Cantidad = stockProdMix.Cantidad + cantidadMix;

                            stockBL.UpdateStock(stockProdMix);

                        }


                    }
                    else
                    {
                        //Producto Comun
                        Stock stock = stockBL.ValidarStockProducto(ventaMayoristaInDB.ProductosXVenta[i].ProductoID, ventaMayoristaInDB.ProductosXVenta[i].TipoDeUnidadID);

                        var cantidad = ventaMayoristaInDB.ProductosXVenta[i].Cantidad;

                        //Devolver stock                       
                        //Actualizamos dependiendo si es un producto Blister o Comun
                        if (ventaMayoristaInDB.ProductosXVenta[i].TipoDeUnidadID == Constants.TIPODEUNIDAD_BLISTER)
                        {
                            ListaPrecioBlister productoBlisterSegunLista = ventaMayoristaBL.CalcularImporteBlisterSegunCliente(ventaMayoristaInDB.ProductosXVenta[i].ProductoID);

                            double cantidadEnKG = (Convert.ToDouble(productoBlisterSegunLista.Gramos) * Convert.ToDouble(cantidad)) / 1000; //Convierto a KG
                            stock.Cantidad = stock.Cantidad + cantidadEnKG;

                        }
                        else
                            stock.Cantidad = stock.Cantidad + cantidad;

                        stockBL.UpdateStock(stock);

                    }

                }

                //BORRAMOS PRODUCTOS ASOCIADOS Y LA VENTA MAYORISTA

                List<ProductoXVenta> prodsABorrar = new List<ProductoXVenta>();
                foreach (var prod in ventaMayoristaInDB.ProductosXVenta)
                {
                    //Guardamos referencia de los productos asociados
                    var prodInDB = productoXVentaBL.GetProductoXVentaById(prod.ID);

                    if (prodInDB == null)
                        return BadRequest();

                    prodsABorrar.Add(prodInDB);
                }

                ////Borramos Venta Mayorista
                var ventaABorrar = ventaMayoristaBL.GetVentaMayoristaById(Id);
                ventaMayoristaBL.RemoveVentaMayorista(ventaABorrar);

                //Borramos Productos asociados
                foreach (var item in prodsABorrar)
                {
                    productoXVentaBL.RemoveProductoXVenta(item);
                }


            }
            else
            {
                //Venta No Concretada - No se devuelve stock

                List<ProductoXVenta> prodsABorrar = new List<ProductoXVenta>();
                foreach (var prod in ventaMayoristaInDB.ProductosXVenta)
                {
                    //Guardamos referencia de los productos asociados
                    var prodInDB = productoXVentaBL.GetProductoXVentaById(prod.ID);

                    if (prodInDB == null)
                        return BadRequest();

                    prodsABorrar.Add(prodInDB);
                }

                ////Borramos Venta Mayorista
                var ventaABorrar = ventaMayoristaBL.GetVentaMayoristaById(Id);
                ventaMayoristaBL.RemoveVentaMayorista(ventaABorrar);

                //Borramos Productos asociados
                foreach (var item in prodsABorrar)
                {
                    productoXVentaBL.RemoveProductoXVenta(item);
                }


            }

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