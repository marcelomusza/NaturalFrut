using AutoMapper;
using NaturalFrut.App_BLL;
using NaturalFrut.App_BLL.Interfaces;
using NaturalFrut.DTOs;
using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NaturalFrut.Controllers.Api
{
    public class VentasMayoristaController : ApiController
    {

        private readonly VentaMayoristaLogic ventaMayoristaBL;
        private readonly StockLogic stockBL;
        private readonly ClienteLogic clienteBL;

        public VentasMayoristaController(IRepository<VentaMayorista> VentaMayoristaRepo,
            IRepository<Stock> StockRepo,
            IRepository<Cliente> ClienteRepo)
        {
            ventaMayoristaBL = new VentaMayoristaLogic(VentaMayoristaRepo);
            stockBL = new StockLogic(StockRepo);
            clienteBL = new ClienteLogic(ClienteRepo);
        }
        
        //GET /api/ventasMayorista
        public IEnumerable<VentaMayoristaDTO> GetVentasMayorista()
        {
            var ventasMayorista = ventaMayoristaBL.GetAllVentaMayorista();

            return ventasMayorista.Select(Mapper.Map<VentaMayorista, VentaMayoristaDTO>);
        }




        //POST /api/ventasMayorista
        [HttpPost]
        public IHttpActionResult CreateVentasMayorista(VentaMayoristaDTO ventaMayoristaDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var cliente = clienteBL.GetClienteById(ventaMayoristaDTO.ClienteID);

            if (cliente == null)
                return BadRequest();

            var ventaMayorista = Mapper.Map<VentaMayoristaDTO, VentaMayorista>(ventaMayoristaDTO);

            ventaMayoristaBL.AddVentaMayorista(ventaMayorista);

            //if (cliente.Saldo > 0)
            //    cliente.Saldo = cliente.Saldo + ventaMayoristaDTO.EntregaEfectivo;

            if (ventaMayorista.NoConcretado)
            {
                //Logica para Ventas No Concretadas -- Devolución de Stock
            }
            else
            {
                //Una vez cargada la venta, actualizamos Stock
                foreach (var item in ventaMayorista.ProductosXVenta)
                {
                    Stock stock = stockBL.ValidarStockProducto(item.ProductoID, item.TipoDeUnidadID);

                    stock.Cantidad = stock.Cantidad - item.Cantidad;

                    stockBL.UpdateStock(stock);
    

                }
            }
                        

            return Ok();
        }


    }
}