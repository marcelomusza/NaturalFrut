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
using NaturalFrut.App_BLL.ViewModels;

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

        //PUT /api/stock
        [HttpPut]
        public IHttpActionResult UpdateStock(StockViewModel stockUpdate)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            Stock stockmodif = new Stock();

            stockmodif.ProductoID = stockUpdate.ProductoID;
            stockmodif.TipoDeUnidadID = stockUpdate.TipoDeUnidadID;
            stockmodif.ID = stockUpdate.ID;



            if (!stockUpdate.isDelete)
            {
                stockmodif.Cantidad = stockUpdate.NuevaCantidad + stockUpdate.Cantidad;
                stockBL.UpdateStock(stockmodif);


            }else
            {
                stockmodif.Cantidad = stockUpdate.Cantidad - stockUpdate.NuevaCantidad ;
                stockBL.UpdateStock(stockmodif);
            }

            return Ok();
        }

 
    }
}
