using Microsoft.EntityFrameworkCore;

namespace PharmacyLocator.Models
{
    public class PharmaDbContext : DbContext
    {
        public DbSet<Admin> admins { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Pharmacy> pharmacies { get; set; }
        public DbSet<Medicine> medicines { get; set; }
        public DbSet<Record> records { get; set; }
        public DbSet<Store> stores { get; set; }
        public DbSet <Location> locations { get; set; }

        public PharmaDbContext(DbContextOptions<PharmaDbContext> options) : base(options) { }

    }
}
