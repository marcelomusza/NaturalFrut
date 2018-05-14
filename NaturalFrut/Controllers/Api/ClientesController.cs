using NaturalFrut.App_BLL;
using NaturalFrut.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using NaturalFrut.Models;
using NaturalFrut.App_BLL.Interfaces;
using System.Data.Entity;

namespace NaturalFrut.Controllers.Api
{
    public class ClientesController : ApiController
    {
        
        private readonly ClienteLogic clienteBL;

        public ClientesController(IRepository<Cliente> ClienteRepo)
        {            
            clienteBL = new ClienteLogic(ClienteRepo);
        }



        //GET /api/clientes
        public IEnumerable<ClienteDTO> GetClientes()
        {

            var clientes = clienteBL.GetAllClientes();
            
            return clientes.Select(Mapper.Map<Cliente, ClienteDTO>);
        }

        //GET /api/clientes/1
        public IHttpActionResult GetCliente(int id)
        {


            var cliente = clienteBL.GetClienteById(id);

            if (cliente == null)
                return NotFound();

            return Ok(Mapper.Map<Cliente, ClienteDTO>(cliente));
        }

    }
}
