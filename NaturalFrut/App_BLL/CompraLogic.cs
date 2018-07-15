using NaturalFrut.App_BLL.Interfaces;
using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace NaturalFrut.App_BLL
{
    public class CompraLogic
    {

        private readonly IRepository<Compra> compraRP;
        private readonly IRepository<Proveedor> proveedorRP;
        private readonly IRepository<Clasificacion> clasificacionRP;
     

        public CompraLogic(IRepository<Compra> CompraRepository,
            IRepository<Proveedor> ProveedorRepository,
            IRepository<Clasificacion> ClasificacionRepository)
        {
            compraRP = CompraRepository;
            proveedorRP = ProveedorRepository;
            clasificacionRP = ClasificacionRepository;

        }

        /*public CompraLogic(IRepository<C> ClienteRepository)
        {
            clienteRP = ClienteRepository;
        }*/

        public CompraLogic(IRepository<Compra> compraRepo)
        {
            this.compraRP = compraRepo;
        }

        public List<Compra> GetAllCompra()
        {
            return compraRP.GetAll()
                .Include(c => c.Proveedor)
                .Include(v => v.Clasificacion)
                .ToList();
        }

        public Compra GetCompraById(int id)
        {
            return compraRP
                .GetAll()
                .Include(c => c.Proveedor)
                .Include(v => v.Clasificacion)
                .Where(c => c.ID == id).SingleOrDefault();
        }

        

        public void RemoveCompra(Compra compra)
        {
            compraRP.Delete(compra);
            compraRP.Save();
        }

        public void AddCompra(Compra compra)
        {
            compraRP.Add(compra);
            compraRP.Save();
        }

        public void UpdateCompra(Compra compra)
        {
            compraRP.Update(compra);
            compraRP.Save();
        }

        public List<Proveedor> GetProveedorList()
        {
            return proveedorRP.GetAll().ToList();
        }

        public List<Clasificacion> GetClasificacionList()
        {
            return clasificacionRP.GetAll().ToList();
        }

    }
}