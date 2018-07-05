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

        public VentaMayoristaLogic(IRepository<VentaMayorista> VentaMayoristaRepository,
            IRepository<Cliente> ClienteRepository,
            IRepository<Vendedor> VendedorRepository,
            IRepository<Lista> ListaRepository)
        {
            ventaMayoristaRP = VentaMayoristaRepository;
            clienteRP = ClienteRepository;
            vendedorRP = VendedorRepository;
            listaRP = ListaRepository;
        }

        public VentaMayoristaLogic(IRepository<Cliente> ClienteRepository)
        {
            clienteRP = ClienteRepository;
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
    }
}