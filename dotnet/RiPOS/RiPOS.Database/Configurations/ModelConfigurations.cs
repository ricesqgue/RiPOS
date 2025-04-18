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
            modelBuilder.Entity<User>(u =>
            {
                u.Property(x => x.IsActive)
                    .HasDefaultValue(true);
                u.Property(x => x.CreationDateTime)
                    .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)");
                u.Property(x => x.LastModificationDateTime)
                    .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)")
                    .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<Store>(s =>
            {
                s.Property(x => x.IsActive)
                    .HasDefaultValue(true);
                s.Property(x => x.CreationDateTime)
                    .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)");
                s.Property(x => x.LastModificationDateTime)
                    .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)")
                    .ValueGeneratedOnAddOrUpdate();
            });
            
            modelBuilder.Entity<Brand>(b =>
            {
                b.Property(x => x.IsActive)
                    .HasDefaultValue(true);
                b.Property(x => x.CreationDateTime)
                    .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)");
                b.Property(x => x.LastModificationDateTime)
                    .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)")
                    .ValueGeneratedOnAddOrUpdate();
            });
            
            modelBuilder.Entity<CashRegister>(c =>
            {
                c.Property(x => x.IsActive)
                    .HasDefaultValue(true);
                c.Property(x => x.CreationDateTime)
                    .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)");
                c.Property(x => x.LastModificationDateTime)
                    .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)")
                    .ValueGeneratedOnAddOrUpdate();
            });
            
            modelBuilder.Entity<Size>(s =>
            {
                s.Property(x => x.IsActive)
                    .HasDefaultValue(true);
                s.Property(x => x.CreationDateTime)
                    .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)");
                s.Property(x => x.LastModificationDateTime)
                    .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)")
                    .ValueGeneratedOnAddOrUpdate();
            });
            
            modelBuilder.Entity<Gender>(g =>
            {
                g.Property(x => x.IsActive)
                    .HasDefaultValue(true);
                g.Property(x => x.CreationDateTime)
                    .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)");
                g.Property(x => x.LastModificationDateTime)
                    .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)")
                    .ValueGeneratedOnAddOrUpdate();
            });
            
            modelBuilder.Entity<Color>(c =>
            {
                c.Property(x => x.IsActive)
                    .HasDefaultValue(true);
                c.Property(x => x.CreationDateTime)
                    .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)");
                c.Property(x => x.LastModificationDateTime)
                    .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)")
                    .ValueGeneratedOnAddOrUpdate();
            });
            
            modelBuilder.Entity<Category>(c =>
            {
                c.Property(x => x.IsActive)
                    .HasDefaultValue(true);
                c.Property(x => x.CreationDateTime)
                    .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)");
                c.Property(x => x.LastModificationDateTime)
                    .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)")
                    .ValueGeneratedOnAddOrUpdate();
            });
            
            modelBuilder.Entity<Customer>(c =>
            {
                c.Property(x => x.IsActive)
                    .HasDefaultValue(true);
                c.Property(x => x.CreationDateTime)
                    .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)");
                c.Property(x => x.LastModificationDateTime)
                    .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)")
                    .ValueGeneratedOnAddOrUpdate();
            });
            
            modelBuilder.Entity<Vendor>(v =>
            {
                v.Property(x => x.IsActive)
                    .HasDefaultValue(true);
                v.Property(x => x.CreationDateTime)
                    .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)");
                v.Property(x => x.LastModificationDateTime)
                    .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)")
                    .ValueGeneratedOnAddOrUpdate();
            });
        }
    }
}
