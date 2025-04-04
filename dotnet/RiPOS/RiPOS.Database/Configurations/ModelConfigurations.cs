﻿using Microsoft.EntityFrameworkCore;
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
            
            modelBuilder.Entity<Brand>()
                .Property(b => b.IsActive)
                .HasDefaultValue(true);
            
            modelBuilder.Entity<Brand>()
                .Property(b => b.CreationDateTime)
                .HasDefaultValueSql("GETDATE()");
            
            modelBuilder.Entity<Brand>()
                .Property(b => b.LastModificationDateTime)
                .HasDefaultValueSql("GETDATE()");
            
            modelBuilder.Entity<CashRegister>()
                .Property(c => c.IsActive)
                .HasDefaultValue(true);
            
            modelBuilder.Entity<CashRegister>()
                .Property(c => c.CreationDateTime)
                .HasDefaultValueSql("GETDATE()");
            
            modelBuilder.Entity<CashRegister>()
                .Property(c => c.LastModificationDateTime)
                .HasDefaultValueSql("GETDATE()");
            
            modelBuilder.Entity<Size>()
                .Property(s => s.IsActive)
                .HasDefaultValue(true);
            
            modelBuilder.Entity<Size>()
                .Property(s => s.CreationDateTime)
                .HasDefaultValueSql("GETDATE()");
            
            modelBuilder.Entity<Size>()
                .Property(s => s.LastModificationDateTime)
                .HasDefaultValueSql("GETDATE()");
            
            modelBuilder.Entity<Gender>()
                .Property(g => g.IsActive)
                .HasDefaultValue(true);
            
            modelBuilder.Entity<Gender>()
                .Property(g => g.CreationDateTime)
                .HasDefaultValueSql("GETDATE()");
            
            modelBuilder.Entity<Gender>()
                .Property(g => g.LastModificationDateTime)
                .HasDefaultValueSql("GETDATE()");
            
            modelBuilder.Entity<Color>()
                .Property(c => c.IsActive)
                .HasDefaultValue(true);
            
            modelBuilder.Entity<Color>()
                .Property(c => c.CreationDateTime)
                .HasDefaultValueSql("GETDATE()");
            
            modelBuilder.Entity<Color>()
                .Property(c => c.LastModificationDateTime)
                .HasDefaultValueSql("GETDATE()");
            
            modelBuilder.Entity<Category>()
                .Property(c => c.IsActive)
                .HasDefaultValue(true);
            
            modelBuilder.Entity<Category>()
                .Property(c => c.CreationDateTime)
                .HasDefaultValueSql("GETDATE()");
            
            modelBuilder.Entity<Category>()
                .Property(c => c.LastModificationDateTime)
                .HasDefaultValueSql("GETDATE()");
            
            modelBuilder.Entity<Customer>()
                .Property(c => c.IsActive)
                .HasDefaultValue(true);
            
            modelBuilder.Entity<Customer>()
                .Property(c => c.CreationDateTime)
                .HasDefaultValueSql("GETDATE()");
            
            modelBuilder.Entity<Customer>()
                .Property(c => c.LastModificationDateTime)
                .HasDefaultValueSql("GETDATE()");
            
            modelBuilder.Entity<Vendor>()
                .Property(v => v.IsActive)
                .HasDefaultValue(true);
            
            modelBuilder.Entity<Vendor>()
                .Property(v => v.CreationDateTime)
                .HasDefaultValueSql("GETDATE()");
            
            modelBuilder.Entity<Vendor>()
                .Property(v => v.LastModificationDateTime)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
