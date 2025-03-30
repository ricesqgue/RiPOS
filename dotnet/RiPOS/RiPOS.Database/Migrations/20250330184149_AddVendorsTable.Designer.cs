﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RiPOS.Database;

#nullable disable

namespace RiPOS.Database.Migrations
{
    [DbContext(typeof(RiPosDbContext))]
    [Migration("20250330184149_AddVendorsTable")]
    partial class AddVendorsTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("RiPOS.Domain.Entities.Brand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreationByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<int?>("LastModificationByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastModificationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("LogoPath")
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("CreationByUserId");

                    b.HasIndex("LastModificationByUserId");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.CashRegister", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreationByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<int?>("LastModificationByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastModificationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("StoreId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreationByUserId");

                    b.HasIndex("LastModificationByUserId");

                    b.HasIndex("StoreId");

                    b.ToTable("CashRegisters");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreationByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<int?>("LastModificationByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastModificationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("CreationByUserId");

                    b.HasIndex("LastModificationByUserId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.Color", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreationByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<int?>("LastModificationByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastModificationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("RgbHex")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.HasIndex("CreationByUserId");

                    b.HasIndex("LastModificationByUserId");

                    b.ToTable("Colors");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.CountryState", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.ToTable("CountryStates");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.Property<string>("City")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("CountryStateId")
                        .HasColumnType("int");

                    b.Property<int?>("CreationByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<int?>("LastModificationByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastModificationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("MobilePhone")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Rfc")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("SecondSurname")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ZipCode")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.HasIndex("CountryStateId");

                    b.HasIndex("CreationByUserId");

                    b.HasIndex("LastModificationByUserId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.Gender", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreationByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<int?>("LastModificationByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastModificationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("CreationByUserId");

                    b.HasIndex("LastModificationByUserId");

                    b.ToTable("Genders");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.Size", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreationByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<int?>("LastModificationByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastModificationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.HasIndex("CreationByUserId");

                    b.HasIndex("LastModificationByUserId");

                    b.ToTable("Sizes");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.Store", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.Property<int?>("CreationByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<int?>("LastModificationByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastModificationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("LogoPath")
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<string>("MobilePhone")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.HasKey("Id");

                    b.HasIndex("CreationByUserId");

                    b.HasIndex("LastModificationByUserId");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreationByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<int?>("LastModificationByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastModificationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("MobilePhone")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("ProfileImagePath")
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<string>("SecondSurname")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("CreationByUserId");

                    b.HasIndex("LastModificationByUserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.UserStoreRole", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("StoreId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "StoreId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("StoreId");

                    b.ToTable("UserStoreRoles");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.Vendor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.Property<string>("City")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("CountryStateId")
                        .HasColumnType("int");

                    b.Property<int?>("CreationByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<int?>("LastModificationByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastModificationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("MobilePhone")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("SecondSurname")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ZipCode")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.HasIndex("CountryStateId");

                    b.HasIndex("CreationByUserId");

                    b.HasIndex("LastModificationByUserId");

                    b.ToTable("Vendors");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.Brand", b =>
                {
                    b.HasOne("RiPOS.Domain.Entities.User", "CreationByUser")
                        .WithMany()
                        .HasForeignKey("CreationByUserId");

                    b.HasOne("RiPOS.Domain.Entities.User", "LastModificationByUser")
                        .WithMany()
                        .HasForeignKey("LastModificationByUserId");

                    b.Navigation("CreationByUser");

                    b.Navigation("LastModificationByUser");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.CashRegister", b =>
                {
                    b.HasOne("RiPOS.Domain.Entities.User", "CreationByUser")
                        .WithMany()
                        .HasForeignKey("CreationByUserId");

                    b.HasOne("RiPOS.Domain.Entities.User", "LastModificationByUser")
                        .WithMany()
                        .HasForeignKey("LastModificationByUserId");

                    b.HasOne("RiPOS.Domain.Entities.Store", "Store")
                        .WithMany()
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreationByUser");

                    b.Navigation("LastModificationByUser");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.Category", b =>
                {
                    b.HasOne("RiPOS.Domain.Entities.User", "CreationByUser")
                        .WithMany()
                        .HasForeignKey("CreationByUserId");

                    b.HasOne("RiPOS.Domain.Entities.User", "LastModificationByUser")
                        .WithMany()
                        .HasForeignKey("LastModificationByUserId");

                    b.Navigation("CreationByUser");

                    b.Navigation("LastModificationByUser");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.Color", b =>
                {
                    b.HasOne("RiPOS.Domain.Entities.User", "CreationByUser")
                        .WithMany()
                        .HasForeignKey("CreationByUserId");

                    b.HasOne("RiPOS.Domain.Entities.User", "LastModificationByUser")
                        .WithMany()
                        .HasForeignKey("LastModificationByUserId");

                    b.Navigation("CreationByUser");

                    b.Navigation("LastModificationByUser");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.Customer", b =>
                {
                    b.HasOne("RiPOS.Domain.Entities.CountryState", "CountryState")
                        .WithMany()
                        .HasForeignKey("CountryStateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RiPOS.Domain.Entities.User", "CreationByUser")
                        .WithMany()
                        .HasForeignKey("CreationByUserId");

                    b.HasOne("RiPOS.Domain.Entities.User", "LastModificationByUser")
                        .WithMany()
                        .HasForeignKey("LastModificationByUserId");

                    b.Navigation("CountryState");

                    b.Navigation("CreationByUser");

                    b.Navigation("LastModificationByUser");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.Gender", b =>
                {
                    b.HasOne("RiPOS.Domain.Entities.User", "CreationByUser")
                        .WithMany()
                        .HasForeignKey("CreationByUserId");

                    b.HasOne("RiPOS.Domain.Entities.User", "LastModificationByUser")
                        .WithMany()
                        .HasForeignKey("LastModificationByUserId");

                    b.Navigation("CreationByUser");

                    b.Navigation("LastModificationByUser");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.RefreshToken", b =>
                {
                    b.HasOne("RiPOS.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.Size", b =>
                {
                    b.HasOne("RiPOS.Domain.Entities.User", "CreationByUser")
                        .WithMany()
                        .HasForeignKey("CreationByUserId");

                    b.HasOne("RiPOS.Domain.Entities.User", "LastModificationByUser")
                        .WithMany()
                        .HasForeignKey("LastModificationByUserId");

                    b.Navigation("CreationByUser");

                    b.Navigation("LastModificationByUser");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.Store", b =>
                {
                    b.HasOne("RiPOS.Domain.Entities.User", "CreationByUser")
                        .WithMany()
                        .HasForeignKey("CreationByUserId");

                    b.HasOne("RiPOS.Domain.Entities.User", "LastModificationByUser")
                        .WithMany()
                        .HasForeignKey("LastModificationByUserId");

                    b.Navigation("CreationByUser");

                    b.Navigation("LastModificationByUser");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.User", b =>
                {
                    b.HasOne("RiPOS.Domain.Entities.User", "CreationByUser")
                        .WithMany()
                        .HasForeignKey("CreationByUserId");

                    b.HasOne("RiPOS.Domain.Entities.User", "LastModificationByUser")
                        .WithMany()
                        .HasForeignKey("LastModificationByUserId");

                    b.Navigation("CreationByUser");

                    b.Navigation("LastModificationByUser");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.UserStoreRole", b =>
                {
                    b.HasOne("RiPOS.Domain.Entities.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RiPOS.Domain.Entities.Store", "Store")
                        .WithMany()
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RiPOS.Domain.Entities.User", "User")
                        .WithMany("UserStoreRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("Store");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.Vendor", b =>
                {
                    b.HasOne("RiPOS.Domain.Entities.CountryState", "CountryState")
                        .WithMany()
                        .HasForeignKey("CountryStateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RiPOS.Domain.Entities.User", "CreationByUser")
                        .WithMany()
                        .HasForeignKey("CreationByUserId");

                    b.HasOne("RiPOS.Domain.Entities.User", "LastModificationByUser")
                        .WithMany()
                        .HasForeignKey("LastModificationByUserId");

                    b.Navigation("CountryState");

                    b.Navigation("CreationByUser");

                    b.Navigation("LastModificationByUser");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.User", b =>
                {
                    b.Navigation("UserStoreRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
