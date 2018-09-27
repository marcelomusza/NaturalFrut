using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using NaturalFrut.DTOs;
using NaturalFrut.Models;

namespace NaturalFrut.App_Start
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            Mapper.CreateMap<Cliente, ClienteDTO>();
            Mapper.CreateMap<ClienteDTO, Cliente>();

            Mapper.CreateMap<CondicionIVA, CondicionIVADTO>();
            Mapper.CreateMap<CondicionIVADTO, CondicionIVA>();

            Mapper.CreateMap<TipoCliente, TipoClienteDTO>();
            Mapper.CreateMap<TipoClienteDTO, TipoCliente>();

            Mapper.CreateMap<Proveedor, ProveedorDTO>();
            Mapper.CreateMap<ProveedorDTO, Proveedor>();

            Mapper.CreateMap<Producto, ProductoDTO>();
            Mapper.CreateMap<ProductoDTO, Producto>();

            Mapper.CreateMap<Categoria, CategoriaDTO>();
            Mapper.CreateMap<CategoriaDTO, Categoria>();

            Mapper.CreateMap<Marca, MarcaDTO>();
            Mapper.CreateMap<MarcaDTO, Marca>();

            Mapper.CreateMap<Vendedor, VendedorDTO>();
            Mapper.CreateMap<VendedorDTO, Vendedor>();

            Mapper.CreateMap<ListaPrecio, ListaPrecioDTO>();
            Mapper.CreateMap<ListaPrecioDTO, ListaPrecio>();

            Mapper.CreateMap<ListaPrecioBlister, ListaPrecioBlisterDTO>();
            Mapper.CreateMap<ListaPrecioBlisterDTO, ListaPrecioBlister>();

            Mapper.CreateMap<VentaMayorista, VentaMayoristaDTO>();
            Mapper.CreateMap<VentaMayoristaDTO, VentaMayorista>();

            Mapper.CreateMap<ProductoXVenta, ProductoXVentaDTO>();
            Mapper.CreateMap<ProductoXVentaDTO, ProductoXVenta>();

            Mapper.CreateMap<VentaUpdate, VentaUpdateDTO>();
            Mapper.CreateMap<VentaUpdateDTO, VentaUpdate>();

            Mapper.CreateMap<ProductoMix, ProductoMixDTO>();
            Mapper.CreateMap<ProductoMixDTO, ProductoMix>();

            Mapper.CreateMap<Compra, CompraDTO>();
            Mapper.CreateMap<CompraDTO, Compra>();

            Mapper.CreateMap<ProductoXCompra, ProductoXCompraDTO>();
            Mapper.CreateMap<ProductoXCompraDTO, ProductoXCompra>();

            Mapper.CreateMap<Clasificacion, ClasificacionDTO>();
            Mapper.CreateMap<ClasificacionDTO, Clasificacion>();

            Mapper.CreateMap<VentaMinorista, VentaMinoristaDTO>();
            Mapper.CreateMap<VentaMinoristaDTO, VentaMinorista>();



        }

       
    }
}