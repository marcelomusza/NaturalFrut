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

    public class ListaPreciosBlisterController : ApiController
    {
        
        private readonly ListaPreciosLogic listaPreciosBL;

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
                return NotFound();

            return Ok(Mapper.Map<ListaPrecioBlister, ListaPrecioBlisterDTO>(listaPrecioBlister));
        }

        //POST /api/listaPreciosBlister
        [HttpPost]
        public IHttpActionResult CreateListaPrecios(ListaPrecioBlisterDTO listaPrecioBlisterDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var listaPrecioBlister = Mapper.Map<ListaPrecioBlisterDTO, ListaPrecioBlister>(listaPrecioBlisterDTO);

            listaPreciosBL.AddListaPrecioBlister(listaPrecioBlister);

            listaPrecioBlisterDTO.ID = listaPrecioBlister.ID;

            return Created(new Uri(Request.RequestUri + "/" + listaPrecioBlister.ID), listaPrecioBlisterDTO);
        }

        //PUT /api/listaPreciosBlister/1
        [HttpPut]
        public IHttpActionResult UpdateListaPrecios(int id, ListaPrecioBlisterDTO listaPrecioBlisterDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var listaPrecioBlisterInDB = listaPreciosBL.GetListaPrecioBlisterById(id);

            if (listaPrecioBlisterInDB == null)
                return NotFound();

            Mapper.Map(listaPrecioBlisterDTO, listaPrecioBlisterInDB);

            listaPreciosBL.UpdateListaPrecioBlister(listaPrecioBlisterInDB);

            return Ok();
        }

        //DELETE /api/listaPreciosBlister/1
        [HttpDelete]
        public IHttpActionResult DeleteListaPrecios(int id)
        {

            var listaPrecioBlisterInDB = listaPreciosBL.GetListaPrecioBlisterById(id);

            if (listaPrecioBlisterInDB == null)
                return NotFound();

            listaPreciosBL.RemoveListaPrecioBlister(listaPrecioBlisterInDB);

            return Ok();

        }

    }
}
