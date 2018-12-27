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

    public class ProveedoresController : ApiController
    {
        
        private readonly ProveedorLogic proveedorBL;

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            {
                log.Error("Proveedor no encontrado con ID: " + id);
                return NotFound();
            }

            return Ok(Mapper.Map<Proveedor, ProveedorDTO>(proveedor));
        }

        //POST /api/proveedores
        [HttpPost]
        public IHttpActionResult CreateProveedor(ProveedorDTO proveedorDTO)
        {
            if (!ModelState.IsValid)
            {
                log.Error("Formulario con datos incorrectos o insuficientes.");
                return BadRequest();
            }
                

            var proveedor = Mapper.Map<ProveedorDTO, Proveedor>(proveedorDTO);

            proveedorBL.AddProveedor(proveedor);
            
            proveedorDTO.ID = proveedor.ID;

            log.Info("Proveedor creado satisfactoriamente, ID: " + proveedor.ID);

            return Created(new Uri(Request.RequestUri + "/" + proveedor.ID), proveedorDTO);
        }

        //PUT /api/proveedores/1
        [HttpPut]
        public IHttpActionResult UpdateProveedores(int id, ProveedorDTO proveedorDTO)
        {
            if (!ModelState.IsValid)
            {
                log.Error("Formulario con datos incorrectos o insuficientes.");
                return BadRequest();
            }

            var proveedorInDB = proveedorBL.GetProveedorById(id);

            if (proveedorInDB == null)
            {
                log.Error("Proveedor no encontrado en la base de datos con ID: " + id);
                return NotFound();
            }

            Mapper.Map(proveedorDTO, proveedorInDB);

            proveedorBL.UpdateProveedor(proveedorInDB);

            log.Info("Proveedor actualizado satisfactoriamente, ID: " + id);

            return Ok();
        }

        //DELETE /api/proveedores/1
        [HttpDelete]
        public IHttpActionResult DeleteProveedor(int id)
        {

            var proveedorInDB = proveedorBL.GetProveedorById(id);

            if (proveedorInDB == null)
            {
                log.Error("Proveedor no encontrado en la base de datos con ID: " + id);
                return NotFound();
            }

            proveedorBL.RemoveProveedor(proveedorInDB);

            log.Info("Proveedor eliminado satisfactoriamente, ID: " + id);
            
            return Ok();

        }

    }
}
