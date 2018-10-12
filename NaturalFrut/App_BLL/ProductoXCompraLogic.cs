using NaturalFrut.App_BLL.Interfaces;
using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace NaturalFrut.App_BLL
{
    public class ProductoXCompraLogic
    {

        private readonly IRepository<ProductoXCompra> ProductoXCompraRP;
        private readonly IRepository<Producto> ProductoRP;
        private readonly IRepository<TipoDeUnidad> TipoDeUnidadRP;
        private readonly IRepository<Compra> CompraRP;

        public ProductoXCompraLogic(IRepository<ProductoXCompra> ProductoXCompraRepository,
           IRepository<Producto> ProductoRepository,
           IRepository<TipoDeUnidad> TipoDeUnidadRepository,
           IRepository<Compra> CompraRepository)
        {
            ProductoXCompraRP = ProductoXCompraRepository;
            ProductoRP = ProductoRepository;
            TipoDeUnidadRP = TipoDeUnidadRepository;
            CompraRP = CompraRepository;
        }

        public ProductoXCompraLogic(IRepository<ProductoXCompra> ProductoXCompraRepository)
        {
            ProductoXCompraRP = ProductoXCompraRepository;
        }

        public List<ProductoXCompra> GetAllProductoXCompra()
        {
            return ProductoXCompraRP.GetAll()
                .Include(p => p.Producto)
                .Include(t => t.TipoDeUnidad)
                .Include(v => v.Compra)
                .ToList();
        }

        public ProductoXCompra GetProductoXCompraById(int id)
        {
            return ProductoXCompraRP
                .GetAll()
                .Include(p => p.Producto)
                .Include(t => t.TipoDeUnidad)
                .Include(v => v.Compra)
                .Where(c => c.ID == id).SingleOrDefault();
        }


        public void RemoveProductoXCompra(ProductoXCompra ProductoXCompra)
        {
            ProductoXCompraRP.Delete(ProductoXCompra);
            ProductoXCompraRP.Save();
        }

        public void AddProductoXCompra(ProductoXCompra ProductoXCompra)
        {
            ProductoXCompraRP.Add(ProductoXCompra);
            ProductoXCompraRP.Save();
        }


        public void UpdateProductoXCompra(ProductoXCompra ProductoXCompra)
        {
            ProductoXCompraRP.Update(ProductoXCompra);
            ProductoXCompraRP.Save();
        }



        public List<ProductoXCompra> GetProductoXCompraByIdCompra(int compraID)
        {
            return ProductoXCompraRP
               .GetAll()
               .Include(p => p.Producto)
               .Include(t => t.TipoDeUnidad)
               .Where(v => v.CompraID == compraID)
               .ToList();
        }

    }
}