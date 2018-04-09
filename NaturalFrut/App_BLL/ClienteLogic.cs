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

        public ClienteLogic(IRepository<Cliente> ClienteRepository)
        {
            clienteRP = ClienteRepository;
        }

        public List<Cliente> GetAllClientes()
        {
            return clienteRP.GetAll().ToList();
        }
    }
}