using NaturalFrut.App_BLL;
using NaturalFrut.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using NaturalFrut.Models;
using NaturalFrut.App_BLL.Interfaces;
using System.Data.Entity;

namespace NaturalFrut.Controllers.Api
{

    public class StockController : ApiController
    {

        private readonly StockLogic stockBL;
        private readonly ProductoLogic productoBL;

        public StockController(IRepository<Stock> StockRepo, IRepository<Producto> ProductoRepo)
        {
            stockBL = new StockLogic(StockRepo);
            productoBL = new ProductoLogic(ProductoRepo);
        }



        //GET /api/stock
        public IEnumerable<StockDTO> GetStock()
        {

            var stocks = stockBL.GetAllStock();

            return stocks.Select(Mapper.Map<Stock, StockDTO>);
        }

        ////GET /api/stock/1
        //public IHttpActionResult GetStock(int id)
        //{


        //    var stock = stockBL.GetStockById(id);

        //    if (stock == null)
        //        return NotFound();

        //    return Ok(Mapper.Map<Stock, StockDTO>(stock));
        //}

        //POST /api/clientes
        //[HttpPost]
        //public IHttpActionResult AltaStock(StockDTO stockDTO)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest();

        //    var stock = Mapper.Map<StockDTO, Stock>(stockDTO);

        //    stockBL.AddStock(stock);

        //    stockDTO.ID = stock.ID;

        //    return Created(new Uri(Request.RequestUri + "/" + stock.ID), stockDTO);
        //}

        //POST /api/stock
        [HttpPost]
        public IHttpActionResult CreateStock(StockDTO stock)
        {
            if (!ModelState.IsValid)
                return BadRequest();


            Producto producto = productoBL.GetProductoById(stock.ProductoID);


            Stock stockIngresado = stockBL.ValidarStockProducto(stock.ProductoID, stock.TipoDeUnidadID);

            if (stockIngresado != null)
            {
                stockIngresado.Cantidad = stockIngresado.Cantidad + stock.Cantidad;
                stockBL.UpdateStock(stockIngresado);

            }
            else
            {
                Stock stockNuevo = new Stock();

                stockNuevo.ProductoID = stock.ProductoID;
                stockNuevo.TipoDeUnidadID = stock.TipoDeUnidadID;
                stock.Cantidad = stock.Cantidad;

                stockBL.AddStock(stockNuevo);


            }



            return Ok();
        }

        ////PUT /api/clientes/1
        //[HttpPut]
        //public IHttpActionResult UpdateCliente(int id, ClienteDTO clienteDTO)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest();

        //    var clienteInDB = clienteBL.GetClienteById(id);

        //    if (clienteInDB == null)
        //        return NotFound();

        //    Mapper.Map(clienteDTO, clienteInDB);

        //    clienteBL.UpdateCliente(clienteInDB);

        //    return Ok();
        //}

        ////DELETE /api/clientes/1
        //[HttpDelete]
        //public IHttpActionResult DeleteCliente(int id)
        //{

        //    var clienteInDB = clienteBL.GetClienteById(id);

        //    if (clienteInDB == null)
        //        return NotFound();

        //    clienteBL.RemoveCliente(clienteInDB);

        //    return Ok();

        //}

    }
}
