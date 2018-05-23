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

        public ProductoLogic(IRepository<Producto> ProductoRepository, IRepository<Categoria> CategoriaRepository)
        {
            productoRP = ProductoRepository;
            categoriaRP = CategoriaRepository;
        }

        public List<Producto> GetAllProducto()
        {
            return productoRP.GetAll()
                .Include(c => c.Categoria)
                .ToList();
        }

        public Producto GetProductoById(int id)
        {
            return productoRP
                .GetAll()
                .Include(c => c.Categoria)
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
    }
}