using NaturalFrut.App_BLL.Interfaces;
using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

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

        public ClienteLogic(IRepository<Cliente> ClienteRepository)
        {
            clienteRP = ClienteRepository;
        }

        public List<Cliente> GetAllClientes()
        {
            return clienteRP.GetAll()
                .Include(c => c.CondicionIVA)
                .Include(c => c.TipoCliente)
                .ToList();
        }

        public Cliente GetClienteById(int id)
        {
            return clienteRP
                .GetAll()
                .Include(c => c.TipoCliente)
                .Include(c => c.CondicionIVA)
                .Where(c => c.ID == id).SingleOrDefault();
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

        public void UpdateCliente(Cliente cliente)
        {
            clienteRP.Update(cliente);
            clienteRP.Save();
        }

        public List<TipoCliente> GetTipoClienteList()
        {
            return tipoClienteRP.GetAll().ToList();
        }

        public List<CondicionIVA> GetCondicionIvaList()
        {
            return condicionIVARP.GetAll().ToList();
        }
    }
}