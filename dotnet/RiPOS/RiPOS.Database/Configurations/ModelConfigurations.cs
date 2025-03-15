using Microsoft.EntityFrameworkCore;
using RiPOS.Domain.Entities;

namespace RiPOS.Database.Configurations
{
    internal static class ModelConfigurations
    {
        internal static void ConfigureDbContext(ModelBuilder modelBuilder)
        {
            ConfigureUserStoreRoles(modelBuilder);
            ConfigureUniqueOrAlternateKeys(modelBuilder);
        }

        private static void ConfigureUniqueOrAlternateKeys(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>()
                .HasAlternateKey(c => c.Code)
                .HasName("AlternateKey_Company_Code");
        }

        private static void ConfigureUserStoreRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserStoreRole>()
                .HasKey(usr => new { usr.UserId, usr.StoreId, usr.RoleId });

        }
    }
}
