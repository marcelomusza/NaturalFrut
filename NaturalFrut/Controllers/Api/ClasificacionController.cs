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

    public class ClasificacionController : ApiController
    {
        
        private readonly CommonLogic clasificacionBL;

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            {
                log.Error("No se ha podido traer la lista de Clasificaciones");
                return NotFound();
            }
                

            return Ok(Mapper.Map<Clasificacion, ClasificacionDTO>(clasificacion));
        }

        //POST /api/clasificaciones
        [HttpPost]
        public IHttpActionResult CreateClasificacion(ClasificacionDTO clasificacionDTO)
        {
            if (!ModelState.IsValid)
            {
                log.Error("No se ha llenado todos los campos correctamente para Clasificacion");
                return BadRequest();
            }

            var clasificacion = Mapper.Map<ClasificacionDTO, Clasificacion>(clasificacionDTO);

            clasificacionBL.AddClasificacion(clasificacion);
            
            clasificacionDTO.ID = clasificacion.ID;

            log.Info("Clasificacion creada: " + clasificacion.Nombre);

            return Created(new Uri(Request.RequestUri + "/" + clasificacion.ID), clasificacionDTO);
        }

        //PUT /api/clasificacion/1
        [HttpPut]
        public IHttpActionResult UpdateClasificacion(int id, ClasificacionDTO clasificacionDTO)
        {
            if (!ModelState.IsValid)
            {
                log.Error("No se ha llenado todos los campos correctamente para Clasificacion");
                return BadRequest();
            }

            var clasificacionInDB = clasificacionBL.GetClasificacionById(id);

            if (clasificacionInDB == null)
            {
                log.Error("No se ha podido traer la Clasificacion de la base de datos con ID: " + id);
                return NotFound();
            }

            Mapper.Map(clasificacionDTO, clasificacionInDB);

            clasificacionBL.UpdateClasificacion(clasificacionInDB);

            log.Info("Clasificacion actualizada, Nombre: " + clasificacionInDB.Nombre);

            return Ok();
        }

        //DELETE /api/clasificacion/1
        [HttpDelete]
        public IHttpActionResult DeleteClasificacion(int id)
        {

            var clasificacionInDB = clasificacionBL.GetClasificacionById(id);

            if (clasificacionInDB == null)
            { 
                log.Error("No se ha podido traer la clasificación con ID: " + id);
                return NotFound();
            }

            clasificacionBL.RemoveClasificacion(clasificacionInDB);

            log.Info("Clasificacion " + clasificacionInDB.Nombre + " borrada exitosamente...");
            
            return Ok();

        }

    }
}
