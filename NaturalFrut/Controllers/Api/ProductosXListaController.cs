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

    public class ProductosXListaController : ApiController
    {
        
        private readonly ListaDePreciosLogic listaDePreciosBL;

        public ProductosXListaController(IRepository<ProductoXLista> ProductosXListaRepo)
        {
            listaDePreciosBL = new ListaDePreciosLogic(ProductosXListaRepo);
        }



        //GET /api/productosXLista
        public IEnumerable<ProductoXListaDTO> GetProductosXLista()
        {
            var productoXLista = listaDePreciosBL.GetAllProductosXLista();

            return productoXLista.Select(Mapper.Map<ProductoXLista, ProductoXListaDTO>);
        }

        //GET /api/ProductosXLista/1
        public IHttpActionResult GetProductosXLista(int id)
        {
            var productoXLista = listaDePreciosBL.GetProductoXListaById(id);

            if (productoXLista == null)
                return NotFound();

            return Ok(Mapper.Map<ProductoXLista, ProductoXListaDTO>(productoXLista));
        }

        //POST /api/ProductosXLista
        [HttpPost]
        public IHttpActionResult CreateProductosXLista(ProductoXListaDTO productoXListaDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var productoXLista = Mapper.Map<ProductoXListaDTO, ProductoXLista>(productoXListaDTO);

            listaDePreciosBL.AddProductoXLista(productoXLista);

            productoXListaDTO.ID = productoXLista.ID;

            return Created(new Uri(Request.RequestUri + "/" + productoXLista.ID), productoXListaDTO);
        }

        //PUT /api/ProductosXLista/1
        [HttpPut]
        public IHttpActionResult UpdateProductosXLista(int id, ProductoXListaDTO productoXListaDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var productoXListaInDB = listaDePreciosBL.GetProductoXListaById(id);

            if (productoXListaInDB == null)
                return NotFound();

            Mapper.Map(productoXListaDTO, productoXListaInDB);

            listaDePreciosBL.UpdateProductoXLista(productoXListaInDB);

            return Ok();
        }

        //DELETE /api/ProductosXLista/1
        [HttpDelete]
        public IHttpActionResult DeleteProductosXLista(int id)
        {

            var productoXListaInDB = listaDePreciosBL.GetProductoXListaById(id);

            if (productoXListaInDB == null)
                return NotFound();

            listaDePreciosBL.RemoveProductoXLista(productoXListaInDB);

            return Ok();

        }

    }
}
