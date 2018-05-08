using NaturalFrut.App_BLL.Interfaces;
using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NaturalFrut.App_BLL
{
    public class ClienteLogic
    {

        private readonly IRepository<Cliente> clienteRP;
        private readonly IRepository<CondicionIVA> condicionIVARP;
        private readonly IRepository<TipoCliente> tipoClienteRP;

        public ClienteLogic(IRepository<Cliente> ClienteRepository, 
            IRepository<CondicionIVA> CondicionIVARepository,
            IRepository<TipoCliente> TipoClienteRepository)
        {
            clienteRP = ClienteRepository;
            condicionIVARP = CondicionIVARepository;
            tipoClienteRP = TipoClienteRepository;
        }

        public List<Cliente> GetAllClientes()
        {
            return clienteRP.GetAll().ToList();
        }

        public Cliente GetClienteById(int id)
        {
            return clienteRP.GetByID(id);
        }

        public void RemoveCliente(Cliente cliente)
        {
            clienteRP.Delete(cliente);
            clienteRP.Save();
        }

        public void AddCliente(Cliente cliente)
        {
            clienteRP.Add(cliente);
            clienteRP.Save();
        }

        internal void UpdateCliente(Cliente cliente)
        {
            clienteRP.Update(cliente);
            clienteRP.Save();
        }

        internal List<TipoCliente> GetTipoClienteList()
        {
            return tipoClienteRP.GetAll().ToList();
        }

        internal List<CondicionIVA> GetCondicionIvaList()
        {
            return condicionIVARP.GetAll().ToList();
        }
    }
}