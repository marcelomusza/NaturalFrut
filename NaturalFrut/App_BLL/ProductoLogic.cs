using NaturalFrut.App_BLL.Interfaces;
using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NaturalFrut.App_BLL
{
    public class ProductoLogic
    {

        private readonly IRepository<Producto> productoRP;      

        public ProductoLogic(IRepository<Producto> ProductoRepository)
        {
            productoRP = ProductoRepository;
        }

        public List<Producto> GetAllProducto()
        {
            return productoRP.GetAll().ToList();
        }

        public Producto GetProductoById(int id)
        {
            return productoRP.GetByID(id);
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

    }
}