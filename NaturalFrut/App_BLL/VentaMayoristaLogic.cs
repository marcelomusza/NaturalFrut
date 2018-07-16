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
        //private IRepository<VentaMayorista> ventaMayoristaRepo;

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

        public VentaMayoristaLogic(IRepository<Cliente> ClienteRepository)
        {
            clienteRP = ClienteRepository;
        }

        public VentaMayoristaLogic(IRepository<VentaMayorista> ventaMayoristaRepo)
        {
            this.ventaMayoristaRP = ventaMayoristaRepo;
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

        

        public void RemoveCliente(VentaMayorista ventaMayorista)
        {
            ventaMayoristaRP.Delete(ventaMayorista);
            ventaMayoristaRP.Save();
        }

        public void AddCliente(VentaMayorista ventaMayorista)
        {
            ventaMayoristaRP.Add(ventaMayorista);
            ventaMayoristaRP.Save();
        }

        public void UpdateCliente(VentaMayorista ventaMayorista)
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