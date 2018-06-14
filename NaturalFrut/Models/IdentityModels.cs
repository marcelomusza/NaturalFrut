using System.Data.Entity;
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
        public DbSet<Venta> Ventas { get; set; }

        // CONTEXT MARCELO        
        public ApplicationDbContext()
            : base("DefaultConnectionMarcelo", throwIfV1Schema: false)
        {
        }


        // CONTEXT YESICA

        //public ApplicationDbContext()
        //    : base("DefaultConnectionYesica", throwIfV1Schema: false)
        //{
        //}


        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
            
        //    modelBuilder.Entity<Proveedor>()
        //        .ToTable("Proveedores");
        //}

    }

   
}