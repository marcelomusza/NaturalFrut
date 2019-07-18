using NaturalFrut.App_BLL.Interfaces;
using NaturalFrut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;


namespace NaturalFrut.App_BLL
{
    public class VentaMayoristaLogic
    {

        private readonly IRepository<VentaMayorista> ventaMayoristaRP;
        private readonly IRepository<Cliente> clienteRP;
        private readonly IRepository<Vendedor> vendedorRP;
        private readonly IRepository<Lista> listaRP;
        private readonly IRepository<ListaPrecio> listaPreciosRP;
        private readonly IRepository<ListaPrecioBlister> listaPreciosBlisterRP;
        private readonly IRepository<Producto> productoRP;
        private readonly IRepository<ProductoXVenta> productoXVentaRP;
        private readonly IRepository<Categoria> categoriaRP;
        private readonly IRepository<Marca> marcaRP;

        public VentaMayoristaLogic(IRepository<VentaMayorista> VentaMayoristaRepository,
            IRepository<Cliente> ClienteRepository,
            IRepository<Vendedor> VendedorRepository,
            IRepository<Lista> ListaRepository,
            IRepository<ListaPrecio> ListaPrecioRepository,
            IRepository<ListaPrecioBlister> ListaPrecioBlisterRepository,
            IRepository<Producto> ProductoRepository,
            IRepository<ProductoXVenta> ProductoXVentaRepository,
            IRepository<Categoria> CategoriaRepository,
            IRepository<Marca> MarcaRepository)
        {
            ventaMayoristaRP = VentaMayoristaRepository;
            clienteRP = ClienteRepository;
            vendedorRP = VendedorRepository;
            listaRP = ListaRepository;
            listaPreciosRP = ListaPrecioRepository;
            listaPreciosBlisterRP = ListaPrecioBlisterRepository;
            productoRP = ProductoRepository;
            productoXVentaRP = ProductoXVentaRepository;
        }

        public VentaMayoristaLogic(IRepository<VentaMayorista> VentaMayoristaRepository,
            IRepository<ListaPrecioBlister> ListaPrecioBlisterRepository)
        {
            ventaMayoristaRP = VentaMayoristaRepository;
            listaPreciosBlisterRP = ListaPrecioBlisterRepository;
        }

        
        public VentaMayoristaLogic(IRepository<Cliente> ClienteRepository)
        {
            clienteRP = ClienteRepository;
        }

        public VentaMayoristaLogic(IRepository<ListaPrecioBlister> ListaPrecioRepository)
        {
            listaPreciosBlisterRP = ListaPrecioRepository;
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

        public List<VentaMayorista> GetAllVentaMayoristaPorCliente(int clienteID)
        {
            return ventaMayoristaRP.GetAll()
                .Include(c => c.Cliente)
                .Include(v => v.Vendedor)
                .Where(c => c.ClienteID == clienteID)
                .ToList();
        }

        public VentaMayorista GetVentaMayoristaById(int id)
        {
            VentaMayorista vtaMayorista = ventaMayoristaRP
                .GetAll()
                .Include(c => c.Cliente)
                .Include(v => v.Vendedor)
                .Include(p => p.ProductosXVenta)                
                .Include("ProductosXVenta.TipoDeUnidad")
                .Include("ProductosXVenta.Producto")
                .Include("ProductosXVenta.Producto.Marca")
                .Include("ProductosXVenta.Producto.Categoria")
                .Where(c => c.ID == id).SingleOrDefault();

            return vtaMayorista;
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

        public ListaPrecio CalcularImporteSegunCliente(int clienteID, int productoID, double cantidad)
        {

            var cliente = clienteRP.GetByID(clienteID);

            var listaAsociada = cliente.ListaId;

            var productoSegunLista = listaPreciosRP.GetAll()
                .Where(p => p.ProductoID == productoID)
                .Where(p => p.ListaID == listaAsociada)
                .SingleOrDefault();


            return productoSegunLista;

        }

        public Producto ValidateTipoDeProducto(int productoID)
        {

            Producto producto = productoRP.GetByID(productoID);

            if (producto != null)
            {
                return producto;
            }
                
            else
                throw new Exception("Error al Validar tipo de Producto");


        }

        public ListaPrecioBlister CalcularImporteBlisterSegunCliente(int productoID)
        {
           
            var productoSegunLista = listaPreciosBlisterRP.GetAll()
                .Where(p => p.ProductoID == productoID)
                .SingleOrDefault();

            return productoSegunLista;
        }

        #region SECCION REPORTES
        public List<VentaMayorista> GetAllVentaMayoristaSegunFechas(DateTime fechaDesde, DateTime fechaHasta)
        {

            var reporteVentasSegunFecha = ventaMayoristaRP
                .GetAll()
                .Include(c => c.Cliente)
                .Where(f => f.Fecha >= fechaDesde && f.Fecha <= fechaHasta)
                .ToList();


            return reporteVentasSegunFecha;
        }

        public List<VentaMayorista> GetVentasMayoristas()
        {

            var reporteVentasMayoristas = ventaMayoristaRP
                .GetAll()
                .Include(p => p.ProductosXVenta)
                .ToList();

            return reporteVentasMayoristas;
        }
        #endregion

    }
}