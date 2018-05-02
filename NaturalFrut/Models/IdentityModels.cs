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

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        /*protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

             modelBuilder.Entity<CondicionIVA>()
                 .HasMany(e => e.Clientes)
                 .WithRequired(e => e.CondicionIVA)
                 .HasForeignKey(e => e.ID)
                 .WillCascadeOnDelete(false);

            modelBuilder.Entity<TipoCliente>()
                 .HasMany(e => e.Clientes)
                 .WithRequired(e => e.TipoCliente)
                 .HasForeignKey(e => e.ID)
                 .WillCascadeOnDelete(false);
        }*/
    }

   
}