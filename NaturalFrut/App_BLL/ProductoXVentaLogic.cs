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
               .Include("Producto.Marca")
               .Include("Producto.Categoria")
               .Include(t => t.TipoDeUnidad)
               .Where(v => v.VentaID == ventaID)
               .OrderBy(p => p.Producto.Marca.Nombre).ThenBy(p=>p.Producto.Categoria.Nombre).ThenBy(p => p.Producto.Nombre)
               .ToList();
        }

        public ProductoXVenta GetProductoXVentaIndividualById(BorrarProdVtaMayDTO prodVenta)
        {

            return ProductoXVentaRP
            .GetAll()
            .Include(p => p.Producto)
            .Include(t => t.TipoDeUnidad)
            .Include(v => v.Venta)
            .Where(c => c.VentaID == prodVenta.VentaID && c.ProductoID == prodVenta.ProductoID && c.TipoDeUnidadID == prodVenta.TipoDeUnidadID).SingleOrDefault();
                                   
        }

        public List<ReporteProductosVendidos> GetProductoXVentaByIdProducto(int id)
        {

            ApplicationDbContext db = new ApplicationDbContext();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;

            var prods = (from lp in db.ProductoXVenta
                         join p in db.Productos on lp.ProductoID equals p.ID
                         join t in db.TipoDeUnidad on lp.TipoDeUnidadID equals t.ID
                         join v in db.Ventas on lp.VentaID equals v.ID
                         join c in db.Clientes on lp.Venta.ClienteID equals c.ID
                         where lp.ProductoID == id
                         select new ReporteProductosVendidos
                         {
                             FechaVenta = v.Fecha,
                             NombreProducto = p.NombreAuxiliar,
                             NumeroVenta = v.NumeroVenta,
                             NombreCliente = c.Nombre,
                             Cantidad = lp.Cantidad,
                             Importe = lp.Importe
                         }).ToList();

            return prods;

            //return ProductoXVentaRP
            //  .GetAll()
            //  .Include(p => p.Producto)
            //  .Include(t => t.TipoDeUnidad)
            //  .Include(v => v.Venta)
            //  .Include("Venta.Cliente")
            //  .Where(p => p.ProductoID == id)
            //  .ToList();
        }
    }
}