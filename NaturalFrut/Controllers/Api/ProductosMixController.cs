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

        public ProductosMixController(IRepository<ProductoMix> ProductoMixRepo,
            IRepository<Producto> ProductoRepo)
        {            
            productoBL = new ProductoLogic(ProductoMixRepo, ProductoRepo);
        }



        //GET /api/productosmix
        public IEnumerable<ProductoMixDTO> GetProductosMix()
        {

            //var productosMix = productoBL.GetAllProductosSegunFlagMix();
            var productosMix = productoBL.GetAllProductoMix();

           
            
                       
            return productosMix.Select(Mapper.Map<ProductoMix, ProductoMixDTO>);
        }

        

        ////GET /api/productomix/1
        //public IHttpActionResult GetProductoMix(int id)
        //{


        //    var productoMix = productoBL.GetProductoMixById(id);

        //    if (productoMix == null)
        //        return NotFound();

        //    return Ok(Mapper.Map<ProductoMix, ProductoMixDTO>(productoMix));
        //}

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
        public IHttpActionResult UpdateProductosMix(ProductosMixUpdateDTO productosDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            //Actualizamos productos existentes
            foreach (var prodDTO in productosDTO.ProductosAnteriores)
            {
                var productoMixInDB = productoBL.GetProductoDelMixById(prodDTO.ProdMixId, prodDTO.ProductoDelMixId);

                if (productoMixInDB == null)
                    return NotFound();

                productoMixInDB.Cantidad = prodDTO.Cantidad;

                productoBL.UpdateProductoMix(productoMixInDB);

            }

            //Si hay productos nuevos, los agregamos
            if(productosDTO.ProductosNuevos != null)
            {

                foreach (var prodDTO in productosDTO.ProductosNuevos)
                {
                    var productoMix = Mapper.Map<ProductoMixDTO, ProductoMix>(prodDTO);

                    productoBL.AddProductoMix(productoMix);
                }

            }


            return Ok();
        }

        //DELETE /api/productosmix/1
        [HttpDelete]        
        public IHttpActionResult DeleteProductoMix(int id)
        {

            var productoMixInDB = productoBL.GetProductoMixByIdReal(id);

            if (productoMixInDB == null)
                return NotFound();

            productoBL.RemoveProductoMix(productoMixInDB);

            return Ok();

        }

    }
}
