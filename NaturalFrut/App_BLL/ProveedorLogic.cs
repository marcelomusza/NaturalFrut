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
        

        public ProveedorLogic(IRepository<Proveedor> ProveedorRepository)
            
        {
            proveedorRP = ProveedorRepository;
            
        }

        public List<Proveedor> GetAllProveedores()
        {
            return proveedorRP.GetAll().ToList();
        }

        public Proveedor GetProveedorById(int id)
        {
            return proveedorRP.GetAll().Where(c => c.ID == id).SingleOrDefault();


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

    }
}