using Microsoft.EntityFrameworkCore;
using RiPOS.Domain.Entities;

namespace RiPOS.Database.Configurations
{
    internal static class ModelConfigurations
    {
        internal static void ConfigureDbContext(ModelBuilder modelBuilder)
        {
            ConfigureDefaultValues(modelBuilder);
            ConfigureUserStoreRoles(modelBuilder);
        }
        
        private static void ConfigureUserStoreRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserStoreRole>()
                .HasKey(usr => new { usr.UserId, usr.StoreId, usr.RoleId });

        }

        private static void ConfigureDefaultValues(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.IsActive)
                .HasDefaultValue(true);
            
            modelBuilder.Entity<User>()
                .Property(u => u.CreationDateTime)
                .HasDefaultValueSql("GETDATE()");
            
            modelBuilder.Entity<User>()
                .Property(u => u.LastModificationDateTime)
                .HasDefaultValueSql("GETDATE()");
            
            modelBuilder.Entity<Store>()
                .Property(s => s.IsActive)
                .HasDefaultValue(true);
            
            modelBuilder.Entity<Store>()
                .Property(s => s.CreationDateTime)
                .HasDefaultValueSql("GETDATE()");
            
            modelBuilder.Entity<Store>()
                .Property(s => s.LastModificationDateTime)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
