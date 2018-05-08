using NaturalFrut.App_BLL.Interfaces;
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

        public CommonLogic(IRepository<CondicionIVA> CondicionIVARepository,
            IRepository<TipoCliente> TipoClienteRepository)
        {
            condicionIVARP = CondicionIVARepository;
            tipoClienteRP = TipoClienteRepository;
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
    }
}