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
    public class VentasMayoristaController : ApiController
    {

        private readonly VentaMayoristaLogic ventaMayoristaBL;
        private readonly StockLogic stockBL;
        private readonly ClienteLogic clienteBL;
        private readonly VendedorLogic vendedorBL;
        private readonly ProductoLogic productoBL;
        private readonly ProductoXVentaLogic productoXVentaBL;

        private UOWVentaMayorista _UOWVentaMayorista = new UOWVentaMayorista();

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

            //ventaMayoristaBL.AddVentaMayorista(ventaMayorista);
            _UOWVentaMayorista.VentaMayoristaRepository.Add(ventaMayorista);


            if(ventaMayoristaDTO.NoConcretado)
            {

                //Actualizamos el Saldo en base a la Entrega de Efectivo   
                cliente.Saldo = ventaMayorista.SaldoParcial;
                cliente.SaldoParcial = ventaMayorista.SaldoParcial;

                //clienteBL.UpdateCliente(cliente);
                _UOWVentaMayorista.ClienteRepository.Update(cliente);

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

                            int prodDelMixID = prod.ProductoDelMixId.GetValueOrDefault();
                            //Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), item.TipoDeUnidadID);
                            Stock stockProdMix = _UOWVentaMayorista.StockRepository
                                .GetAll()
                                .Where(s => s.ProductoID == prodDelMixID && s.TipoDeUnidadID == item.TipoDeUnidadID)
                                .SingleOrDefault();


                            if (stockProdMix == null)
                                return BadRequest();

                            double cantidadEnKG = Convert.ToDouble(prod.Cantidad) * Convert.ToDouble(item.Cantidad);
                            stockProdMix.Cantidad = stockProdMix.Cantidad - cantidadEnKG;

                            if (stockProdMix.Cantidad < 0)
                                stockProdMix.Cantidad = 0;

                            //stockBL.UpdateStock(stockProdMix);
                            _UOWVentaMayorista.StockRepository.Update(stockProdMix);
                            
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
                            int prodDelMixID = prod.ProductoDelMixId.GetValueOrDefault();
                            //Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), Constants.TIPODEUNIDAD_MIX);
                            Stock stockProdMix = _UOWVentaMayorista.StockRepository
                                .GetAll()
                                .Where(s => s.ProductoID == prodDelMixID && s.TipoDeUnidadID == Constants.TIPODEUNIDAD_MIX)
                                .SingleOrDefault();

                            if (stockProdMix == null)
                                return BadRequest();

                            //Convertimos la cantidad a gramos (en formato kg)
                            double cantidadEnGramos = (Convert.ToDouble(prod.Cantidad) / 1000) * item.Cantidad;
                            stockProdMix.Cantidad = stockProdMix.Cantidad - cantidadEnGramos;

                            if (stockProdMix.Cantidad < 0)
                                stockProdMix.Cantidad = 0;

                            //stockBL.UpdateStock(stockProdMix);
                            _UOWVentaMayorista.StockRepository.Update(stockProdMix);
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
                        return BadRequest();

                    foreach (var prod in productosMixStock)
                    {
                        int prodDelMixID = prod.ProductoDelMixId.GetValueOrDefault();
                        //Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), item.TipoDeUnidadID);
                        Stock stockProdMix = _UOWVentaMayorista.StockRepository
                            .GetAll()
                            .Where(s => s.ProductoID == prodDelMixID && s.TipoDeUnidadID == item.TipoDeUnidadID)
                            .SingleOrDefault();

                        if (stockProdMix == null)
                            return BadRequest();

                        //Convertimos la cantidad a gramos (en formato kg)
                        double cantidadEnKG = Convert.ToDouble(prod.Cantidad) * Convert.ToDouble(item.Cantidad);
                        stockProdMix.Cantidad = stockProdMix.Cantidad - cantidadEnKG;

                        if (stockProdMix.Cantidad < 0)
                            stockProdMix.Cantidad = 0;

                        //stockBL.UpdateStock(stockProdMix);
                        _UOWVentaMayorista.StockRepository.Update(stockProdMix);
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
                    }
                }

            }

            //Salvamos los cambios
            _UOWVentaMayorista.Save();

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
            int ventaId = ventaMayoristaInDB.ID;

            foreach (var prod in ventaMayoristaInDB.ProductosXVenta)
            {
                cantProds.Add(prod.Cantidad);
            }


            if (ventaMayoristaInDB == null)
                return NotFound();

            VentaMayorista ventaUpdate = _UOWVentaMayorista.VentaMayoristaRepository.GetByID(ventaMayoristaDTO.ID);

            //Update para los campos de VentaMayorista
            ventaUpdate.Descuento = ventaMayoristaDTO.Descuento;
            ventaUpdate.EntregaEfectivo = ventaMayoristaDTO.EntregaEfectivo;
            ventaUpdate.Impreso = ventaMayoristaDTO.Impreso;
            ventaUpdate.NoConcretado = ventaMayoristaDTO.NoConcretado;
            ventaUpdate.Facturado = ventaMayoristaDTO.Facturado;
            //ventaUpdate.Saldo = ventaMayoristaDTO.SaldoParcial;
            //ventaUpdate.SaldoParcial = ventaMayoristaDTO.SaldoParcial;
            ventaUpdate.SumaTotal = ventaMayoristaDTO.SumaTotal;

            //UPDATE VENTA MAYORISTA
            //ventaMayoristaBL.UpdateVentaMayorista(ventaMayoristaInDB);
            _UOWVentaMayorista.VentaMayoristaRepository.Update(ventaUpdate);

            //Update para los campos de sus ProductosXVenta asociados
            for (int i = 0; i < ventaMayoristaInDB.ProductosXVenta.Count; i++)
            {

                int prodDeLaVentaId = ventaMayoristaInDB.ProductosXVenta[i].ProductoID;
                ProductoXVenta prodsXVenta = _UOWVentaMayorista.ProductosXVentaRepository.GetAll().Where(s => s.VentaID == ventaId && s.ProductoID == prodDeLaVentaId).SingleOrDefault();
                
                prodsXVenta.Cantidad = ventaMayoristaDTO.ProductosXVenta[i].Cantidad;
                prodsXVenta.Descuento = ventaMayoristaDTO.ProductosXVenta[i].Descuento;
                prodsXVenta.Importe = ventaMayoristaDTO.ProductosXVenta[i].Importe;
                prodsXVenta.Total = ventaMayoristaDTO.ProductosXVenta[i].Total;

                //De Referencia para calcular stock luego
                ventaMayoristaInDB.ProductosXVenta[i].Cantidad = ventaMayoristaDTO.ProductosXVenta[i].Cantidad;
                ventaMayoristaInDB.ProductosXVenta[i].Descuento = ventaMayoristaDTO.ProductosXVenta[i].Descuento;
                ventaMayoristaInDB.ProductosXVenta[i].Importe = ventaMayoristaDTO.ProductosXVenta[i].Importe;
                ventaMayoristaInDB.ProductosXVenta[i].Total = ventaMayoristaDTO.ProductosXVenta[i].Total;

                //UPDATE PRODUCTOS DE VENTA MAYORISTA
                _UOWVentaMayorista.ProductosXVentaRepository.Update(prodsXVenta);
            }
            
            if (ventaMayoristaDTO.NoConcretado)
            {

                //Actualizamos el Saldo en base a la Entrega de Efectivo   
                cliente.Saldo = ventaMayoristaDTO.SaldoParcial;
                cliente.SaldoParcial = ventaMayoristaDTO.SaldoParcial;

                //clienteBL.UpdateCliente(cliente);
                _UOWVentaMayorista.ClienteRepository.Update(cliente);

            }


            //Una vez actualizada la venta, actualizamos Stock
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
                            return BadRequest();

                        foreach (var prod in productosMixStock)
                        {

                            int prodDelMixId = prod.ProductoDelMixId.GetValueOrDefault();

                            Stock stockProdMix = _UOWVentaMayorista.StockRepository.GetAll().Where(s => s.ProductoID == prodDelMixId && s.TipoDeUnidadID == tipoUnidadId).SingleOrDefault();
                            //Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), ventaMayoristaInDB.ProductosXVenta[i].TipoDeUnidadID);

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

                                    if (stockProdMix.Cantidad < 0)
                                        stockProdMix.Cantidad = 0;

                                    //stockBL.UpdateStock(stockProdMix);
                                    _UOWVentaMayorista.StockRepository.Update(stockProdMix);

                                }
                                else if (cantProds[i] > ventaMayoristaInDB.ProductosXVenta[i].Cantidad)
                                {
                                    //Devolver stock                       
                                    stockProdMix.Cantidad = stockProdMix.Cantidad + diferenciaMix;

                                    //stockBL.UpdateStock(stockProdMix);
                                    _UOWVentaMayorista.StockRepository.Update(stockProdMix);
                                }
                            }
                        }


                    }
                    else if (tipoUnidadId == Constants.TIPODEUNIDAD_BLISTER)
                    {
                        
                        //Producto Blister caso particular mix
                        var productosBlisterMixStock = stockBL.GetListaProductosMixById(prodId);

                        if (productosBlisterMixStock == null)
                            return BadRequest();

                        foreach (var prod in productosBlisterMixStock)
                        {
                            int prodDelMixId = prod.ProductoDelMixId.GetValueOrDefault();

                            Stock stockProdMix = _UOWVentaMayorista.StockRepository.GetAll().Where(s => s.ProductoID == prodDelMixId && s.TipoDeUnidadID == Constants.TIPODEUNIDAD_MIX).SingleOrDefault();
                            //Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), Constants.TIPODEUNIDAD_MIX);

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
                                    stockProdMix.Cantidad = stockProdMix.Cantidad - (diferenciaMixBlister / 1000);

                                    if (stockProdMix.Cantidad < 0)
                                        stockProdMix.Cantidad = 0;

                                    //stockBL.UpdateStock(stockProdMix);
                                    _UOWVentaMayorista.StockRepository.Update(stockProdMix);

                                }
                                else if (cantProds[i] > ventaMayoristaInDB.ProductosXVenta[i].Cantidad)
                                {
                                    //Devolver stock                       
                                    stockProdMix.Cantidad = stockProdMix.Cantidad + (diferenciaMixBlister / 1000);

                                    //stockBL.UpdateStock(stockProdMix);
                                    _UOWVentaMayorista.StockRepository.Update(stockProdMix);
                                }
                            }
                        }

                    }

                }
                else if (ventaMayoristaInDB.ProductosXVenta[i].Producto.EsMix)
                {
                    //Producto Mix
                    var productosMixStock = stockBL.GetListaProductosMixById(prodId);

                    if (productosMixStock == null)
                        return BadRequest();

                    foreach (var prod in productosMixStock)
                    {
                        int prodDelMixId = prod.ProductoDelMixId.GetValueOrDefault();

                        Stock stockProdMix = _UOWVentaMayorista.StockRepository
                            .GetAll()
                            .Where(s => s.ProductoID == prodDelMixId && s.TipoDeUnidadID == tipoUnidadId)
                            .SingleOrDefault();
                        //Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), ventaMayoristaInDB.ProductosXVenta[i].TipoDeUnidadID);

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

                                if (stockProdMix.Cantidad < 0)
                                    stockProdMix.Cantidad = 0;

                                //stockBL.UpdateStock(stockProdMix);
                                _UOWVentaMayorista.StockRepository.Update(stockProdMix);

                            }
                            else if (cantProds[i] > ventaMayoristaInDB.ProductosXVenta[i].Cantidad)
                            {
                                //Devolver stock                       
                                stockProdMix.Cantidad = stockProdMix.Cantidad + diferenciaMix;

                                //stockBL.UpdateStock(stockProdMix);
                                _UOWVentaMayorista.StockRepository.Update(stockProdMix);
                            }
                        }                        
                    }
                }
                else
                {
                    
                    var diferencia = cantProds[i] - ventaMayoristaInDB.ProductosXVenta[i].Cantidad;
                    if (diferencia < 0)
                        diferencia = diferencia * (-1);

                    if (diferencia != 0)
                    {
                        if (cantProds[i] < ventaMayoristaInDB.ProductosXVenta[i].Cantidad)
                        {
                            //Cantidad de productos actual es superior a la venta original, restar stock
                            //Actualizamos dependiendo si es un producto Blister o Comun                       

                            if (tipoUnidadId == Constants.TIPODEUNIDAD_BLISTER)
                            {
                                //Producto Comun
                                Stock stock = _UOWVentaMayorista.StockRepository.GetAll().Where(s => s.ProductoID == prodId && s.TipoDeUnidadID == Constants.PRECIO_X_KG).SingleOrDefault();
                                //Stock stock = stockBL.ValidarStockProducto(ventaMayoristaInDB.ProductosXVenta[i].ProductoID, Constants.PRECIO_X_KG);

                                ListaPrecioBlister productoBlisterSegunLista = ventaMayoristaBL.CalcularImporteBlisterSegunCliente(prodId);

                                double cantidadEnKG = (Convert.ToDouble(productoBlisterSegunLista.Gramos) * Convert.ToDouble(diferencia) / 1000); //Convierto a KG
                                stock.Cantidad = stock.Cantidad - cantidadEnKG;

                                if (stock.Cantidad < 0)
                                    stock.Cantidad = 0;

                                //stockBL.UpdateStock(stock);
                                _UOWVentaMayorista.StockRepository.Update(stock);
                            }
                            else
                            {
                                
                                //Producto Comun
                                //Stock stock = stockBL.ValidarStockProducto(ventaMayoristaInDB.ProductosXVenta[i].ProductoID, ventaMayoristaInDB.ProductosXVenta[i].TipoDeUnidadID);
                                Stock stock = _UOWVentaMayorista.StockRepository.GetAll().Where(s => s.ProductoID == prodId && s.TipoDeUnidadID == tipoUnidadId).SingleOrDefault();
                                stock.Cantidad = stock.Cantidad - diferencia;

                                if (stock.Cantidad < 0)
                                    stock.Cantidad = 0;

                                //stockBL.UpdateStock(stock);
                                _UOWVentaMayorista.StockRepository.Update(stock);
                            }
                            

                        }
                        else if (cantProds[i] > ventaMayoristaInDB.ProductosXVenta[i].Cantidad)
                        {
                            //Devolver stock                       
                            //Actualizamos dependiendo si es un producto Blister o Comun
                            if (ventaMayoristaInDB.ProductosXVenta[i].TipoDeUnidadID == Constants.TIPODEUNIDAD_BLISTER)
                            {
                                //Producto Blister
                                Stock stock = _UOWVentaMayorista.StockRepository.GetAll().Where(s => s.ProductoID == prodId && s.TipoDeUnidadID == Constants.PRECIO_X_KG).SingleOrDefault();
                                //Stock stock = stockBL.ValidarStockProducto(ventaMayoristaInDB.ProductosXVenta[i].ProductoID, Constants.PRECIO_X_KG);

                                ListaPrecioBlister productoBlisterSegunLista = ventaMayoristaBL.CalcularImporteBlisterSegunCliente(prodId);

                                double cantidadEnKG = (Convert.ToDouble(productoBlisterSegunLista.Gramos) * Convert.ToDouble(diferencia)) / 1000; //Convierto a KG
                                stock.Cantidad = stock.Cantidad + cantidadEnKG;

                                //stockBL.UpdateStock(stock);
                                _UOWVentaMayorista.StockRepository.Update(stock);

                            }
                            else
                            {
                                Stock stock = _UOWVentaMayorista.StockRepository.GetAll().Where(s => s.ProductoID == prodId && s.TipoDeUnidadID == tipoUnidadId).SingleOrDefault();
                                //Stock stock = stockBL.ValidarStockProducto(ventaMayoristaInDB.ProductosXVenta[i].ProductoID, ventaMayoristaInDB.ProductosXVenta[i].TipoDeUnidadID);
                                stock.Cantidad = stock.Cantidad + diferencia;

                                //stockBL.UpdateStock(stock);
                                _UOWVentaMayorista.StockRepository.Update(stock);
                            }
                            

                            
                        }
                    }
                }
            }

            //Actualizamos los valores
            _UOWVentaMayorista.Save();

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
                        return BadRequest();

                    foreach (var prod in productosMixStock)
                    {
                        Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), productoInDB.TipoDeUnidadID);

                        if (stockProdMix == null)
                            return BadRequest();

                        double cantidadEnKG = Convert.ToDouble(prod.Cantidad) * Convert.ToDouble(productoInDB.Cantidad);
                        stockProdMix.Cantidad = stockProdMix.Cantidad + cantidadEnKG;

                        //stockBL.UpdateStock(stockProdMix);
                        _UOWVentaMayorista.StockRepository.Update(stockProdMix);
                    }


                }
                else if (productoInDB.TipoDeUnidadID == Constants.TIPODEUNIDAD_BLISTER)
                {

                    //Caso particular para descontar stock de Blister
                    var productosBlisterMixStock = stockBL.GetListaProductosMixById(productoInDB.ProductoID);

                    if (productosBlisterMixStock == null)
                        return BadRequest();

                    foreach (var prod in productosBlisterMixStock)
                    {
                        Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), Constants.TIPODEUNIDAD_MIX);

                        if (stockProdMix == null)
                            return BadRequest();

                        ListaPrecioBlister productoBlisterSegunLista = ventaMayoristaBL.CalcularImporteBlisterSegunCliente(productoInDB.ProductoID);
                        double cantidadEnKG = (Convert.ToDouble(productoInDB.Cantidad) * (Convert.ToDouble(productoBlisterSegunLista.Gramos) / 1000)); //Convierto a KG

                        stockProdMix.Cantidad = stockProdMix.Cantidad + cantidadEnKG;

                        //stockBL.UpdateStock(stockProdMix);
                        _UOWVentaMayorista.StockRepository.Update(stockProdMix);
                    }

                }

            }
            else if (productoInDB.Producto.EsMix)
            {
                //Producto Mix - Devolver Stock
                var productosMixStock = stockBL.GetListaProductosMixById(productoInDB.ProductoID);

                if (productosMixStock == null)
                    return BadRequest();

                foreach (var prod in productosMixStock)
                {
                    Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), productoInDB.TipoDeUnidadID);

                    if (stockProdMix == null)
                        return BadRequest();

                    double cantidadEnKG = Convert.ToDouble(prod.Cantidad) * Convert.ToDouble(productoInDB.Cantidad);
                    stockProdMix.Cantidad = stockProdMix.Cantidad + cantidadEnKG;

                    //stockBL.UpdateStock(stockProdMix);
                    _UOWVentaMayorista.StockRepository.Update(stockProdMix);
                }
            }
            else
            {

                //Stock para el productos comunes y blisters
                if (productoInDB.TipoDeUnidadID == Constants.TIPODEUNIDAD_BLISTER)
                {
                    //Producto Blister
                    Stock stock = stockBL.ValidarStockProducto(productoInDB.ProductoID, Constants.PRECIO_X_KG);
                    ListaPrecioBlister productoBlisterSegunLista = ventaMayoristaBL.CalcularImporteBlisterSegunCliente(productoInDB.ProductoID);

                    double cantidadEnKG = (Convert.ToDouble(productoInDB.Cantidad) * (Convert.ToDouble(productoBlisterSegunLista.Gramos) / 1000)); //Convierto a KG

                    stock.Cantidad = stock.Cantidad + cantidadEnKG;

                    //stockBL.UpdateStock(stock);
                    _UOWVentaMayorista.StockRepository.Update(stock);
                }
                else
                {
                    //Producto Comun
                    Stock stock = stockBL.ValidarStockProducto(productoInDB.ProductoID, productoInDB.TipoDeUnidadID);

                    stock.Cantidad = stock.Cantidad + productoInDB.Cantidad;

                    //stockBL.UpdateStock(stock);
                    _UOWVentaMayorista.StockRepository.Update(stock);
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
                            return BadRequest();

                        foreach (var prod in productosMixStock)
                        {

                            int prodDelMixId = prod.ProductoDelMixId.GetValueOrDefault();

                            Stock stockProdMix = _UOWVentaMayorista.StockRepository.GetAll().Where(s => s.ProductoID == prodDelMixId && s.TipoDeUnidadID == tipoUnidadId).SingleOrDefault();
                            //Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), ventaMayoristaInDB.ProductosXVenta[i].TipoDeUnidadID);

                            if (stockProdMix == null)
                                return BadRequest();

                            double cantidadEnKG = Convert.ToDouble(prod.Cantidad) * Convert.ToDouble(ventaMayoristaInDB.ProductosXVenta[i].Cantidad);
                            stockProdMix.Cantidad = stockProdMix.Cantidad + cantidadEnKG;

                            //stockBL.UpdateStock(stockProdMix);
                            _UOWVentaMayorista.StockRepository.Update(stockProdMix);
                            
                        }

                    }
                    else if (tipoUnidadId == Constants.TIPODEUNIDAD_BLISTER)
                    {

                        //Producto Blister caso particular mix
                        var productosBlisterMixStock = stockBL.GetListaProductosMixById(prodId);

                        if (productosBlisterMixStock == null)
                            return BadRequest();

                        foreach (var prod in productosBlisterMixStock)
                        {
                            int prodDelMixId = prod.ProductoDelMixId.GetValueOrDefault();

                            Stock stockProdMix = _UOWVentaMayorista.StockRepository.GetAll().Where(s => s.ProductoID == prodDelMixId && s.TipoDeUnidadID == Constants.TIPODEUNIDAD_MIX).SingleOrDefault();
                            //Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), Constants.TIPODEUNIDAD_MIX);

                            if (stockProdMix == null)
                                return BadRequest();

                            ListaPrecioBlister productoBlisterSegunLista = ventaMayoristaBL.CalcularImporteBlisterSegunCliente(ventaMayoristaInDB.ProductosXVenta[i].ProductoID);

                            double cantidadEnKG = (Convert.ToDouble(ventaMayoristaInDB.ProductosXVenta[i].Cantidad) * (Convert.ToDouble(productoBlisterSegunLista.Gramos) / 1000)); //Convierto a KG
                            stockProdMix.Cantidad = stockProdMix.Cantidad + cantidadEnKG;

                           
                            //stockBL.UpdateStock(stockProdMix);
                            _UOWVentaMayorista.StockRepository.Update(stockProdMix);
                            
                        }

                    }

                }
                else if (ventaMayoristaInDB.ProductosXVenta[i].Producto.EsMix)
                {
                    //Producto Mix
                    var productosMixStock = stockBL.GetListaProductosMixById(prodId);

                    if (productosMixStock == null)
                        return BadRequest();

                    foreach (var prod in productosMixStock)
                    {

                        int prodDelMixId = prod.ProductoDelMixId.GetValueOrDefault();

                        Stock stockProdMix = _UOWVentaMayorista.StockRepository.GetAll().Where(s => s.ProductoID == prodDelMixId && s.TipoDeUnidadID == tipoUnidadId).SingleOrDefault();
                        //Stock stockProdMix = stockBL.ValidarStockProducto(prod.ProductoDelMixId.GetValueOrDefault(), ventaMayoristaInDB.ProductosXVenta[i].TipoDeUnidadID);

                        if (stockProdMix == null)
                            return BadRequest();

                        double cantidadEnKG = Convert.ToDouble(prod.Cantidad) * Convert.ToDouble(ventaMayoristaInDB.ProductosXVenta[i].Cantidad);
                        stockProdMix.Cantidad = stockProdMix.Cantidad + cantidadEnKG;

                        //stockBL.UpdateStock(stockProdMix);
                        _UOWVentaMayorista.StockRepository.Update(stockProdMix);

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

                    }
                    else
                    {
                        Stock stock = _UOWVentaMayorista.StockRepository.GetAll().Where(s => s.ProductoID == prodId && s.TipoDeUnidadID == tipoUnidadId).SingleOrDefault();
                        //Stock stock = stockBL.ValidarStockProducto(ventaMayoristaInDB.ProductosXVenta[i].ProductoID, ventaMayoristaInDB.ProductosXVenta[i].TipoDeUnidadID);
                        stock.Cantidad = stock.Cantidad + ventaMayoristaInDB.ProductosXVenta[i].Cantidad;

                        //stockBL.UpdateStock(stock);
                        _UOWVentaMayorista.StockRepository.Update(stock);
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

                ////Borramos Venta Mayorista
                var ventaABorrar = _UOWVentaMayorista.VentaMayoristaRepository.GetByID(Id);
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