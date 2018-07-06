using NaturalFrut.App_BLL.Interfaces;
using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace NaturalFrut.App_BLL
{
    public class VentaMinoristaLogic
    {

        private readonly IRepository<VentaMinorista> ventaMinoristaRP;
        
        public VentaMinoristaLogic(IRepository<VentaMinorista> VentaMinoristaRepository)
        {
            ventaMinoristaRP = VentaMinoristaRepository;
           
        }


        public List<VentaMinorista> GetAllVentaMinorista()
        {
            return ventaMinoristaRP.GetAll().ToList();
        }

        public VentaMinorista GetVentaMinoristaById(int id)
        {
            return ventaMinoristaRP
                .GetAll()                
                .Where(c => c.ID == id).SingleOrDefault();
        }

        public void RemoveVentaMinorista(VentaMinorista ventaMinorista)
        {
            ventaMinoristaRP.Delete(ventaMinorista);
            ventaMinoristaRP.Save();
        }

        public void AddVentaMinorista(VentaMinorista ventaMinorista)
        {
            ventaMinoristaRP.Add(ventaMinorista);
            ventaMinoristaRP.Save();
        }

        public void UpdateVentaMinorista(VentaMinorista ventaMinorista)
        {
            ventaMinoristaRP.Update(ventaMinorista);
            ventaMinoristaRP.Save();
        }

    }
}