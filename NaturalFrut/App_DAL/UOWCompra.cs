using log4net;
using NaturalFrut.App_BLL.Interfaces;
using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NaturalFrut.App_DAL
{
    public class UOWCompra : IDisposable
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        private BaseRepository<Compra> compraRP;
        private BaseRepository<Proveedor> proveedorRP;
        private BaseRepository<Stock> stockRP;
        private BaseRepository<ProductoXCompra> prodXCompraRP;

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public IRepository<Compra> CompraRepository
        {
            get
            {
                if (compraRP == null)
                {
                    compraRP = new BaseRepository<Compra>(_context);
                }
                return compraRP;
            }
        }

        public IRepository<Proveedor> ProveedorRepository
        {
            get
            {
                if (proveedorRP == null)
                {
                    proveedorRP = new BaseRepository<Proveedor>(_context);
                }
                return proveedorRP;
            }
        }

        public IRepository<Stock> StockRepository
        {
            get
            {
                if (stockRP == null)
                {
                    stockRP = new BaseRepository<Stock>(_context);
                }
                return stockRP;
            }
        }

        public IRepository<ProductoXCompra> ProductosXCompraRepository
        {
            get
            {
                if (prodXCompraRP == null)
                {
                    prodXCompraRP = new BaseRepository<ProductoXCompra>(_context);
                }
                return prodXCompraRP;
            }
        }


        public void Save()
        {
            _context.SaveChanges();

            log.Info("Datos salvados satisfactoriamente en la base de datos. Unity of Work COMPRA");
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}