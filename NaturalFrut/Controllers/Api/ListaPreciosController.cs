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

    public class ListaPreciosController : ApiController
    {
        
        private readonly ListaPreciosLogic listaPreciosBL;

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ListaPreciosController(IRepository<ListaPrecio> ListaPrecioRepo)
        {
            listaPreciosBL = new ListaPreciosLogic(ListaPrecioRepo);
        }



        //GET /api/listaPrecios
        public IEnumerable<ListaPrecioDTO> GetListaPrecios()
        {
            var listaPrecio = listaPreciosBL.GetAllListaPrecio();

            return listaPrecio.Select(Mapper.Map<ListaPrecio, ListaPrecioDTO>);
        }

        //GET /api/listaPrecios/1
        public IHttpActionResult GetListaPrecios(int id)
        {
            var listaPrecio = listaPreciosBL.GetListaPrecioById(id);

            if (listaPrecio == null)
            {
                log.Error("Lista de Precios no encontrada con ID: " + id);
                return NotFound();
            }

            return Ok(Mapper.Map<ListaPrecio, ListaPrecioDTO>(listaPrecio));
        }

        //POST /api/listaPrecios
        [HttpPost]
        public IHttpActionResult CreateListaPrecios(ListaPrecioDTO listaPrecioDTO)
        {
            if (!ModelState.IsValid)
            {
                log.Error("Formulario con datos incorrectos o inexistentes.");
                return BadRequest();
            }

            var listaPrecio = Mapper.Map<ListaPrecioDTO, ListaPrecio>(listaPrecioDTO);

            listaPreciosBL.AddListaPrecio(listaPrecio);

            listaPrecioDTO.ID = listaPrecio.ID;

            log.Info("Lista de Precios creada satisfactoriamente. ID:" + listaPrecio.ID);

            return Created(new Uri(Request.RequestUri + "/" + listaPrecio.ID), listaPrecioDTO);
        }

        //PUT /api/listaPrecios/1
        [HttpPut]
        public IHttpActionResult UpdateListaPrecios(int id, ListaPrecioDTO listaPrecioDTO)
        {
            if (!ModelState.IsValid)
            {
                log.Error("Formulario con datos incorrectos o inexistentes.");
                return BadRequest();
            }

            var listaPrecioInDB = listaPreciosBL.GetListaPrecioById(id);

            if (listaPrecioInDB == null)
            {
                log.Error("Lista de Precios no encontrada en la base de datos con ID: " + id);
                return NotFound();
            }

            Mapper.Map(listaPrecioDTO, listaPrecioInDB);

            listaPreciosBL.UpdateListaPrecio(listaPrecioInDB);

            log.Info("Lista de Precios actualizada satisfactoriamente, ID: " + id);

            return Ok();
        }

        //DELETE /api/listaPrecios/1
        [HttpDelete]
        public IHttpActionResult DeleteListaPrecios(int id)
        {

            var listaPrecioInDB = listaPreciosBL.GetListaPrecioById(id);

            if (listaPrecioInDB == null)
            {
                log.Error("Lista de Precios no encontrada en la base de datos con ID: " + id);
                return NotFound();
            }

            listaPreciosBL.RemoveListaPrecio(listaPrecioInDB);

            log.Info("Lista de Precios eliminada satisfactoriamente. ID: " + id);

            return Ok();

        }

    }
}
