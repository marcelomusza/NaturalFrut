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

    public class ProductosMixController : ApiController
    {
        
        private readonly ProductoLogic productoBL;

        public ProductosMixController(IRepository<ProductoMix> ProductoMixRepo)
        {            
            productoBL = new ProductoLogic(ProductoMixRepo);
        }



        //GET /api/productosmix
        public IEnumerable<ProductoMixDTO> GetProductosMix()
        {

            var productosMix = productoBL.GetAllProductoMix();
                       
            return productosMix.Select(Mapper.Map<ProductoMix, ProductoMixDTO>);
        }

        

        //GET /api/productomix/1
        public IHttpActionResult GetProductoMix(int id)
        {


            var productoMix = productoBL.GetProductoMixById(id);

            if (productoMix == null)
                return NotFound();

            return Ok(Mapper.Map<ProductoMix, ProductoMixDTO>(productoMix));
        }

        //POST /api/productosmix
        [HttpPost]
        public IHttpActionResult CreateProductoMix(ProductoMixCreateDTO productosMixDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            foreach (var producto in productosMixDTO.ProductoMix)
            {
                var productoMix = Mapper.Map<ProductoMixDTO, ProductoMix>(producto);

                productoBL.AddProductoMix(productoMix);
            }

            return Ok();
        }

        //PUT /api/productosMix/1
        [HttpPut]
        public IHttpActionResult UpdateProductosMix(int id, ProductoMixDTO productoMixDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var productoMixInDB = productoBL.GetProductoMixById(id);

            if (productoMixInDB == null)
                return NotFound();

            Mapper.Map(productoMixDTO, productoMixInDB);

            productoBL.UpdateProductoMix(productoMixInDB);

            return Ok();
        }

        //DELETE /api/productosmix/1
        [HttpDelete]
        public IHttpActionResult DeleteProductoMix(int id)
        {

            var productoMixInDB = productoBL.GetProductoMixById(id);

            if (productoMixInDB == null)
                return NotFound();

            productoBL.RemoveProductoMix(productoMixInDB);
            
            return Ok();

        }

    }
}
