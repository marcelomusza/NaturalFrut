using NaturalFrut.App_BLL.Interfaces;
using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NaturalFrut.App_BLL
{
    public class VendedorLogic
    {

        private readonly IRepository<Vendedor> vendedorRP;      

        public VendedorLogic(IRepository<Vendedor> VendedorRepository)
        {
            vendedorRP = VendedorRepository;
        }

        public List<Vendedor> GetAllVendedores()
        {
            return vendedorRP.GetAll().ToList();
        }

        public Vendedor GetVendedorById(int id)
        {
            return vendedorRP.GetByID(id);
        }

        public void RemoveVendedor(Vendedor vendedor)
        {
            vendedorRP.Delete(vendedor);
            vendedorRP.Save();
        }

        public void AddVendedor(Vendedor vendedor)
        {
            vendedorRP.Add(vendedor);
            vendedorRP.Save();
        }

        public void UpdateVendedor(Vendedor vendedor)
        {
            vendedorRP.Update(vendedor);
            vendedorRP.Save();
        }

    }
}