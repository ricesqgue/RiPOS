using Microsoft.EntityFrameworkCore;
using RiPOS.Domain.Entities;

namespace RiPOS.Database.Configurations
{
    internal static class ModelConfigurations
    {
        internal static void ConfigureDbContext(ModelBuilder modelBuilder)
        {
            ConfigureUserStoreRoles(modelBuilder);
        }
        
        private static void ConfigureUserStoreRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserStoreRole>()
                .HasKey(usr => new { usr.UserId, usr.StoreId, usr.RoleId });

        }
    }
}
