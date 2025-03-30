using Microsoft.EntityFrameworkCore;
using RiPOS.Database.Configurations;
using RiPOS.Domain.Entities;

namespace RiPOS.Database
{
    public class RiPosDbContext(DbContextOptions<RiPosDbContext> options) : DbContext(options)
    {
        public DbSet<Brand> Brands { get; set; }

        public DbSet<CashRegister> CashRegisters { get; set; }

        // public DbSet<Category> Categories { get; set; }

        // public DbSet<Color> Colors { get; set; }
        public DbSet<CountryState> CountryStates { get; set; }

        // public DbSet<Customer> Customers { get; set; }

        // public DbSet<Gender> Genders { get; set; }

        public DbSet<Role> Roles { get; set; }
        
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        // public DbSet<Size> Sizes { get; set; }

        public DbSet<Store> Stores { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserStoreRole> UserStoreRoles { get; set; }

        // public DbSet<Vendor> Vendors { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ModelConfigurations.ConfigureDbContext(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
    }
}