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
        }

       
    }
}