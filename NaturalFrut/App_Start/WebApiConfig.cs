using NaturalFrut.App_BLL.Interfaces;
using NaturalFrut.App_DAL;
using NaturalFrut.Models;
using NaturalFrut.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Unity;
using Unity.Lifetime;

namespace NaturalFrut
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer();
            container.RegisterType<IRepository<Cliente>, BaseRepository<Cliente>>(new HierarchicalLifetimeManager());
            container.RegisterType<IRepository<Proveedor>, BaseRepository<Proveedor>>(new HierarchicalLifetimeManager());
            container.RegisterType<IRepository<Producto>, BaseRepository<Producto>>(new HierarchicalLifetimeManager());
            container.RegisterType<IRepository<Vendedor>, BaseRepository<Vendedor>>(new HierarchicalLifetimeManager());
            container.RegisterType<IRepository<Lista>, BaseRepository<Lista>>(new HierarchicalLifetimeManager());
            container.RegisterType<IRepository<ListaPrecio>, BaseRepository<ListaPrecio>>(new HierarchicalLifetimeManager());
            container.RegisterType<IRepository<ListaPrecioBlister>, BaseRepository<ListaPrecioBlister>>(new HierarchicalLifetimeManager());
            container.RegisterType<IRepository<VentaMayorista>, BaseRepository<VentaMayorista>>(new HierarchicalLifetimeManager());
            container.RegisterType<IRepository<Stock>, BaseRepository<Stock>>(new HierarchicalLifetimeManager());
            container.RegisterType<IRepository<ProductoXVenta>, BaseRepository<ProductoXVenta>>(new HierarchicalLifetimeManager());
            container.RegisterType<IRepository<ProductoMix>, BaseRepository<ProductoMix>>(new HierarchicalLifetimeManager());
            container.RegisterType<IRepository<Clasificacion>, BaseRepository<Clasificacion>>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);

            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
