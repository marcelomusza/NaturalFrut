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

        public ProductoLogic(IRepository<Producto> ProductoRepository, 
            IRepository<Categoria> CategoriaRepository, 
            IRepository<Marca> MarcaRepository,
            IRepository<Cliente> ClienteRepository,
            IRepository<ListaPrecio> ListaPrecioRepository)
        {
            productoRP = ProductoRepository;
            categoriaRP = CategoriaRepository;
            marcaRP = MarcaRepository;
            clienteRP = ClienteRepository;
            listaPrecioRP = ListaPrecioRepository;
        }

        public ProductoLogic(IRepository<Producto> ProductoRepository, 
            IRepository<Cliente> ClienteRepository,
            IRepository<ListaPrecio> ListaPrecioRepository)
        {
            productoRP = ProductoRepository;
            clienteRP = ClienteRepository;
            listaPrecioRP = ListaPrecioRepository;
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

    }
}