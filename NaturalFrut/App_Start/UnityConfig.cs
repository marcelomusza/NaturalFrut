using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NaturalFrut.App_BLL.Interfaces;
using NaturalFrut.App_DAL;
using NaturalFrut.Controllers;
using NaturalFrut.Models;
using System;
using System.Data.Entity;
using Unity;
using Unity.Injection;
using Unity.Lifetime;


namespace NaturalFrut
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();
            container.RegisterType <IRepository<Cliente>, BaseRepository<Cliente>>(new TransientLifetimeManager());
            container.RegisterType<IRepository<CondicionIVA>, BaseRepository<CondicionIVA>>(new TransientLifetimeManager());
            container.RegisterType<IRepository<TipoCliente>, BaseRepository<TipoCliente>>(new TransientLifetimeManager());
            container.RegisterType<IRepository<Proveedor>, BaseRepository<Proveedor>>(new TransientLifetimeManager());
            container.RegisterType<IRepository<Producto>, BaseRepository<Producto>>(new TransientLifetimeManager());
            container.RegisterType<IRepository<Categoria>, BaseRepository<Categoria>>(new TransientLifetimeManager());
            container.RegisterType<IRepository<Vendedor>, BaseRepository<Vendedor>>(new TransientLifetimeManager());
            container.RegisterType<IRepository<Marca>, BaseRepository<Marca>>(new TransientLifetimeManager());
            container.RegisterType<IRepository<TipoDeUnidad>, BaseRepository<TipoDeUnidad>>(new TransientLifetimeManager());
            container.RegisterType<IRepository<Lista>, BaseRepository<Lista>>(new TransientLifetimeManager());
            container.RegisterType<IRepository<ListaPrecio>, BaseRepository<ListaPrecio>>(new TransientLifetimeManager());
            container.RegisterType<IRepository<VentaMayorista>, BaseRepository<VentaMayorista>>(new TransientLifetimeManager());
            container.RegisterType<IRepository<VentaMinorista>, BaseRepository<VentaMinorista>>(new TransientLifetimeManager());
            container.RegisterType<IRepository<Compra>, BaseRepository<Compra>>(new TransientLifetimeManager());
            container.RegisterType<IRepository<Clasificacion>, BaseRepository<Clasificacion>>(new TransientLifetimeManager());
            container.RegisterType<IRepository<Stock>, BaseRepository<Stock>>(new TransientLifetimeManager());
            container.RegisterType<IRepository<ProductoXVenta>, BaseRepository<ProductoXVenta>>(new TransientLifetimeManager());



            container.RegisterType<DbContext, ApplicationDbContext>(new HierarchicalLifetimeManager());

            container.RegisterType<UserManager<ApplicationUser>>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(new HierarchicalLifetimeManager());

            container.RegisterType<RoleManager<IdentityRole>>(new HierarchicalLifetimeManager());
            container.RegisterType<IRoleStore<IdentityRole, string>, RoleStore<IdentityRole>>(new HierarchicalLifetimeManager());


            container.RegisterType<AccountController>(new InjectionConstructor());

        }
    }
}