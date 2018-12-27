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

    public class ProductosMixController : ApiController
    {
        
        private readonly ProductoLogic productoBL;

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            {
                log.Error("Formulario con datos inexistentes o incorrectos.");
                return BadRequest();
            }

            try
            {
                foreach (var producto in productosMixDTO.ProductoMix)
                {
                    var productoMix = Mapper.Map<ProductoMixDTO, ProductoMix>(producto);

                    productoBL.AddProductoMix(productoMix);
                }

                log.Info("Producto Mix con ID: " + productosMixDTO.ID + ", creado satisfactoriamente");

                return Ok();
            }
            catch (Exception ex)
            {
                log.Error("Se ha producido un error al intentar agregar un nuevo Producto Mix. Error: " + ex.Message);
                return BadRequest();
            }

            
        }

        //PUT /api/productosMix/1
        [HttpPut]
        public IHttpActionResult UpdateProductosMix(ProductosMixUpdateDTO productosDTO)
        {
            if (!ModelState.IsValid)
            {
                log.Error("Formulario con datos inexistentes o incorrectos.");
                return BadRequest();
            }

            try
            {
                //Actualizamos productos existentes
                foreach (var prodDTO in productosDTO.ProductosAnteriores)
                {
                    var productoMixInDB = productoBL.GetProductoDelMixById(prodDTO.ProdMixId, prodDTO.ProductoDelMixId);

                    if (productoMixInDB == null)
                    {
                        log.Error("Producto Mix no encontrado en la base de datos, ProdMixId: " + prodDTO.ProdMixId + " y ProductoDelMixId: " + prodDTO.ProductoDelMixId);
                        return NotFound();
                    }

                    productoMixInDB.Cantidad = prodDTO.Cantidad;

                    productoBL.UpdateProductoMix(productoMixInDB);

                    log.Info("ProdMixId: " + prodDTO.ProdMixId + " y ProductoDelMixId: " + prodDTO.ProductoDelMixId + ", Actualizado Satisfactoriamente");
                }

                //Si hay productos nuevos, los agregamos
                if (productosDTO.ProductosNuevos != null)
                {

                    foreach (var prodDTO in productosDTO.ProductosNuevos)
                    {
                        var productoMix = Mapper.Map<ProductoMixDTO, ProductoMix>(prodDTO);

                        productoBL.AddProductoMix(productoMix);

                        log.Info("ProdMixId: " + prodDTO.ProdMixId + " y ProductoDelMixId: " + prodDTO.ProductoDelMixId + ", Actualizado Satisfactoriamente");
                    }

                }


                return Ok();
            }
            catch (Exception ex)
            {
                log.Error("Se ha producido un error al intentar actualizar Producto Mix. Error: " + ex.Message);
                return BadRequest();
            }

            
        }

        //DELETE /api/productosmix/1
        [HttpDelete]        
        public IHttpActionResult DeleteProductoMix(int id)
        {

            var productoMixInDB = productoBL.GetProductoMixByIdReal(id);

            if (productoMixInDB == null)
            {
                log.Error("No se ha encontrado Producto Mix con ID: " + id);
                return NotFound();
            }

            productoBL.RemoveProductoMix(productoMixInDB);

            log.Info("Producto Mix eliminado satisfactoriamente, ID: " + id);

            return Ok();

        }

    }
}
