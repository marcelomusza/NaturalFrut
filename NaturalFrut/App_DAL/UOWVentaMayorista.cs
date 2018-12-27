using log4net;
using NaturalFrut.App_BLL.Interfaces;
using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NaturalFrut.App_DAL
{
    public class UOWVentaMayorista : IDisposable
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        private BaseRepository<VentaMayorista> ventaMayoristaRP;
        private BaseRepository<Cliente> clienteRP;
        private BaseRepository<Stock> stockRP;
        private BaseRepository<ProductoXVenta> prodXVentaRP;

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public IRepository<VentaMayorista> VentaMayoristaRepository
        {
            get
            {
                if (ventaMayoristaRP == null)
                {
                    ventaMayoristaRP = new BaseRepository<VentaMayorista>(_context);
                }
                return ventaMayoristaRP;
            }
        }

        public IRepository<Cliente> ClienteRepository
        {
            get
            {
                if (clienteRP == null)
                {
                    clienteRP = new BaseRepository<Cliente>(_context);
                }
                return clienteRP;
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

        public IRepository<ProductoXVenta> ProductosXVentaRepository
        {
            get
            {
                if (prodXVentaRP == null)
                {
                    prodXVentaRP = new BaseRepository<ProductoXVenta>(_context);
                }
                return prodXVentaRP;
            }
        }


        public void Save()
        {
            _context.SaveChanges();

            log.Info("Datos salvados satisfactoriamente en la base de datos. Unity of Work VENTA MAYORISTA");
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