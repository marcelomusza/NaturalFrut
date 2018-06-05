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

    public class ProveedoresController : ApiController
    {
        
        private readonly ProveedorLogic proveedorBL;

        public ProveedoresController(IRepository<Proveedor> ProveedorRepo)
        {            
            proveedorBL = new ProveedorLogic(ProveedorRepo);
        }



        //GET /api/clientes
        public IEnumerable<ProveedorDTO> GetProveedores()
        {

            var proveedores = proveedorBL.GetAllProveedores();
            
            return proveedores.Select(Mapper.Map<Proveedor, ProveedorDTO>);
        }

        //GET /api/proveedor/1
        public IHttpActionResult GetProveedor(int id)
        {


            var proveedor = proveedorBL.GetProveedorById(id);

            if (proveedor == null)
                return NotFound();

            return Ok(Mapper.Map<Proveedor, ProveedorDTO>(proveedor));
        }

        //POST /api/proveedores
        [HttpPost]
        public IHttpActionResult CreateProveedor(ProveedorDTO proveedorDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var proveedor = Mapper.Map<ProveedorDTO, Proveedor>(proveedorDTO);

            proveedorBL.AddProveedor(proveedor);
            
            proveedorDTO.ID = proveedor.ID;

            return Created(new Uri(Request.RequestUri + "/" + proveedor.ID), proveedorDTO);
        }

        //PUT /api/proveedores/1
        [HttpPut]
        public IHttpActionResult UpdateProveedores(int id, ProveedorDTO proveedorDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var proveedorInDB = proveedorBL.GetProveedorById(id);

            if (proveedorInDB == null)
                return NotFound();

            Mapper.Map(proveedorDTO, proveedorInDB);

            proveedorBL.UpdateProveedor(proveedorInDB);

            return Ok();
        }

        //DELETE /api/proveedores/1
        [HttpDelete]
        public IHttpActionResult DeleteProveedor(int id)
        {

            var proveedorInDB = proveedorBL.GetProveedorById(id);

            if (proveedorInDB == null)
                return NotFound();

            proveedorBL.RemoveProveedor(proveedorInDB);
            
            return Ok();

        }

    }
}
