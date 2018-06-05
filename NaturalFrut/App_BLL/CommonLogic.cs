﻿using NaturalFrut.App_BLL.Interfaces;
using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NaturalFrut.App_BLL
{
    public class CommonLogic
    {

        private readonly IRepository<CondicionIVA> condicionIVARP;
        private readonly IRepository<TipoCliente> tipoClienteRP;
        private readonly IRepository<Categoria> categoriaRP;
        private readonly IRepository<Marca> marcaRP;

        public CommonLogic(IRepository<CondicionIVA> CondicionIVARepository,
            IRepository<TipoCliente> TipoClienteRepository,
            IRepository<Categoria> CategoriaRepository,
            IRepository<Marca> MarcaRepository)
        {
            condicionIVARP = CondicionIVARepository;
            tipoClienteRP = TipoClienteRepository;
            categoriaRP = CategoriaRepository;
            marcaRP = MarcaRepository;
        }


        #region Operaciones Condicion IVA
        public List<CondicionIVA> GetAllCondicionIVA()
        {
            return condicionIVARP.GetAll().ToList();
        }

        public CondicionIVA GetCondicionIVAById(int id)
        {
            return condicionIVARP.GetByID(id);
        }

        public void RemoveCondicionIVA(CondicionIVA condicionIVA)
        {
            condicionIVARP.Delete(condicionIVA);
            condicionIVARP.Save();
        }

        public void AddCondicionIVA(CondicionIVA condicionIVA)
        {
            condicionIVARP.Add(condicionIVA);
            condicionIVARP.Save();
        }

        public void UpdateCondicionIVA(CondicionIVA condicionIVA)
        {
            condicionIVARP.Update(condicionIVA);
            condicionIVARP.Save();
        }
        #endregion

        #region Operaciones Tipo Cliente
        public List<TipoCliente> GetAllTipoCliente()
        {
            return tipoClienteRP.GetAll().ToList();
        }

        public TipoCliente GetTipoClienteById(int id)
        {
            return tipoClienteRP.GetByID(id);
        }

        public void RemoveTipoCliente(TipoCliente tipoCliente)
        {
            tipoClienteRP.Delete(tipoCliente);
            tipoClienteRP.Save();
        }

        public void AddTipoCliente(TipoCliente tipoCliente)
        {
            tipoClienteRP.Add(tipoCliente);
            tipoClienteRP.Save();
        }

        public void UpdateTipoCliente(TipoCliente tipoCliente)
        {
            tipoClienteRP.Update(tipoCliente);
            tipoClienteRP.Save();
        }
        #endregion

        #region Operaciones Categoria
        public List<Categoria> GetAllCategorias()
        {
            return categoriaRP.GetAll().ToList();
        }

        public Categoria GetCategoriaById(int id)
        {
            return categoriaRP.GetByID(id);
        }

        public void RemoveCategoria(Categoria categoria)
        {
            categoriaRP.Delete(categoria);
            categoriaRP.Save();
        }

        public void AddCategoria(Categoria categoria)
        {
            categoriaRP.Add(categoria);
            categoriaRP.Save();
        }

        public void UpdateCategoria(Categoria categoria)
        {
            categoriaRP.Update(categoria);
            categoriaRP.Save();
        }
        #endregion

        #region Operaciones Marca
        public void AddMarca(Marca marca)
        {
            marcaRP.Add(marca);
            marcaRP.Save();
        }

        public void UpdateMarca(Marca marca)
        {
            marcaRP.Update(marca);
            marcaRP.Save();
        }

        public Marca GetMarcaById(int id)
        {
            return marcaRP.GetByID(id);
        }

        public void RemoveMarca(Marca marca)
        {
            marcaRP.Delete(marca);
            marcaRP.Save();
        }

        public List<Marca> GetAllMarcas()
        {
            return marcaRP.GetAll().ToList();
        } 
        #endregion


    }
}