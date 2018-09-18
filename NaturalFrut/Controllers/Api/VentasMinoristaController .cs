using AutoMapper;
using NaturalFrut.App_BLL;
using NaturalFrut.App_BLL.Interfaces;
using NaturalFrut.DTOs;
using NaturalFrut.Helpers;
using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace NaturalFrut.Controllers.Api
{
    public class VentasMinoristaController : ApiController
    {

        private readonly VentaMinoristaLogic ventaMinoristaBL;

        public VentasMinoristaController(IRepository<VentaMinorista> VentaMinoristaRepo)
        {
            ventaMinoristaBL = new VentaMinoristaLogic(VentaMinoristaRepo);
            
        }
        
        //GET /api/ventasMinorista
        public IEnumerable<VentaMinoristaDTO> GetVentasMinorista()
        {
            var ventasMinorista = ventaMinoristaBL.GetAllVentaMinorista();

            return ventasMinorista.Select(Mapper.Map<VentaMinorista, VentaMinoristaDTO>);
        }


        //POST /api/ventasMinorista
        [HttpPost]
        public IHttpActionResult CreateVentasMinorista(VentaMinoristaDTO ventaMinoristaDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var ventaMinorista = Mapper.Map<VentaMinoristaDTO, VentaMinorista>(ventaMinoristaDTO);

            ventaMinoristaBL.AddVentaMinorista(ventaMinorista);

            ventaMinoristaDTO.ID = ventaMinorista.ID;

            return Created(new Uri(Request.RequestUri + "/" + ventaMinorista.ID), ventaMinoristaDTO);
        }


        //PUT /api/ventasMinorista
        [HttpPut]
        public IHttpActionResult UpdateventasMinorista(VentaUpdateDTO ventaUpdateDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();


            return Ok();
        }

        //[HttpPost]
        //[Route("api/ventasminorista/generareporteventasminoristas")]
        //public IHttpActionResult GenerarReporteVentasMinoristas(List<VentaMinorista> vtasMinorista)
        //{



        //    return Created();
        //}

    }
}