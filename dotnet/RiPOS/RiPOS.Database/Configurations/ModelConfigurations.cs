using Microsoft.EntityFrameworkCore;
using RiPOS.Domain.Entities;

namespace RiPOS.Database.Configurations
{
    internal static class ModelConfigurations
    {
        internal static void ConfigureDbContext(ModelBuilder modelBuilder)
        {
            ConfigureDefaultValues(modelBuilder);
            ConfigureRelationalTables(modelBuilder);
            ConfigureIndexes(modelBuilder);
        }
        
        private static void ConfigureRelationalTables(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserStoreRole>()
                .HasKey(usr => new { usr.UserId, usr.StoreId, usr.RoleId });

            modelBuilder.Entity<ProductCategory>(pc =>
            {
                pc.HasKey(x => new { x.ProductHeaderId, x.CategoryId });
                
                pc.HasOne(x => x.ProductHeader)
                    .WithMany(x => x.ProductCategories)
                    .HasForeignKey(x => x.ProductHeaderId);
            });
            
            modelBuilder.Entity<ProductColor>(pc =>
            {
                pc.HasKey(x => new { x.ProductDetailId, x.ColorId });
                
                pc.HasOne(x => x.ProductDetail)
                    .WithMany(x => x.ProductColors)
                    .HasForeignKey(x => x.ProductDetailId);
            });
            
            modelBuilder.Entity<ProductGender>(pg =>
            {
                pg.HasKey(x => new { x.ProductHeaderId, x.GenderId });
                
                pg.HasOne(x => x.ProductHeader)
                    .WithMany(x => x.ProductGenders)
                    .HasForeignKey(x => x.ProductHeaderId);
            });

            modelBuilder.Entity<Inventory>(i =>
            {
                i.HasKey(x => new { x.ProductDetailId, x.StoreId });
            });
        }

        private static void ConfigureDefaultValues(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(u =>
            {
                u.Property(x => x.IsActive)
                    .HasDefaultValue(true);
                u.Property(x => x.CreationDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                u.Property(x => x.LastModificationDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<Store>(s =>
            {
                s.Property(x => x.IsActive)
                    .HasDefaultValue(true);
                s.Property(x => x.CreationDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                s.Property(x => x.LastModificationDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            
            modelBuilder.Entity<Brand>(b =>
            {
                b.Property(x => x.IsActive)
                    .HasDefaultValue(true);
                b.Property(x => x.CreationDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                b.Property(x => x.LastModificationDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            
            modelBuilder.Entity<CashRegister>(c =>
            {
                c.Property(x => x.IsActive)
                    .HasDefaultValue(true);
                c.Property(x => x.CreationDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                c.Property(x => x.LastModificationDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            
            modelBuilder.Entity<Size>(s =>
            {
                s.Property(x => x.IsActive)
                    .HasDefaultValue(true);
                s.Property(x => x.CreationDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                s.Property(x => x.LastModificationDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            
            modelBuilder.Entity<Gender>(g =>
            {
                g.Property(x => x.IsActive)
                    .HasDefaultValue(true);
                g.Property(x => x.CreationDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                g.Property(x => x.LastModificationDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            
            modelBuilder.Entity<Color>(c =>
            {
                c.Property(x => x.IsActive)
                    .HasDefaultValue(true);
                c.Property(x => x.CreationDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                c.Property(x => x.LastModificationDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            
            modelBuilder.Entity<Category>(c =>
            {
                c.Property(x => x.IsActive)
                    .HasDefaultValue(true);
                c.Property(x => x.CreationDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                c.Property(x => x.LastModificationDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            
            modelBuilder.Entity<Customer>(c =>
            {
                c.Property(x => x.IsActive)
                    .HasDefaultValue(true);
                c.Property(x => x.CreationDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                c.Property(x => x.LastModificationDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            
            modelBuilder.Entity<Vendor>(v =>
            {
                v.Property(x => x.IsActive)
                    .HasDefaultValue(true);
                v.Property(x => x.CreationDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                v.Property(x => x.LastModificationDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<ProductHeader>(p =>
            {
                p.Property(x => x.IsActive)
                    .HasDefaultValue(true);
                p.Property(x => x.CreationDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                p.Property(x => x.LastModificationDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            
            modelBuilder.Entity<ProductDetail>(p =>
            {
                p.Property(x => x.IsActive)
                    .HasDefaultValue(true);
                p.Property(x => x.CreationDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                p.Property(x => x.LastModificationDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<InventoryTransfer>(i =>
            {
                i.Property(x => x.TransferDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            
            modelBuilder.Entity<PurchaseOrder>(p =>
            {
                p.Property(x => x.CreationDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                p.Property(x => x.LastModificationDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                p.Property(x => x.PurchaseOrderStatusId)
                    .HasConversion<int>();
            });
            
            modelBuilder.Entity<PurchaseOrderDetail>(p =>
            {
                p.Property(x => x.CreationDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                p.Property(x => x.LastModificationDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            
            modelBuilder.Entity<PurchaseOrderNote>(p =>
            {
                p.Property(x => x.IsDeleted).HasDefaultValue(false);
                p.Property(x => x.CreationDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                p.Property(x => x.LastModificationDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            
            modelBuilder.Entity<PurchaseOrderStatus>(p =>
            {
                p.Property(x => x.IsActive)
                    .HasDefaultValue(true);
            });

            modelBuilder.Entity<PaymentMethod>(p =>
            {
                p.Property(x => x.IsActive)
                    .HasDefaultValue(true);
            });
            
            modelBuilder.Entity<VendorDebt>(v =>
            {
                v.Property(x => x.CreationDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                v.Property(x => x.LastModificationDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            
            modelBuilder.Entity<VendorPayment>(v =>
            {
                v.Property(x => x.IsDeleted)
                    .HasDefaultValue(false);
                v.Property(x => x.CreationDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                v.Property(x => x.LastModificationDateTime)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }

        private static void ConfigureIndexes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PurchaseOrder>()
                .HasIndex(po => new { po.VendorId, po.InvoiceNumber })
                .IsUnique();
        }
    }
}
