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
using log4net;

namespace NaturalFrut.Controllers.Api
{

    public class StockController : ApiController
    {

        private readonly StockLogic stockBL;
        private readonly ProductoLogic productoBL;
        private readonly CommonLogic tipoDeUnidadBL;

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public StockController(IRepository<Stock> StockRepo, IRepository<Producto> ProductoRepo, IRepository<TipoDeUnidad> TipoDeUnidadRepo)
        {
            stockBL = new StockLogic(StockRepo);
            productoBL = new ProductoLogic(ProductoRepo);
            tipoDeUnidadBL = new CommonLogic(TipoDeUnidadRepo);
        }



        //GET /api/stock
        public IEnumerable<StockDTO> GetStock()
        {

            var stocks = stockBL.GetAllStock();

            return stocks.Select(Mapper.Map<Stock, StockDTO>);
        }

        [HttpGet]
        [Route("api/stock/stocksinrelaciones")]
        public IEnumerable<Stock> GetStockSinRelaciones()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;

            List<Stock> stock = db.Stock.ToList();

            return stock;
        }


        //POST /api/stock
        [HttpPost]
        public IHttpActionResult CreateStock(StockDTO stock)
        {
            if (!ModelState.IsValid)
            {
                log.Error("Formulario con datos inexistentes o incorrectos.");
                return BadRequest();
            }


            Producto producto = productoBL.GetProductoById(stock.ProductoID);
            TipoDeUnidad tunidad = tipoDeUnidadBL.GetTipoDeUnidadById(stock.TipoDeUnidadID);

            Stock stockIngresado = stockBL.ValidarStockProducto(stock.ProductoID, stock.TipoDeUnidadID);

            if (stockIngresado != null)
            {
                stockIngresado.Cantidad = stockIngresado.Cantidad + stock.Cantidad;
                stockBL.UpdateStock(stockIngresado);

                log.Info("Stock Actualizado satisfactoriamente. ID: " + stockIngresado.ID);

            }
            else
            {
                Stock stockNuevo = new Stock();

                stockNuevo.ProductoID = stock.ProductoID;
                stockNuevo.TipoDeUnidadID = stock.TipoDeUnidadID;
                stockNuevo.Cantidad = stock.Cantidad;
                stockNuevo.ProductoAuxiliar = producto.NombreAuxiliar;
                stockNuevo.TipoDeUnidadAuxiliar = tunidad.Nombre;

                stockBL.AddStock(stockNuevo);

                log.Info("Stock Agregado satisfactoriamente");


            }



            return Ok();
        }

        //PUT /api/stock
        [HttpPut]
        public IHttpActionResult UpdateStock(StockViewModel stockUpdate)
        {
            if (!ModelState.IsValid)
            {
                log.Error("Formulario con datos inexistentes o incorrectos.");
                return BadRequest();
            }

            Stock stockmodif = new Stock();

            stockmodif.ProductoID = stockUpdate.ProductoID;
            stockmodif.TipoDeUnidadID = stockUpdate.TipoDeUnidadID;
            stockmodif.ID = stockUpdate.ID;
            stockmodif.ProductoAuxiliar = stockUpdate.ProductoAuxiliar;
            stockmodif.TipoDeUnidadAuxiliar = stockUpdate.TipoDeUnidadAuxiliar;
            
            if (!stockUpdate.isDelete)
            {
                stockmodif.Cantidad = stockUpdate.NuevaCantidad + stockUpdate.Cantidad;
                stockBL.UpdateStock(stockmodif);


            }else
            {
                stockmodif.Cantidad = stockUpdate.Cantidad - stockUpdate.NuevaCantidad ;
                stockBL.UpdateStock(stockmodif);
            }

            log.Info("Stock actualizado satisfactoriamente, ID: " + stockmodif.ID);


            return Ok();
        }

 
    }
}
