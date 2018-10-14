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

            //Actualizamos el Saldo en base a la Entrega de Efectivo            
            cliente.Saldo = ventaMayorista.NuevoSaldo;
            clienteBL.UpdateCliente(cliente);           

            if (ventaMayorista.NoConcretado)
            {
                //Logica para Ventas No Concretadas -- Devolución de Stock
            }
            else
            {
                //Una vez cargada la venta, actualizamos Stock
                foreach (var item in ventaMayorista.ProductosXVenta)
                {
                    
                    Producto producto = productoBL.GetProductoById(item.ProductoID);

                    //Actualizamos dependiendo si es un producto Blister o Comun
                    if(item.TipoDeUnidadID == Constants.TIPODEUNIDAD_BLISTER)
                    {
                        Stock stock = stockBL.ValidarStockProducto(item.ProductoID, item.TipoDeUnidadID);

                        ListaPrecioBlister productoBlisterSegunLista = ventaMayoristaBL.CalcularImporteBlisterSegunCliente(item.ProductoID);

                        double cantidadEnKG = (Convert.ToDouble(productoBlisterSegunLista.Gramos) * Convert.ToDouble(item.Cantidad)) / 1000; //Convierto a KG
                        stock.Cantidad = stock.Cantidad - cantidadEnKG;

                        stockBL.UpdateStock(stock);

                    }
                    else if(producto.EsMix)
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
                    else
                    {
                        Stock stock = stockBL.ValidarStockProducto(item.ProductoID, item.TipoDeUnidadID);

                        stock.Cantidad = stock.Cantidad - item.Cantidad;
                        stockBL.UpdateStock(stock);
                    }                       
                    
                }
            }
                        

            return Ok();
        }


        //PUT /api/ventasMayorista
        [HttpPut]
        public IHttpActionResult UpdateVentasMayorista(VentaUpdateDTO ventaUpdateDTO)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest();

            var cliente = clienteBL.GetClienteById(ventaUpdateDTO.VentaMayorista.ClienteID);

            if (cliente == null)
                return BadRequest();

            var ventaMayoristaInDB = ventaMayoristaBL.GetVentaMayoristaById(ventaUpdateDTO.VentaMayorista.ID);

            List<float> cantProds = new List<float>();

            foreach (var prod in ventaMayoristaInDB.ProductosXVenta)
            {
                cantProds.Add(prod.Cantidad);
            }


            if (ventaMayoristaInDB == null)
                return NotFound();

           
           
            //Update para los campos de VentaMayorista
            ventaMayoristaInDB.Descuento = ventaUpdateDTO.VentaMayorista.Descuento;
            ventaMayoristaInDB.EntregaEfectivo = ventaUpdateDTO.VentaMayorista.EntregaEfectivo;
            ventaMayoristaInDB.Impreso = ventaUpdateDTO.VentaMayorista.Impreso;
            ventaMayoristaInDB.NoConcretado = ventaUpdateDTO.VentaMayorista.NoConcretado;
            ventaMayoristaInDB.Facturado = ventaUpdateDTO.VentaMayorista.Facturado;
            ventaMayoristaInDB.Saldo = ventaUpdateDTO.VentaMayorista.NuevoSaldo;
            ventaMayoristaInDB.NuevoSaldo = ventaUpdateDTO.VentaMayorista.NuevoSaldo;
            ventaMayoristaInDB.SumaTotal = ventaUpdateDTO.VentaMayorista.SumaTotal;

            //Update para los campos de sus ProductosXVenta asociados
            for (int i = 0; i < ventaMayoristaInDB.ProductosXVenta.Count; i++)
            {
                ventaMayoristaInDB.ProductosXVenta[i].Cantidad = ventaUpdateDTO.VentaMayorista.ProductosXVenta[i].Cantidad;
                ventaMayoristaInDB.ProductosXVenta[i].Descuento = ventaUpdateDTO.VentaMayorista.ProductosXVenta[i].Descuento;
                ventaMayoristaInDB.ProductosXVenta[i].Importe = ventaUpdateDTO.VentaMayorista.ProductosXVenta[i].Importe;
                ventaMayoristaInDB.ProductosXVenta[i].Total = ventaUpdateDTO.VentaMayorista.ProductosXVenta[i].Total;
            }
            
            ventaMayoristaBL.UpdateVentaMayorista(ventaMayoristaInDB);

            //Actualizamos el Saldo en base a la Entrega de Efectivo            
            cliente.Saldo = ventaUpdateDTO.VentaMayorista.NuevoSaldo;
            clienteBL.UpdateCliente(cliente);

            if (ventaMayoristaInDB.NoConcretado)
            {
                //Logica para Ventas No Concretadas -- Devolución de Stock
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

            }
            else
            {
                //Una vez actualizada la venta, actualizamos Stock
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
                            var diferencia = cantProds[i] - ventaMayoristaInDB.ProductosXVenta[i].Cantidad;
                            if (diferencia < 0)
                                diferencia = diferencia * (-1);

                            var diferenciaMix = prod.Cantidad * diferencia;


                            if (cantProds[i] < ventaMayoristaInDB.ProductosXVenta[i].Cantidad)
                            {
                                //Cantidad de productos actual es superior a la venta original, restar stock
                                stockProdMix.Cantidad = stockProdMix.Cantidad - diferenciaMix;

                                stockBL.UpdateStock(stockProdMix);

                            }
                            if (cantProds[i] > ventaMayoristaInDB.ProductosXVenta[i].Cantidad)
                            {
                                //Devolver stock                       
                                stockProdMix.Cantidad = stockProdMix.Cantidad + diferenciaMix;

                                stockBL.UpdateStock(stockProdMix);
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
                        if (cantProds[i] > ventaMayoristaInDB.ProductosXVenta[i].Cantidad)
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


            //Paso siguiente - Agregamos productos nuevos a la venta, si los hay
            if(ventaUpdateDTO.ProductosXVenta != null)
            {
                foreach (var prod in ventaUpdateDTO.ProductosXVenta)
                {
                    ProductoXVenta nuevoProd = new ProductoXVenta
                    {
                        Cantidad = prod.Cantidad,
                        Descuento = prod.Descuento,
                        Importe = prod.Importe,
                        Total = prod.Total,
                        ProductoID = prod.ProductoID,
                        TipoDeUnidadID = prod.TipoDeUnidadID,
                        VentaID = prod.VentaID
                    };

                    productoXVentaBL.AddProductoXVenta(nuevoProd);
                }

                if (ventaMayoristaInDB.NoConcretado)
                {
                    //Logica para Ventas No Concretadas -- Devolución de Stock
                }
                else
                {
                    //Una vez cargada la venta, actualizamos Stock
                    foreach (var item in ventaUpdateDTO.ProductosXVenta)
                    {
                                                
                        Producto producto = productoBL.GetProductoById(item.ProductoID);

                        //Actualizamos dependiendo si es un producto Blister o Comun
                        if (item.TipoDeUnidadID == Constants.TIPODEUNIDAD_BLISTER)
                        {
                            Stock stock = stockBL.ValidarStockProducto(item.ProductoID, item.TipoDeUnidadID);

                            ListaPrecioBlister productoBlisterSegunLista = ventaMayoristaBL.CalcularImporteBlisterSegunCliente(item.ProductoID);

                            double cantidadEnKG = (Convert.ToDouble(productoBlisterSegunLista.Gramos) * Convert.ToDouble(item.Cantidad)) / 1000; //Convierto a KG
                            stock.Cantidad = stock.Cantidad - cantidadEnKG;

                            stockBL.UpdateStock(stock);

                        }
                        if (producto.EsMix)
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
                        else
                        {
                            Stock stock = stockBL.ValidarStockProducto(item.ProductoID, item.TipoDeUnidadID);

                            stock.Cantidad = stock.Cantidad - item.Cantidad;
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
            var importeTotalProducto = productoInDB.Total;

            if (productoInDB == null)
                return NotFound();

            productoXVentaBL.RemoveProductoXVenta(productoInDB);

            //Actualizamos el total de la venta
            var ventaMayoristaInDB = ventaMayoristaBL.GetVentaMayoristaById(prodVenta.VentaID);

            ventaMayoristaInDB.SumaTotal = ventaMayoristaInDB.SumaTotal - importeTotalProducto;

            ventaMayoristaBL.UpdateVentaMayorista(ventaMayoristaInDB);

            return Ok();

        }

    }
}