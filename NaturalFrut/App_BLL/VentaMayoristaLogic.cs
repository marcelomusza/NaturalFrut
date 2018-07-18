using NaturalFrut.App_BLL.Interfaces;
using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace NaturalFrut.App_BLL
{
    public class VentaMayoristaLogic
    {

        private readonly IRepository<VentaMayorista> ventaMayoristaRP;
        private readonly IRepository<Cliente> clienteRP;
        private readonly IRepository<Vendedor> vendedorRP;
        private readonly IRepository<Lista> listaRP;
        private readonly IRepository<ListaPrecio> listaPreciosRP;

        public VentaMayoristaLogic(IRepository<VentaMayorista> VentaMayoristaRepository,
            IRepository<Cliente> ClienteRepository,
            IRepository<Vendedor> VendedorRepository,
            IRepository<Lista> ListaRepository,
            IRepository<ListaPrecio> ListaPrecioRepository)
        {
            ventaMayoristaRP = VentaMayoristaRepository;
            clienteRP = ClienteRepository;
            vendedorRP = VendedorRepository;
            listaRP = ListaRepository;
            listaPreciosRP = ListaPrecioRepository;
        }

        public VentaMayoristaLogic(IRepository<VentaMayorista> VentaMayoristaRepository)
        {
            ventaMayoristaRP = VentaMayoristaRepository;
        }

        public VentaMayoristaLogic(IRepository<Cliente> ClienteRepository)
        {
            clienteRP = ClienteRepository;
        }

        public VentaMayorista GetNumeroDeVenta()
        {
            var ultimaVenta = ventaMayoristaRP.GetAll().OrderByDescending(p => p.ID).FirstOrDefault();

            return ultimaVenta;
        }

        public List<VentaMayorista> GetAllVentaMayorista()
        {
            return ventaMayoristaRP.GetAll()
                .Include(c => c.Cliente)
                .Include(v => v.Vendedor)
                .ToList();
        }

        public VentaMayorista GetVentaMayoristaById(int id)
        {
            return ventaMayoristaRP
                .GetAll()
                .Include(c => c.Cliente)
                .Include(v => v.Vendedor)
                .Where(c => c.ID == id).SingleOrDefault();
        }


        public void RemoveVentaMayorista(VentaMayorista ventaMayorista)
        {
            ventaMayoristaRP.Delete(ventaMayorista);
            ventaMayoristaRP.Save();
        }

        public void AddVentaMayorista(VentaMayorista ventaMayorista)
        {
            ventaMayoristaRP.Add(ventaMayorista);
            ventaMayoristaRP.Save();
        }


        public void UpdateVentaMayorista(VentaMayorista ventaMayorista)
        {
            ventaMayoristaRP.Update(ventaMayorista);
            ventaMayoristaRP.Save();
        }

        public List<Cliente> GetClienteList()
        {
            return clienteRP.GetAll().ToList();
        }

        public List<Vendedor> GetVendedorList()
        {
            return vendedorRP.GetAll().ToList();
        }

        public List<Lista> GetListaList()
        {
            return listaRP.GetAll().ToList();
        }

        public ListaPrecio CalcularImporteSegunCliente(int clienteID, int productoID, int cantidad)
        {

            var cliente = clienteRP.GetByID(clienteID);

            int listaAsociada = cliente.ListaId;

            var productoSegunLista = listaPreciosRP.GetAll()
                .Where(p => p.ProductoID == productoID)
                .Where(p => p.ListaID == listaAsociada)
                .SingleOrDefault();


            return productoSegunLista;

        }
    }
}