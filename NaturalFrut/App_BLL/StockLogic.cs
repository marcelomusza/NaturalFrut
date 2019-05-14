using NaturalFrut.App_BLL.Interfaces;
using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using NaturalFrut.DTOs;

namespace NaturalFrut.App_BLL
{
    public class StockLogic
    {

        private readonly IRepository<Stock> StockRP;
        private readonly IRepository<Producto> ProductoRP;
        private readonly IRepository<TipoDeUnidad> TipoDeUnidadRP;
        private readonly IRepository<ProductoMix> ProductoMixRP;

        public StockLogic(IRepository<Stock> StockRepository,
           IRepository<Producto> ProductoRepository,
           IRepository<TipoDeUnidad> TipoDeUnidadRepository,
           IRepository<ProductoMix> ProductoMixRepository)
        {
            StockRP = StockRepository;
            ProductoRP = ProductoRepository;
            TipoDeUnidadRP = TipoDeUnidadRepository;
            ProductoMixRP = ProductoMixRepository;
        }

        

        public StockLogic(IRepository<Stock> StockRepository)
        {
            StockRP = StockRepository;
        }

        public StockLogic(IRepository<Stock> StockRepository, IRepository<ProductoMix> ProductoMixRepository)
        {
            StockRP = StockRepository;
            ProductoMixRP = ProductoMixRepository;
        }


        public List<Stock> GetAllStock()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;

            return StockRP.GetAll()
                .Include(p => p.Producto)
                .Include(t => t.TipoDeUnidad)
                .ToList();
        }

        public Stock GetStockById(int id)
        {
            return StockRP
                .GetAll()
                .Include(p => p.Producto)
                .Include(t => t.TipoDeUnidad)
                .Where(c => c.ID == id).SingleOrDefault();
        }

        public Stock GetStockByProductoId(int prodId)
        {
            return StockRP
                .GetAll()
                .Include(p => p.Producto)
                .Include(t => t.TipoDeUnidad)
                .Where(c => c.ProductoID == prodId)
                .SingleOrDefault();
        }


        public void RemoveStock(Stock stock)
        {
            StockRP.Delete(stock);
            StockRP.Save();
        }

        public void AddStock(Stock stock)
        {
            StockRP.Add(stock);
            StockRP.Save();
        }


        public void UpdateStock(Stock stock)
        {
            StockRP.Update(stock);
            StockRP.Save();
        }

       

        public Stock ValidarStockProducto(int productoID, int tipoUnidadID)
        {
            return StockRP
               .GetAll()
               .Include(p => p.Producto)
               .Include(t => t.TipoDeUnidad)
               .Where(p => p.ProductoID == productoID)
               .Where(t => t.TipoDeUnidadID == tipoUnidadID)
               .SingleOrDefault();
        }

        public List<TipoDeUnidad> GetTipoDeUnidadList()
        {
            return TipoDeUnidadRP.GetAll().ToList();
        }

        public List<ProductoMix> GetListaProductosMixById(int prodMixId)
        {
            return ProductoMixRP
                .GetAll()
                .Include(c => c.ProductoDelMix)
                .Include(c => c.ProdMix)
                .Where(p => p.ProdMixId == prodMixId)
                .ToList();
        }

        //public List<Stock> ValidarStockProductoMix(int productoID, int tipoDeUnidadID)
        //{

        //    var productosMix = GetListaProductosMixById(productoID);
        //    List<Stock> listaStockProdMix = new List<Stock>();

        //    foreach (var prodMix in productosMix)
        //    {
        //        Stock stockProd = StockRP
        //           .GetAll()
        //           .Include(p => p.Producto)
        //           .Include(t => t.TipoDeUnidad)
        //           .Where(p => p.ProductoID == prodMix.ProductoDelMix.ID)
        //           .Where(t => t.TipoDeUnidadID == tipoDeUnidadID)
        //           .SingleOrDefault();

        //        listaStockProdMix.Add(stockProd);
        //    }

        //    return listaStockProdMix;
        //}


        internal void UpdateStockAuxiliar()
        {
            var stock = StockRP.GetAll().ToList();

            foreach (var item in stock)
            {
                //Preparamos el Stock Auxiliar
                Producto prod = ProductoRP.GetByID(item.ProductoID);
                TipoDeUnidad tu = TipoDeUnidadRP.GetByID(item.TipoDeUnidadID);


                item.ProductoAuxiliar = prod.NombreAuxiliar;
                item.TipoDeUnidadAuxiliar = tu.Nombre;

                StockRP.Update(item);
                StockRP.Save();
            }


        }
    }
}