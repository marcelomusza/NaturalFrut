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
            return ventaMinoristaRP
                .GetAll()
                .ToList();
        }

        public VentaMinorista GetVentaMinoristaById(int id)
        {
            return ventaMinoristaRP
                .GetAll()                
                .Where(c => c.ID == id)
                .SingleOrDefault();
        }

        public VentaMinorista GetVentaMinoristaByNumeroVenta(int numVenta)
        {
            return ventaMinoristaRP
                .GetAll()
                .Where(c => c.NumeroVenta == numVenta)
                .SingleOrDefault();
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

        public VentaMinorista GetNumeroDeVenta()
        {
            var ultimaVenta = ventaMinoristaRP
                .GetAll()
                .OrderByDescending(p => p.ID)
                .FirstOrDefault();

            return ultimaVenta;
        }

        public List<VentaMinorista> GetAllVentaMinoristaSegunFechas(DateTime fechaDesde, DateTime fechaHasta)
        {

            var reporteVentasSegunFecha = ventaMinoristaRP
                .GetAll()
                .Where(f => f.Fecha >= fechaDesde && f.Fecha <= fechaHasta)
                .ToList();


            return reporteVentasSegunFecha;
        }


        #region Seccion Reportes
        public List<VentaMinorista> GetAllVentaMinoristaSegunFechas(DateTime fechaDesde, DateTime fechaHasta, string local)
        {

            var reporteVentaSegunFecha = ventaMinoristaRP
                .GetAll()
                .Where(f => f.Fecha >= fechaDesde && f.Fecha <= fechaHasta && f.Local == local)
                .ToList();


            return reporteVentaSegunFecha;
        }

        #endregion

    }
}