﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace NaturalFrut.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    //public class ApplicationRole : IdentityRole
    //{
    //    public ApplicationRole() : base() { }
    //    public ApplicationRole(string name)
    //    : base(name)
    //    {
    //    }

    //}

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        //Asignación de DbSets para CodeFirst migrations 
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<CondicionIVA> CondicionIVA { get; set; }
        public DbSet<TipoCliente> TipoCliente { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Vendedor> Vendedor { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<VentaMayorista> Ventas { get; set; }
        public DbSet<VentaMinorista> VentasMinorista { get; set; }
        public DbSet<TipoDeUnidad> TipoDeUnidad { get; set; }
        public DbSet<ProductoXVenta> ProductoXVenta { get; set; }
        public DbSet<Lista> Listas { get; set; }
        public DbSet<ListaPrecio> ListaPrecios { get; set; }
        public DbSet<ListaPrecioBlister> ListaPreciosBlister { get; set; }
        public DbSet<ProductoMix> ProductoMix { get; set; }

        ////CONTEXT MARCELO
        //public ApplicationDbContext()
        //    : base("DefaultConnectionMarcelo", throwIfV1Schema: false)
        //{
        //}


        // CONTEXT YESICA

        public ApplicationDbContext()
            : base("DefaultConnectionYesica", throwIfV1Schema: false)
        {
        }


        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{

        //}
    }

    

   
}