using NaturalFrut.App_BLL.Interfaces;
using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace NaturalFrut.App_BLL
{
    public class CompraLogic
    {

        private readonly IRepository<Compra> compraRP;
        private readonly IRepository<Proveedor> proveedorRP;
        private readonly IRepository<Clasificacion> clasificacionRP;
        private readonly IRepository<Producto> productoRP;
        private readonly IRepository<ProductoXCompra> productoXCompraRP;
        

        public CompraLogic(IRepository<Compra> CompraRepository,
            IRepository<Proveedor> ProveedorRepository,
            IRepository<Clasificacion> ClasificacionRepository,
            IRepository<Producto> ProductoRepository,
            IRepository<ProductoXCompra> ProductoXCompraRepository)
        {
            compraRP = CompraRepository;
            proveedorRP = ProveedorRepository;
            clasificacionRP = ClasificacionRepository;
            productoRP = ProductoRepository;
            productoXCompraRP = ProductoXCompraRepository;
        }

        public CompraLogic(IRepository<Compra> CompraRepository)
        {
            compraRP = CompraRepository;
           
        }

        public CompraLogic(IRepository<Proveedor> ProveedorRepository)
        {
            proveedorRP = ProveedorRepository;
        }        

        public Compra GetNumeroDeCompra()
        {
            var ultimaCompra = compraRP.GetAll().OrderByDescending(p => p.ID).FirstOrDefault();

            return ultimaCompra;
        }

        public List<Compra> GetAllCompra()
        {
            return compraRP.GetAll()
                .Include(p => p.Proveedor)
                .Include(c => c.Clasificacion)
                .ToList();
        }

        public Compra GetCompraById(int id)
        {
            Compra compra = compraRP
                .GetAll()
                .Include(p => p.Proveedor)
                .Include(c => c.Clasificacion)
                .Include(p => p.ProductosXCompra)                
                .Include("ProductosXCompra.TipoDeUnidad")
                .Include("ProductosXCompra.Producto")
                .Include("ProductosXCompra.Producto.Marca")
                .Include("ProductosXCompra.Producto.Categoria")
                .Where(c => c.ID == id).SingleOrDefault();

            return compra;
        }


        public void RemoveCompra(Compra compra)
        {
            compraRP.Delete(compra);
            compraRP.Save();
        }

        public void AddCompra(Compra compra)
        {            
            compraRP.Add(compra);
            compraRP.Save();
        }


        public void UpdateCompra(Compra compra)
        {
            compraRP.Update(compra);
            compraRP.Save();
        }

        public List<Proveedor> GetProveedorList()
        {
            return proveedorRP.GetAll().ToList();
        }

        public List<Clasificacion> GetClasificacionList()
        {
            return clasificacionRP.GetAll().ToList();
        }

        public List<Producto> GetProductoList()
        {
            return productoRP.GetAll().ToList();
        }



        public bool ValidateTipoDeProducto(int productoID)
        {

            Producto producto = productoRP.GetByID(productoID);

            if (producto != null)
                return producto.EsBlister;
            else
                throw new Exception("Error al Validar tipo de Producto");


        }

        //public ListaPrecioBlister CalcularImporteBlisterSegunCliente(int productoID)
        //{

        //    var productoSegunLista = listaPreciosBlisterRP.GetAll()
        //        .Where(p => p.ProductoID == productoID)
        //        .SingleOrDefault();

        //    return productoSegunLista;
        //}

    }
}