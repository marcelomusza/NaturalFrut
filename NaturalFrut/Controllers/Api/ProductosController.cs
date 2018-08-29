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

    public class ProductosController : ApiController
    {
        
        private readonly ProductoLogic productoBL;
        private readonly ClienteLogic clienteBL;

        public ProductosController(IRepository<Producto> ProductoRepo,
            IRepository<Cliente> ClienteRepo,
            IRepository<ListaPrecio> ListaPrecioRepo)
        {            
            productoBL = new ProductoLogic(ProductoRepo, ClienteRepo, ListaPrecioRepo);
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

        [HttpGet]
        [Route("ventamayorista/api/productos/productosxlista")]
        public IEnumerable<ProductoDTO> ProductosXLista(int clienteId)
        {

            var productos = productoBL.GetAllProductosSegunListaAsociada(clienteId);
            var productosBlister = productoBL.GetAllProductosBlister();

            List<Producto> productosConjunto = new List<Producto>();

            foreach (var prod in productos)
            {
                if (prod.Marca != null)
                    prod.Nombre = prod.Nombre + " (" + prod.Marca.Nombre + ")";

                if (prod.Categoria != null)
                    prod.Nombre = prod.Nombre + " (" + prod.Categoria.Nombre + ")";

                productosConjunto.Add(prod);
            }

            foreach (var prodBlister in productosBlister)
            {

                if (prodBlister.Marca != null)
                    prodBlister.Nombre = prodBlister.Nombre + " (" + prodBlister.Marca.Nombre + ") - BLISTER -";

                if (prodBlister.Categoria != null)
                    prodBlister.Nombre = prodBlister.Nombre + " (" + prodBlister.Categoria.Nombre + ") - BLISTER -";

                productosConjunto.Add(prodBlister);
            }



            return productosConjunto.Select(Mapper.Map<Producto, ProductoDTO>);
        }

        [HttpGet]
        [Route("ventamayorista/editarventamayorista/api/productos/productosxlista")]
        public IEnumerable<ProductoDTO> EditarProductosXLista(int clienteId)
        {

            var productos = productoBL.GetAllProductosSegunListaAsociada(clienteId);
            var productosBlister = productoBL.GetAllProductosBlister();

            List<Producto> productosConjunto = new List<Producto>();

            foreach (var prod in productos)
            {
                if (prod.Marca != null)
                    prod.Nombre = prod.Nombre + " (" + prod.Marca.Nombre + ")";

                if (prod.Categoria != null)
                    prod.Nombre = prod.Nombre + " (" + prod.Categoria.Nombre + ")";

                productosConjunto.Add(prod);
            }

            foreach (var prodBlister in productosBlister)
            {

                if (prodBlister.Marca != null)
                    prodBlister.Nombre = prodBlister.Nombre + " (" + prodBlister.Marca.Nombre + ") - BLISTER -";

                if (prodBlister.Categoria != null)
                    prodBlister.Nombre = prodBlister.Nombre + " (" + prodBlister.Categoria.Nombre + ") - BLISTER -";

                productosConjunto.Add(prodBlister);
            }



            return productosConjunto.Select(Mapper.Map<Producto, ProductoDTO>);
        }

        //GET /api/producto/1
        public IHttpActionResult GetProducto(int id)
        {


            var producto = productoBL.GetProductoById(id);

            if (producto == null)
                return NotFound();

            return Ok(Mapper.Map<Producto, ProductoDTO>(producto));
        }

        //POST /api/productos
        [HttpPost]
        public IHttpActionResult CreateProducto(ProductoDTO productoDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var producto = Mapper.Map<ProductoDTO, Producto>(productoDTO);

            productoBL.AddProducto(producto);
            
            productoDTO.ID = producto.ID;

            return Created(new Uri(Request.RequestUri + "/" + producto.ID), productoDTO);
        }

        //PUT /api/productos/1
        [HttpPut]
        public IHttpActionResult UpdateProductos(int id, ProductoDTO productoDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var productoInDB = productoBL.GetProductoById(id);

            if (productoInDB == null)
                return NotFound();

            Mapper.Map(productoDTO, productoInDB);

            productoBL.UpdateProducto(productoInDB);

            return Ok();
        }

        //DELETE /api/productos/1
        [HttpDelete]
        public IHttpActionResult DeleteProducto(int id)
        {

            var productoInDB = productoBL.GetProductoById(id);

            if (productoInDB == null)
                return NotFound();

            productoBL.RemoveProducto(productoInDB);
            
            return Ok();

        }

    }
}
