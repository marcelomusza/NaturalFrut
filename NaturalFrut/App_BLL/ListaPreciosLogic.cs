﻿using NaturalFrut.App_BLL.Interfaces;
using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Globalization;


namespace NaturalFrut.App_BLL
{
    public class ListaPreciosLogic
    {
        private readonly IRepository<ListaPrecio> listaPrecioRP;
        private readonly IRepository<Lista> listaRP;
        private readonly IRepository<Cliente> clienteRP;
        private readonly IRepository<Producto> productoRP;
        private readonly IRepository<TipoDeUnidad> tipoDeUnidadRP;
        private readonly IRepository<ListaPrecioBlister> listaPrecioBlisterRP;

        public ListaPreciosLogic(IRepository<ListaPrecio> ListaPrecioRepository,
            IRepository<Lista> ListaRepository,
            IRepository<Cliente> ClienteRepository,
            IRepository<Producto> ProductoRepository,
            IRepository<TipoDeUnidad> TipoDeUnidadRepository,
            IRepository<ListaPrecioBlister> ListaPrecioBlisterRepository)
        {
            listaPrecioRP = ListaPrecioRepository;
            listaRP = ListaRepository;
            clienteRP = ClienteRepository;
            productoRP = ProductoRepository;
            tipoDeUnidadRP = TipoDeUnidadRepository;
            listaPrecioBlisterRP = ListaPrecioBlisterRepository;
        }

        public ListaPreciosLogic(IRepository<Lista> ListaRepository)
        {            
            listaRP = ListaRepository;
        }

        public ListaPreciosLogic(IRepository<ListaPrecio> ListaPrecioRepository)
        {
            listaPrecioRP = ListaPrecioRepository;            
		}

        public ListaPreciosLogic(IRepository<ListaPrecioBlister> ListaPrecioBlisterRepository)
        {
            listaPrecioBlisterRP = ListaPrecioBlisterRepository;
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
             return productoRP.GetAll()
                .Where(p => p.EsBlister != true)
                .ToList();
        }

        public List<Producto> GetProductoBlisterList()
        {
            return productoRP.GetAll()
                .Where(p => p.EsBlister == true)
                .ToList();
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


        public List<ListaPrecio> GetListaPrecioByIdProducto(int id , int idVenta)
        {
            return listaPrecioRP.GetAll()
                .Include(c => c.Lista)
                .Include(c => c.Producto)
                .Where(c => c.ProductoID == id)
                .Where(c => c.ID != idVenta).ToList();

        }

        public void AddListaPrecio(ListaPrecio ListaPrecio)
        {

            var listas = GetAllLista();
 
            foreach (var lista in listas)
            {
                listaPrecioRP.Add(CalculaPrecios(ListaPrecio, lista, false, null));
                listaPrecioRP.Save();

            }

        }

        public void UpdateListaPrecio(ListaPrecio ListaPrecio)
        {
            List<ListaPrecio> lista = new List<ListaPrecio>();

            lista = GetListaPrecioByIdProducto(ListaPrecio.ProductoID , ListaPrecio.ID);
            listaPrecioRP.Update(ListaPrecio);
            listaPrecioRP.Save();


            foreach (var listas in lista)
            {            
                listaPrecioRP.Update(CalculaPrecios(ListaPrecio, listas.Lista, true, listas));
                listaPrecioRP.Save();
            }


           
        }

        public void RemoveListaPrecio(ListaPrecio ListaPrecio)
        {
            listaPrecioRP.Delete(ListaPrecio);
            listaPrecioRP.Save();
        }
        #endregion

        public ListaPrecio CalculaPrecios(ListaPrecio ListaPrecio, Lista lista, bool var, ListaPrecio ListaPrecioMod)
        {

            decimal precioXKG;
            decimal unidad;
            decimal bulto;
            ListaPrecio listaModificada;
            CultureInfo.CurrentCulture = new CultureInfo("es-AR");

            if (var)
            {
                listaModificada = ListaPrecioMod;

            }
            else
            {
                listaModificada = new ListaPrecio();

            }

            precioXKG = decimal.Parse(ListaPrecio.PrecioXKG, System.Globalization.NumberStyles.AllowDecimalPoint, new System.Globalization.CultureInfo("es-AR"));
            unidad = decimal.Parse(ListaPrecio.PrecioXUnidad, System.Globalization.NumberStyles.AllowDecimalPoint, new System.Globalization.CultureInfo("es-AR"));
            bulto = decimal.Parse(ListaPrecio.PrecioXBultoCerrado, System.Globalization.NumberStyles.AllowDecimalPoint, new System.Globalization.CultureInfo("es-AR"));

           
            listaModificada.ListaID = lista.ID;
            listaModificada.ProductoID = ListaPrecio.ProductoID;
            listaModificada.KGBultoCerrado = ListaPrecio.KGBultoCerrado;
            unidad = Math.Round((((unidad) * lista.PorcentajeAumento) / 100) + unidad, 2);
            listaModificada.PrecioXUnidad = Convert.ToString(unidad);
            precioXKG = Math.Round((((precioXKG) * lista.PorcentajeAumento) / 100) + precioXKG,2);
            listaModificada.PrecioXKG = Convert.ToString(precioXKG);

            bulto = Math.Round((((bulto) * lista.PorcentajeAumento) / 100) + bulto,2);
            listaModificada.PrecioXBultoCerrado = Convert.ToString(bulto);

            return listaModificada;
        }

#region Operaciones Lista Precio Blister
        public List<ListaPrecioBlister> GetAllListaPrecioBlister()
        {
            return listaPrecioBlisterRP.GetAll()
                .Include(p => p.Producto)
                .ToList();

        }

        public ListaPrecioBlister GetListaPrecioBlisterById(int id)
        {
            return listaPrecioBlisterRP.GetAll()
                .Include(c => c.Producto)
                .Where(c => c.ID == id).SingleOrDefault();

        }

        public void AddListaPrecioBlister(ListaPrecioBlister listaPrecioBlister)
        {
            
            listaPrecioBlisterRP.Add(listaPrecioBlister);
            listaPrecioBlisterRP.Save();

        }

        public void UpdateListaPrecioBlister(ListaPrecioBlister listaPrecioBlister)
        {
            listaPrecioBlisterRP.Update(listaPrecioBlister);
            listaPrecioBlisterRP.Save();
        }

        public void RemoveListaPrecioBlister(ListaPrecioBlister listaPrecioBlister)
        {
            listaPrecioBlisterRP.Delete(listaPrecioBlister);
            listaPrecioBlisterRP.Save();
        }
        #endregion


        }
}