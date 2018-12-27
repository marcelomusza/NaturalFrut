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

    public class ListaPreciosBlisterController : ApiController
    {
        
        private readonly ListaPreciosLogic listaPreciosBL;

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ListaPreciosBlisterController(IRepository<ListaPrecioBlister> ListaPrecioRepo)
        {
            listaPreciosBL = new ListaPreciosLogic(ListaPrecioRepo);
        }



        //GET /api/listaPreciosblister
        public IEnumerable<ListaPrecioBlisterDTO> GetListaPreciosBlister()
        {
            var listaPrecioBlister = listaPreciosBL.GetAllListaPrecioBlister();

            return listaPrecioBlister.Select(Mapper.Map<ListaPrecioBlister, ListaPrecioBlisterDTO>);
        }

        //GET /api/listaPreciosBlister/1
        public IHttpActionResult GetListaPreciosBlister(int id)
        {
            var listaPrecioBlister = listaPreciosBL.GetListaPrecioBlisterById(id);

            if (listaPrecioBlister == null)
            {
                log.Error("Lista de precios Blister no encontrada con ID: " + id);
                return NotFound();
            }

            return Ok(Mapper.Map<ListaPrecioBlister, ListaPrecioBlisterDTO>(listaPrecioBlister));
        }

        //POST /api/listaPreciosBlister
        [HttpPost]
        public IHttpActionResult CreateListaPrecios(ListaPrecioBlisterDTO listaPrecioBlisterDTO)
        {
            if (!ModelState.IsValid)
            {
                log.Error("Formulario de Lista Precios con datos invalidos o inexistentes.");
                return BadRequest();
            }

            var listaPrecioBlister = Mapper.Map<ListaPrecioBlisterDTO, ListaPrecioBlister>(listaPrecioBlisterDTO);

            listaPreciosBL.AddListaPrecioBlister(listaPrecioBlister);

            listaPrecioBlisterDTO.ID = listaPrecioBlister.ID;

            log.Info("ListaPreciosBlister creado satisfactoriamente. ID: " + listaPrecioBlister.ID);

            return Created(new Uri(Request.RequestUri + "/" + listaPrecioBlister.ID), listaPrecioBlisterDTO);
        }

        //PUT /api/listaPreciosBlister/1
        [HttpPut]
        public IHttpActionResult UpdateListaPrecios(int id, ListaPrecioBlisterDTO listaPrecioBlisterDTO)
        {
            if (!ModelState.IsValid)
            {
                log.Error("Formulario de Lista Precios con datos invalidos o inexistentes.");
                return BadRequest();
            }

            var listaPrecioBlisterInDB = listaPreciosBL.GetListaPrecioBlisterById(id);

            if (listaPrecioBlisterInDB == null)
                return NotFound();

            Mapper.Map(listaPrecioBlisterDTO, listaPrecioBlisterInDB);

            listaPreciosBL.UpdateListaPrecioBlister(listaPrecioBlisterInDB);

            log.Info("ListaPreciosBlister actualizado satisfactoriamente. ID: " + id);

            return Ok();
        }

        //DELETE /api/listaPreciosBlister/1
        [HttpDelete]
        public IHttpActionResult DeleteListaPrecios(int id)
        {

            var listaPrecioBlisterInDB = listaPreciosBL.GetListaPrecioBlisterById(id);

            if (listaPrecioBlisterInDB == null)
            {
                log.Error("ListaPreciosBlister no encontrado en la base de datos con ID: " + id);
                return NotFound();
            }

            listaPreciosBL.RemoveListaPrecioBlister(listaPrecioBlisterInDB);

            log.Info("ListaPreciosBlister eliminado satisfactoriamente, ID: " + id);

            return Ok();

        }

    }
}
