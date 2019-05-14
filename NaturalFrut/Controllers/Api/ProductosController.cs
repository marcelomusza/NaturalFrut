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

    public class ProductosController : ApiController
    {
        
        private readonly ProductoLogic productoBL;
        private readonly ClienteLogic clienteBL;

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ProductosController(IRepository<Producto> ProductoRepo,
            IRepository<Cliente> ClienteRepo,
            IRepository<ListaPrecio> ListaPrecioRepo,
            IRepository<ProductoMix> ProductoMixRepo)
        {            
            productoBL = new ProductoLogic(ProductoRepo, ClienteRepo, ListaPrecioRepo, ProductoMixRepo);
            clienteBL = new ClienteLogic(ClienteRepo);
        }



        //GET /api/productos
        public IEnumerable<ProductoDTO> GetProductos()
        {

            var productos = productoBL.GetAllProducto();

            //foreach (var prod in productos)
            //{
            //    if(prod.Marca != null)
            //        prod.Nombre = prod.Nombre + " (" + prod.Marca.Nombre + ")";

            //    if(prod.Categoria != null)
            //        prod.Nombre = prod.Nombre + " (" + prod.Categoria.Nombre + ")";
            //}
            
            return productos.Select(Mapper.Map<Producto, ProductoDTO>);
        }

        //GET /api/productos
        [HttpGet]
        [Route("compra/api/productos/productossinrelaciones")]
        [Route("api/productos/productossinrelaciones")]
        [Route("admin/api/productos/productossinrelaciones")]
        public IEnumerable<Producto> GetProductosSinRelaciones()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;

            List<Producto> productos = db.Productos.ToList();
            //List<Marca> marcas = db.Marcas.ToList();
            //List<Categoria> categorias = db.Categorias.ToList();

            //foreach (var prod in productos)
            //{
            //    if (prod.MarcaId != null)
            //    {
            //        var marca = (from a in marcas
            //                      where a.ID == prod.MarcaId
            //                      select a.Nombre).SingleOrDefault();

            //        prod.Nombre = prod.Nombre + " (" + marca + ")";
            //    }                    

            //    if (prod.CategoriaId != null)
            //    {
            //        var catego = (from a in categorias
            //                     where a.ID == prod.CategoriaId
            //                     select a.Nombre).SingleOrDefault();

            //        prod.Nombre = prod.Nombre + " (" + catego + ")";
            //    }
                    
            //}

            return productos;
        }

        [HttpGet]
        [Route("ventamayorista/api/productos/productosxlista")]
        public IEnumerable<ProductoDTO> ProductosXLista(int clienteId)
        {
            var productos = productoBL.GetAllProductosSegunListaAsociada(clienteId);
            //var productosBlister = productoBL.GetAllProductosBlister();

            log.Info("Accediendo a los Productos X Lista segun cliente ID: " + clienteId);

            List<ProductoDTO> productosConjunto = new List<ProductoDTO>();

            foreach (var prod in productos)
            {
                if (prod.EsMix && prod.EsBlister)
                {
                    prod.NombreAuxiliar = prod.NombreAuxiliar + " - MIX/BLISTER -";
                }
                else if (prod.EsMix)
                {
                    prod.NombreAuxiliar = prod.NombreAuxiliar + " - MIX -";
                }          

                productosConjunto.Add(prod);
            }

            //foreach (var prodBlister in productosBlister)
            //{

            //    if(prodBlister.EsBlister == true && prodBlister.EsBlister == true)
            //    {
                    
            //    }

            //    if (prodBlister.Marca != null)
            //        prodBlister.Nombre = prodBlister.Nombre + " (" + prodBlister.Marca.Nombre + ") - BLISTER -";

            //    if (prodBlister.Categoria != null)
            //        prodBlister.Nombre = prodBlister.Nombre + " (" + prodBlister.Categoria.Nombre + ") - BLISTER -";

            //    productosConjunto.Add(prodBlister);
            //}


            //var productos = productoBL.GetAllProductosSegunListaAsociada(clienteId);
            //var productosBlister = productoBL.GetAllProductosBlister();

            //List<Producto> productosConjunto = new List<Producto>();

            //foreach (var prod in productos)
            //{
            //    if(prod.EsMix)
            //    {
            //        if (prod.Marca != null)
            //            prod.Nombre = prod.Nombre + " (" + prod.Marca.Nombre + ") - MIX -";

            //        if (prod.Categoria != null)
            //            prod.Nombre = prod.Nombre + " (" + prod.Categoria.Nombre + ") - MIX -";
            //    }
            //    else
            //    {
            //        if (prod.Marca != null)
            //            prod.Nombre = prod.Nombre + " (" + prod.Marca.Nombre + ")";

            //        if (prod.Categoria != null)
            //            prod.Nombre = prod.Nombre + " (" + prod.Categoria.Nombre + ")";
            //    }


            //    productosConjunto.Add(prod);
            //}

            //foreach (var prodBlister in productosBlister)
            //{

            //    if (prodBlister.Marca != null)
            //        prodBlister.Nombre = prodBlister.Nombre + " (" + prodBlister.Marca.Nombre + ") - BLISTER -";

            //    if (prodBlister.Categoria != null)
            //        prodBlister.Nombre = prodBlister.Nombre + " (" + prodBlister.Categoria.Nombre + ") - BLISTER -";

            //    productosConjunto.Add(prodBlister);
            //}



            return productosConjunto;
        }

        [HttpGet]
        [Route("admin/api/productos/productos")]
        [Route("compra/api/productos/productos")]
        public IEnumerable<ProductoDTO> Productos()
        {

            var productos = productoBL.GetAllProducto();
            //var productosBlister = productoBL.GetAllProductosBlister();            

            List<Producto> productosConjunto = new List<Producto>();

            foreach (var prod in productos)
            {
                if(!prod.EsMix) { 
                    if (prod.Marca != null)
                        prod.Nombre = prod.Nombre + " (" + prod.Marca.Nombre + ")";

                    if (prod.Categoria != null)
                        prod.Nombre = prod.Nombre + " (" + prod.Categoria.Nombre + ")";

                    productosConjunto.Add(prod);
                }
            }
                       
            return productosConjunto.Select(Mapper.Map<Producto, ProductoDTO>);
        }

        [HttpGet]
        [Route("compra/editarcompra/api/productos/productos")]
        public IEnumerable<ProductoDTO> EditarProductos()
        {

            var productos = productoBL.GetAllProducto();

            List<Producto> productosConjunto = new List<Producto>();

            foreach (var prod in productos)
            {
                if (prod.Marca != null)
                    prod.Nombre = prod.Nombre + " (" + prod.Marca.Nombre + ")";

                if (prod.Categoria != null)
                    prod.Nombre = prod.Nombre + " (" + prod.Categoria.Nombre + ")";

                productosConjunto.Add(prod);
            }

            return productosConjunto.Select(Mapper.Map<Producto, ProductoDTO>);
        }

        [HttpGet]
        [Route("api/productos/productossegunflagmix")]
        public IEnumerable<ProductoDTO> ProductosSegunFlagMix()
        {

            var productos = productoBL.GetAllProductosSegunFlagMix();
           

            return productos.Select(Mapper.Map<Producto, ProductoDTO>);
        }

        [HttpGet]
        [Route("ventamayorista/editarventamayorista/api/productos/productosxlista")]
        public IEnumerable<ProductoDTO> EditarProductosXLista(int clienteId)
        {

            var productos = productoBL.GetAllProductosSegunListaAsociada(clienteId);
            //var productosBlister = productoBL.GetAllProductosBlister();

            List<ProductoDTO> productosConjunto = new List<ProductoDTO>();

            foreach (var prod in productos)
            {
                if (prod.Marca != null)
                {
                    if (prod.EsMix)
                        prod.Nombre = prod.Nombre + " (" + prod.Marca.Nombre + ") - MIX - ";
                    else
                        prod.Nombre = prod.Nombre + " (" + prod.Marca.Nombre + ")";
                }
                    

                if (prod.Categoria != null)
                {
                    if (prod.EsMix)
                        prod.Nombre = prod.Nombre + " (" + prod.Categoria.Nombre + ") - MIX - ";
                    else
                        prod.Nombre = prod.Nombre + " (" + prod.Categoria.Nombre + ")";
                }
                   
                productosConjunto.Add(prod);
            }

            //foreach (var prodBlister in productosBlister)
            //{

            //    if (prodBlister.Marca != null)
            //        prodBlister.Nombre = prodBlister.Nombre + " (" + prodBlister.Marca.Nombre + ") - BLISTER -";

            //    if (prodBlister.Categoria != null)
            //        prodBlister.Nombre = prodBlister.Nombre + " (" + prodBlister.Categoria.Nombre + ") - BLISTER -";

            //    productosConjunto.Add(prodBlister);
            //}



            return productosConjunto;
        }

        [HttpGet]
        [Route("api/productos/productosxlistareporte")]
        public IEnumerable<ProductoDTO> ProductosXListaReporte()
        {

            var productos = productoBL.GetAllProducto();
            //var productosBlister = productoBL.GetAllProductosBlister();

            List<Producto> productosConjunto = new List<Producto>();

            foreach (var prod in productos)
            {
                if (prod.EsMix)
                {
                    if (prod.Marca != null)
                        prod.Nombre = prod.Nombre + " (" + prod.Marca.Nombre + ") - MIX -";

                    if (prod.Categoria != null)
                        prod.Nombre = prod.Nombre + " (" + prod.Categoria.Nombre + ") - MIX -";
                }
                else
                {
                    if (prod.Marca != null)
                        prod.Nombre = prod.Nombre + " (" + prod.Marca.Nombre + ")";

                    if (prod.Categoria != null)
                        prod.Nombre = prod.Nombre + " (" + prod.Categoria.Nombre + ")";
                }


                productosConjunto.Add(prod);
            }

            //foreach (var prodBlister in productosBlister)
            //{

            //    if (prodBlister.Marca != null)
            //        prodBlister.Nombre = prodBlister.Nombre + " (" + prodBlister.Marca.Nombre + ") - BLISTER -";

            //    if (prodBlister.Categoria != null)
            //        prodBlister.Nombre = prodBlister.Nombre + " (" + prodBlister.Categoria.Nombre + ") - BLISTER -";

            //    productosConjunto.Add(prodBlister);
            //}



            return productosConjunto.Select(Mapper.Map<Producto, ProductoDTO>);
        }


        //GET /api/producto/1
        public IHttpActionResult GetProducto(int id)
        {


            var producto = productoBL.GetProductoById(id);

            if (producto == null)
            {
                log.Error("Producto no encontrado con ID: " + id);
                return NotFound();
            }

            return Ok(Mapper.Map<Producto, ProductoDTO>(producto));
        }

        //POST /api/productos
        [HttpPost]
        [Route("api/productos/createProducto")]
        public IHttpActionResult CreateProducto(ProductoDTO productoDTO)
        {
            if (!ModelState.IsValid)
            {
                log.Error("Formulario con datos incorrectos o inexistentes.");
                return BadRequest();
            }

            var producto = Mapper.Map<ProductoDTO, Producto>(productoDTO);

            productoBL.AddProducto(producto);
            
            productoDTO.ID = producto.ID;

            log.Info("Producto creado satisfactoriamente. ID: " + producto.ID);

            return Created(new Uri(Request.RequestUri + "/" + producto.ID), productoDTO);
        }

        //PUT /api/productos/1
        [HttpPut]
        public IHttpActionResult UpdateProductos(int id, ProductoDTO productoDTO)
        {
            if (!ModelState.IsValid)
            {
                log.Error("Formulario con datos incorrectos o inexistentes.");
                return BadRequest();
            }

            var productoInDB = productoBL.GetProductoById(id);

            if (productoInDB == null)
            {
                log.Error("No se encontro producto en la base de datos con ID: " + id);
                return NotFound();
            }

            Mapper.Map(productoDTO, productoInDB);

            productoBL.UpdateProducto(productoInDB);

            log.Info("Producto actualizado satisfactoriamente. ID: " + id);

            return Ok();
        }

        //DELETE /api/productos/1
        [HttpDelete]
        public IHttpActionResult DeleteProducto(int id)
        {

            var productoInDb = productoBL.GetProductoById(id);

            if (productoInDb != null)
            {
                if (productoInDb.EsMix)
                {

                    var listaProductosDelMix = productoBL.GetListaProductosMixById(productoInDb.ID);

                    if (listaProductosDelMix != null)
                    {
                        foreach (var productoMix in listaProductosDelMix)
                        {
                            productoBL.RemoveProductoMix(productoMix);
                        }
                    }

                    //Una vez borrados los productos relacionados del mix, borramos el producto principal
                    productoBL.RemoveProducto(productoInDb);

                }

                else
                    productoBL.RemoveProducto(productoInDb);

                log.Info("Producto borrado exitosamente. ID: " + id);
            }
            else
            {
                log.Error("Producto no encontrado con ID:" + id);
                return BadRequest();
            }
                

            return Ok();

        }

    }
}
