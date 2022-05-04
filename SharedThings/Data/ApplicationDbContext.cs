using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mvc1VaccinDemo.Data;

namespace SharedThings.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Vaccin> Vacciner { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Person> Personer { get; set; }
        public DbSet<Vaccinering> Vaccineringar { get; set; }
        public DbSet<VaccineringsFas> VaccineringsFaser { get; set; }
        public DbSet<Myndighet> Myndigheter { get; set; }
    }
}
