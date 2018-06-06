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

    public class VendedoresController : ApiController
    {
        
        private readonly VendedorLogic vendedorBL;

        public VendedoresController(IRepository<Vendedor> VendedorRepo)
        {            
            vendedorBL = new VendedorLogic(VendedorRepo);
        }



        //GET /api/vendedores
        public IEnumerable<VendedorDTO> GetVendedores()
        {

            var vendedores = vendedorBL.GetAllVendedores();
            
            return vendedores.Select(Mapper.Map<Vendedor, VendedorDTO>);
           
        }

        //GET /api/vendedores/1
        public IHttpActionResult GetVendedor(int id)
        {


            var vendedor = vendedorBL.GetVendedorById(id);

            if (vendedor == null)
                return NotFound();

            return Ok(Mapper.Map<Vendedor, VendedorDTO>(vendedor));
        }

        //POST /api/vendedores
        [HttpPost]
        public IHttpActionResult CreateVendedor(VendedorDTO vendedorDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var vendedor = Mapper.Map<VendedorDTO, Vendedor>(vendedorDTO);

            vendedorBL.AddVendedor(vendedor);
            
            vendedorDTO.ID = vendedor.ID;

            return Created(new Uri(Request.RequestUri + "/" + vendedor.ID), vendedorDTO);
        }

        //PUT /api/vendedores/1
        [HttpPut]
        public IHttpActionResult UpdateVendedor(int id, VendedorDTO vendedorDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var vendedorInDB = vendedorBL.GetVendedorById(id);

            if (vendedorInDB == null)
                return NotFound();

            Mapper.Map(vendedorDTO, vendedorInDB);

            vendedorBL.UpdateVendedor(vendedorInDB);

            return Ok();
        }

        //DELETE /api/vendedores/1
        [HttpDelete]
        public IHttpActionResult DeleteVendedor(int id)
        {

            var vendedorInDB = vendedorBL.GetVendedorById(id);

            if (vendedorInDB == null)
                return NotFound();

            vendedorBL.RemoveVendedor(vendedorInDB);
            
            return Ok();

        }

    }
}
