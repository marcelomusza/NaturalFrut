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
using log4net;

namespace NaturalFrut.Controllers.Api
{

    public class ClientesController : ApiController
    {
        
        private readonly ClienteLogic clienteBL;

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            {
                log.Error("Cliente no encontrado con ID: " + id);
                return NotFound();
            }
                

            return Ok(Mapper.Map<Cliente, ClienteDTO>(cliente));
        }

        //POST /api/clientes
        [HttpPost]
        public IHttpActionResult CreateCliente(ClienteDTO clienteDTO)
        {
            if (!ModelState.IsValid)
            {
                log.Error("Formulario con datos invalidos o insuficientes.");
                return BadRequest();
            }
                

            var cliente = Mapper.Map<ClienteDTO, Cliente>(clienteDTO);

            clienteBL.AddCliente(cliente);
            
            clienteDTO.ID = cliente.ID;

            log.Info("Cliente: " + cliente.Nombre + ", agregado satisfactoriamente");

            return Created(new Uri(Request.RequestUri + "/" + cliente.ID), clienteDTO);
        }

        //PUT /api/clientes/1
        [HttpPut]
        public IHttpActionResult UpdateCliente(int id, ClienteDTO clienteDTO)
        {
            if (!ModelState.IsValid)
            {
                log.Error("Formulario con datos invalidos o insuficientes.");
                return BadRequest();
            }

            var clienteInDB = clienteBL.GetClienteById(id);

            if (clienteInDB == null)
            {
                log.Error("Cliente no encontrado con ID: " + id);
                return NotFound();
            }

            Mapper.Map(clienteDTO, clienteInDB);

            clienteBL.UpdateCliente(clienteInDB);

            log.Info("Cliente: " + clienteInDB.Nombre + ", actualizado satisfactoriamente");

            return Ok();
        }

        //DELETE /api/clientes/1
        [HttpDelete]
        public IHttpActionResult DeleteCliente(int id)
        {

            var clienteInDB = clienteBL.GetClienteById(id);

            if (clienteInDB == null)
            {
                log.Error("Cliente no encontrado con ID: " + id);
                return NotFound();
            }

            clienteBL.RemoveCliente(clienteInDB);

            log.Info("Cliente eliminado satisfactoriamente. ID: " + id);
            
            return Ok();

        }

        //GET /api/clientes/1
        [HttpGet]
        [Route("api/clientes/reportesaldocliente/{clienteID}")]
        public IHttpActionResult GetSaldoCliente(int clienteID)
        {


            var cliente = clienteBL.GetClienteById(clienteID);

            if (cliente == null)
            {
                log.Error("Cliente no encontrado con ID: " + clienteID);
                return NotFound();
            }

            return Ok(Mapper.Map<Cliente, ClienteDTO>(cliente));
        }


    }
}
