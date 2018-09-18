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

    public class ClasificacionController : ApiController
    {
        
        private readonly CommonLogic clasificacionBL;

        public ClasificacionController(IRepository<Clasificacion> ClasificacionRepo)
        {            
            clasificacionBL = new CommonLogic(ClasificacionRepo);
        }



        //GET /api/clasificacion
        public IEnumerable<ClasificacionDTO> GetClasificacion()
        {

            var clasificaciones = clasificacionBL.GetAllClasificacion();
            
            return clasificaciones.Select(Mapper.Map<Clasificacion, ClasificacionDTO>);
           
        }

        //GET /api/clasificacion/1
        public IHttpActionResult GetClasificacion(int id)
        {


            var clasificacion = clasificacionBL.GetClasificacionById(id);

            if (clasificacion == null)
                return NotFound();

            return Ok(Mapper.Map<Clasificacion, ClasificacionDTO>(clasificacion));
        }

        //POST /api/clasificaciones
        [HttpPost]
        public IHttpActionResult CreateClasificacion(ClasificacionDTO clasificacionDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var clasificacion = Mapper.Map<ClasificacionDTO, Clasificacion>(clasificacionDTO);

            clasificacionBL.AddClasificacion(clasificacion);
            
            clasificacionDTO.ID = clasificacion.ID;

            return Created(new Uri(Request.RequestUri + "/" + clasificacion.ID), clasificacionDTO);
        }

        //PUT /api/clasificacion/1
        [HttpPut]
        public IHttpActionResult UpdateClasificacion(int id, ClasificacionDTO clasificacionDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var clasificacionInDB = clasificacionBL.GetClasificacionById(id);

            if (clasificacionInDB == null)
                return NotFound();

            Mapper.Map(clasificacionDTO, clasificacionInDB);

            clasificacionBL.UpdateClasificacion(clasificacionInDB);

            return Ok();
        }

        //DELETE /api/clasificacion/1
        [HttpDelete]
        public IHttpActionResult DeleteClasificacion(int id)
        {

            var clasificacionInDB = clasificacionBL.GetClasificacionById(id);

            if (clasificacionInDB == null)
                return NotFound();

            clasificacionBL.RemoveClasificacion(clasificacionInDB);
            
            return Ok();

        }

    }
}
