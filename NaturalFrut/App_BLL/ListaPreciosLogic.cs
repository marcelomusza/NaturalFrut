using NaturalFrut.App_BLL.Interfaces;
using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace NaturalFrut.App_BLL
{
    public class ListaPreciosLogic
    {
        private readonly IRepository<ListaPrecio> listaPrecioRP;
        private readonly IRepository<Lista> listaRP;
        private readonly IRepository<Cliente> clienteRP;
        private readonly IRepository<Producto> productoRP;
        private readonly IRepository<TipoDeUnidad> tipoDeUnidadRP;

        public ListaPreciosLogic(IRepository<ListaPrecio> ListaPrecioRepository,
            IRepository<Lista> ListaRepository,
            IRepository<Cliente> ClienteRepository,
            IRepository<Producto> ProductoRepository,
            IRepository<TipoDeUnidad> TipoDeUnidadRepository)
        {
            listaPrecioRP = ListaPrecioRepository;
            listaRP = ListaRepository;
            clienteRP = ClienteRepository;
            productoRP = ProductoRepository;
            tipoDeUnidadRP = TipoDeUnidadRepository;
        }

        public ListaPreciosLogic(IRepository<Lista> ListaRepository)
        {            
            listaRP = ListaRepository;
        }

        public ListaPreciosLogic(IRepository<ListaPrecio> ListaPrecioRepository)
        {
            listaPrecioRP = ListaPrecioRepository;            
        }

        #region Operaciones Lista
        public List<Lista> GetAllLista()
        {
            return listaRP.GetAll().ToList();

        }

        public Lista GetListaById(int id)
        {
            return listaRP.GetByID(id);

        }

        public void RemoveLista(Lista lista)
        {
            listaRP.Delete(lista);
            listaRP.Save();
        }

        public void AddLista(Lista lista)
        {
            listaRP.Add(lista);
            listaRP.Save();
        }

        public void UpdateLista(Lista lista)
        {
            listaRP.Update(lista);
            listaRP.Save();
        }
        #endregion

        #region Operaciones Genericas para Lista - ListaPrecios
        public List<Lista> GetListaList()
        {
            return listaRP.GetAll().ToList();
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

        #region Operaciones ListaPrecio
        public List<ListaPrecio> GetAllListaPrecio()
        {
            return listaPrecioRP.GetAll()
                .Include(c => c.Lista)
                .Include(c => c.Producto)
                .ToList();

        }

        public ListaPrecio GetListaPrecioById(int id)
        {
            return listaPrecioRP.GetAll()
                .Include(c => c.Lista)
                .Include(c => c.Producto)
                .Where(c => c.ID == id).SingleOrDefault();

        }

        public void AddListaPrecio(ListaPrecio ListaPrecio)
        {
            
            var listas = GetAllLista();
            
            foreach (var lista in listas)
            {
                ListaPrecio listaModificada = new ListaPrecio();
                
                listaModificada.ListaID = lista.ID;
                listaModificada.ProductoID = ListaPrecio.ProductoID;
                listaModificada.KGBultoCerrado = ListaPrecio.KGBultoCerrado;
                listaModificada.PrecioXUnidad = Math.Round(((ListaPrecio.PrecioXUnidad * lista.PorcentajeAumento) / 100) + ListaPrecio.PrecioXUnidad, 2);
                listaModificada.PrecioXKG = Math.Round(((ListaPrecio.PrecioXKG * lista.PorcentajeAumento) / 100) + ListaPrecio.PrecioXKG, 2);
                listaModificada.PrecioXBultoCerrado = Math.Round(((ListaPrecio.PrecioXBultoCerrado * lista.PorcentajeAumento) / 100) + ListaPrecio.PrecioXBultoCerrado, 2);

                listaPrecioRP.Add(listaModificada);
                listaPrecioRP.Save();

            }

            
        }

        public void UpdateListaPrecio(ListaPrecio ListaPrecio)
        {
            listaPrecioRP.Update(ListaPrecio);
            listaPrecioRP.Save();
        }

        public void RemoveListaPrecio(ListaPrecio ListaPrecio)
        {
            listaPrecioRP.Delete(ListaPrecio);
            listaPrecioRP.Save();
        } 
        #endregion


    }
}