using NaturalFrut.App_BLL.Interfaces;
using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NaturalFrut.App_BLL
{
    public class ProveedorLogic
    {

        private readonly IRepository<Proveedor> proveedorRP;
        private readonly IRepository<CondicionIVA> condicionIVARP;

        public ProveedorLogic(IRepository<Proveedor> ProveedorRepository,
            IRepository<CondicionIVA> CondicionIVARepository)
        {
            proveedorRP = ProveedorRepository;
            condicionIVARP = CondicionIVARepository;
        }

        public ProveedorLogic(IRepository<Proveedor> ClienteRepository)
        {
            proveedorRP = ClienteRepository;
        }

        public List<Proveedor> GetAllProveedores()
        {
            return proveedorRP.GetAll()
                .Include(c => c.CondicionIVA)
                .ToList();
        }

        public Proveedor GetProveedorById(int id)
        {
            return proveedorRP.GetAll()
                .Include(c => c.CondicionIVA)
                .Where(c => c.ID == id).SingleOrDefault();
        }

        public void RemoveProveedor(Proveedor proveedor)
        {
            proveedorRP.Delete(proveedor);
            proveedorRP.Save();
        }

        public void AddProveedor(Proveedor proveedor)
        {
            proveedorRP.Add(proveedor);
            proveedorRP.Save();
        }

        public void UpdateProveedor(Proveedor proveedor)
        {
            proveedorRP.Update(proveedor);
            proveedorRP.Save();
        }

        
        public List<CondicionIVA> GetCondicionIvaList()
        {
            return condicionIVARP.GetAll().ToList();
        }

    }
}