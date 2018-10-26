using NaturalFrut.App_BLL.Interfaces;
using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NaturalFrut.App_BLL
{
    public class ProductoLogic
    {

        private readonly IRepository<Producto> productoRP;
        private readonly IRepository<Categoria> categoriaRP;
        private readonly IRepository<Marca> marcaRP;
        private readonly IRepository<Cliente> clienteRP;
        private readonly IRepository<ListaPrecio> listaPrecioRP;
        private readonly IRepository<ProductoMix> productoMixRP;
        private readonly IRepository<Stock> stockRP;

        public ProductoLogic(IRepository<Producto> ProductoRepository,
            IRepository<Categoria> CategoriaRepository,
            IRepository<Marca> MarcaRepository,
            IRepository<Cliente> ClienteRepository,
            IRepository<Stock> stockRepository,
            IRepository<ListaPrecio> ListaPrecioRepository,
            IRepository<ProductoMix> ProductoMixRepository)
        {
            productoRP = ProductoRepository;
            categoriaRP = CategoriaRepository;
            marcaRP = MarcaRepository;
            clienteRP = ClienteRepository;
            listaPrecioRP = ListaPrecioRepository;
            productoMixRP = ProductoMixRepository;
            stockRP = stockRepository;
        }

        public ProductoLogic(IRepository<Producto> ProductoRepository)
        {
            productoRP = ProductoRepository;
        }

        public ProductoLogic(IRepository<ProductoMix> ProductoMixRepository,
            IRepository<Producto> ProductoRepository)
        {
            productoMixRP = ProductoMixRepository;
            productoRP = ProductoRepository;
        }

        public ProductoLogic(IRepository<Stock> StockRepository)
        {
            stockRP = StockRepository;
        }

        public ProductoLogic(IRepository<Producto> ProductoRepository,
            IRepository<Cliente> ClienteRepository,
            IRepository<ListaPrecio> ListaPrecioRepository,
            IRepository<ProductoMix> ProductoMixRepository)
        {
            productoRP = ProductoRepository;
            clienteRP = ClienteRepository;
            listaPrecioRP = ListaPrecioRepository;
            productoMixRP = ProductoMixRepository;
        }

        public List<Producto> GetAllProducto()
        {
            return productoRP.GetAll()
                .Include(c => c.Categoria)
                .Include(c => c.Marca)
                .ToList();
        }

        public Producto GetProductoById(int id)
        {
            return productoRP
                .GetAll()
                .Include(c => c.Categoria)
                .Include(c => c.Marca)
                .Where(p => p.ID == id)
                .SingleOrDefault();
        }

        public void RemoveProducto(Producto producto)
        {
            productoRP.Delete(producto);
            productoRP.Save();
        }

        public void AddProducto(Producto producto)
        {
            productoRP.Add(producto);
            productoRP.Save();
        }

        public void UpdateProducto(Producto producto)
        {
            productoRP.Update(producto);
            productoRP.Save();
        }

        public List<Categoria> GetCategoriaList()
        {
            return categoriaRP.GetAll().ToList();
        }

        public List<Producto> GetAllProductosSegunFlagMix()
        {
            return productoRP
                .GetAll()
                .Where(p => p.EsMix == true)
                .ToList();
        }

        public List<Marca> GetMarcaList()
        {
            return marcaRP.GetAll().ToList();
        }

        public List<Producto> GetAllProductosSegunListaAsociada(int clienteID)
        {
            var cliente = clienteRP.GetByID(clienteID);
            int listaAsociada = cliente.ListaId;

            List<ListaPrecio> productosSegunLista = listaPrecioRP.GetAll()
                .Include(p => p.Producto)
                .Include(p => p.Producto.Categoria)
                .Include(p => p.Producto.Marca)
                .Where(p => p.ListaID == listaAsociada)
                .ToList();

            List<Producto> listaProductos = new List<Producto>();

            foreach (var prod in productosSegunLista)
            {
                listaProductos.Add(prod.Producto);
            }

            return listaProductos;
        }

        public List<Producto> GetAllProductosBlister()
        {
            return productoRP.GetAll()
                .Include(p => p.Categoria)
                .Include(p => p.Marca)
                .Where(p => p.EsBlister == true && p.EsMix == false)
                .ToList();
        }



        public List<ProductoMix> GetAllProductoMix()
        {

            var productos = productoMixRP.GetAll()
                .Include(c => c.ProductoDelMix)
                .Include(c => c.ProdMix)
                .ToList();

            return productos;
        }

        public ProductoMix GetProductoMixByIdReal(int id)
        {
            return productoMixRP
                .GetAll()
                .Include(c => c.ProductoDelMix)
                .Include(c => c.ProdMix)
                .Where(p => p.ID == id)
                .Distinct()
                .SingleOrDefault();
        }

        public void RemoveProductoMix(ProductoMix productoMix)
        {
            productoMixRP.Delete(productoMix);
            productoMixRP.Save();
        }

        public void AddProductoMix(ProductoMix productoMix)
        {
            productoMixRP.Add(productoMix);
            productoMixRP.Save();
        }

        public void UpdateProductoMix(ProductoMix productoMix)
        {
            productoMixRP.Update(productoMix);
            productoMixRP.Save();
        }

        public List<ProductoMix> GetListaProductosMixById(int prodMixId)
        {
            return productoMixRP
                .GetAll()
                .Include(c => c.ProductoDelMix)
                .Include(c => c.ProdMix)
                .Where(p => p.ProdMixId == prodMixId)
                .ToList();
        }

        public ProductoMix GetProductoDelMixById(int prodMixId ,int prodDelMixId)
        {
            return productoMixRP
                .GetAll()
                .Include(c => c.ProductoDelMix)
                .Include(c => c.ProdMix)
                .Where(p => p.ProductoDelMixId == prodDelMixId)
                .Where(p => p.ProdMixId == prodMixId)
                .SingleOrDefault();
        }
                
    }
}