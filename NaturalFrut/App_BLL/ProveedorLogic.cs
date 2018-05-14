using NaturalFrut.App_BLL.Interfaces;
using NaturalFrut.Models;
using System;
using System.Collections.Generic;
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

        public List<Proveedor> GetAllProveedores()
        {
            return proveedorRP.GetAll().ToList();
        }

        public Proveedor GetProveedorById(int id)
        {
            return proveedorRP.GetByID(id);
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