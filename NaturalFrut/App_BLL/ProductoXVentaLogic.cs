using NaturalFrut.App_BLL.Interfaces;
using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace NaturalFrut.App_BLL
{
    public class ProductoXVentaLogic
    {

        private readonly IRepository<ProductoXVenta> ProductoXVentaRP;
        private readonly IRepository<Producto> ProductoRP;
        private readonly IRepository<TipoDeUnidad> TipoDeUnidadRP;
        private readonly IRepository<VentaMayorista> VentaMayoristaRP;

        public ProductoXVentaLogic(IRepository<ProductoXVenta> ProductoXVentaRepository,
           IRepository<Producto> ProductoRepository,
           IRepository<TipoDeUnidad> TipoDeUnidadRepository,
           IRepository<VentaMayorista> VentaMayoristaRepository)
        {
            ProductoXVentaRP = ProductoXVentaRepository;
            ProductoRP = ProductoRepository;
            TipoDeUnidadRP = TipoDeUnidadRepository;
            VentaMayoristaRP = VentaMayoristaRepository;
        }

        public ProductoXVentaLogic(IRepository<ProductoXVenta> ProductoXVentaRepository)
        {
            ProductoXVentaRP = ProductoXVentaRepository;
        }

        public List<ProductoXVenta> GetAllProductoXVenta()
        {
            return ProductoXVentaRP.GetAll()
                .Include(p => p.Producto)
                .Include(t => t.TipoDeUnidad)
                .Include(v => v.Venta)
                .ToList();
        }

        public ProductoXVenta GetProductoXVentaById(int id)
        {
            return ProductoXVentaRP
                .GetAll()
                .Include(p => p.Producto)
                .Include(t => t.TipoDeUnidad)
                .Include(v => v.Venta)
                .Where(c => c.ID == id).SingleOrDefault();
        }


        public void RemoveProductoXVenta(ProductoXVenta ProductoXVenta)
        {
            ProductoXVentaRP.Delete(ProductoXVenta);
            ProductoXVentaRP.Save();
        }

        public void AddProductoXVenta(ProductoXVenta ProductoXVenta)
        {
            ProductoXVentaRP.Add(ProductoXVenta);
            ProductoXVentaRP.Save();
        }


        public void UpdateProductoXVenta(ProductoXVenta ProductoXVenta)
        {
            ProductoXVentaRP.Update(ProductoXVenta);
            ProductoXVentaRP.Save();
        }



        public List<ProductoXVenta> GetProductoXVentaByIdVenta(int ventaID)
        {
            return ProductoXVentaRP
               .GetAll()
               .Include(p => p.Producto)
               .Include(t => t.TipoDeUnidad)
               .Where(v => v.VentaID == ventaID)
               .ToList();
        }
    
    }
}