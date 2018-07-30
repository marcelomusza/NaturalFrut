using NaturalFrut.App_BLL.Interfaces;
using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace NaturalFrut.App_BLL
{
    public class StockLogic
    {

        private readonly IRepository<Stock> StockRP;
        private readonly IRepository<Producto> ProductoRP;
        private readonly IRepository<TipoDeUnidad> TipoDeUnidadRP;

        public StockLogic(IRepository<Stock> StockRepository,
           IRepository<Producto> ProductoRepository,
           IRepository<TipoDeUnidad> TipoDeUnidadRepository)
        {
            StockRP = StockRepository;
            ProductoRP = ProductoRepository;
            TipoDeUnidadRP = TipoDeUnidadRepository;
        }

        public StockLogic(IRepository<Stock> StockRepository)
        {
            StockRP = StockRepository;
        }


        public List<Stock> GetAllStock()
        {
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
                
    }
}