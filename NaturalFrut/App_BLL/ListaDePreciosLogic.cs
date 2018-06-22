using NaturalFrut.App_BLL.Interfaces;
using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace NaturalFrut.App_BLL
{
    public class ListaDePreciosLogic
    {
        private readonly IRepository<ProductoXLista> productoXListaRP;
        private readonly IRepository<ListaDePrecios> listaDePreciosRP;
        private readonly IRepository<Cliente> clienteRP;
        private readonly IRepository<Producto> productoRP;
        private readonly IRepository<TipoDeUnidad> tipoDeUnidadRP;

        public ListaDePreciosLogic(IRepository<ProductoXLista> ProductoXListaRepository,
            IRepository<ListaDePrecios> ListaDePreciosRepository,
            IRepository<Cliente> ClienteRepository,
            IRepository<Producto> ProductoRepository,
            IRepository<TipoDeUnidad> TipoDeUnidadRepository)
        {
            productoXListaRP = ProductoXListaRepository;
            listaDePreciosRP = ListaDePreciosRepository;
            clienteRP = ClienteRepository;
            productoRP = ProductoRepository;
            tipoDeUnidadRP = TipoDeUnidadRepository;
        }

        public ListaDePreciosLogic(IRepository<ListaDePrecios> ListaDePreciosRepository)
        {            
            listaDePreciosRP = ListaDePreciosRepository;
        }

        public ListaDePreciosLogic(IRepository<ProductoXLista> ProductoXListaRepository)
        {
            productoXListaRP = ProductoXListaRepository;            
        }

        #region Operaciones ListaDePrecios
        public List<ListaDePrecios> GetAllListaDePrecios()
        {
            return listaDePreciosRP.GetAll().ToList();

        }

        public ListaDePrecios GetListaDePreciosById(int id)
        {
            return listaDePreciosRP.GetByID(id);

        }

        public void RemoveListaDePrecios(ListaDePrecios listaDePrecios)
        {
            listaDePreciosRP.Delete(listaDePrecios);
            listaDePreciosRP.Save();
        }

        public void AddListaDePrecios(ListaDePrecios listaDePrecios)
        {
            listaDePreciosRP.Add(listaDePrecios);
            listaDePreciosRP.Save();
        }

        public void UpdateListaDePrecios(ListaDePrecios listaDePrecios)
        {
            listaDePreciosRP.Update(listaDePrecios);
            listaDePreciosRP.Save();
        }
        #endregion

        #region Operaciones Genericas para Lista de Precios - Producto X Lista
        public List<ListaDePrecios> GetListaDePreciosList()
        {
            return listaDePreciosRP.GetAll().ToList();
        }

        public List<Cliente> GetClienteList()
        {
            return clienteRP.GetAll().ToList();
        }

        public List<Producto> GetProductoList()
        {
            return productoRP.GetAll().ToList();
        }

        public List<TipoDeUnidad> GetTipoDeUnidadList()
        {
            return tipoDeUnidadRP.GetAll().ToList();
        } 
        #endregion

        #region Operaciones ProductoXLista
        public List<ProductoXLista> GetAllProductosXLista()
        {
            return productoXListaRP.GetAll()
                .Include(c => c.ListaDePrecios)
                .Include(c => c.Cliente)
                .Include(c => c.Producto)
                .Include(c => c.TipoDeUnidad)
                .ToList();

        }

        public ProductoXLista GetProductoXListaById(int id)
        {
            return productoXListaRP.GetAll()
                .Include(c => c.ListaDePrecios)
                .Include(c => c.Cliente)
                .Include(c => c.Producto)
                .Include(c => c.TipoDeUnidad)
                .Where(c => c.ID == id).SingleOrDefault();

        }

        public void AddProductoXLista(ProductoXLista productoXLista)
        {
            productoXListaRP.Add(productoXLista);
            productoXListaRP.Save();
        }

        public void UpdateProductoXLista(ProductoXLista productoXLista)
        {
            productoXListaRP.Update(productoXLista);
            productoXListaRP.Save();
        }

        public void RemoveProductoXLista(ProductoXLista productoXLista)
        {
            productoXListaRP.Delete(productoXLista);
            productoXListaRP.Save();
        } 
        #endregion


    }
}