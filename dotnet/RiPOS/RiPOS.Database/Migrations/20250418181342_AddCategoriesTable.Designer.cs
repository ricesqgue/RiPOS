﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RiPOS.Database;

#nullable disable

namespace RiPOS.Database.Migrations
{
    [DbContext(typeof(RiPosDbContext))]
    [Migration("20250418181342_AddCategoriesTable")]
    partial class AddCategoriesTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("RiPOS.Domain.Entities.Brand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreationByUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamptz")
                        .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<int?>("LastModificationByUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("LastModificationDateTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamptz")
                        .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)");

                    b.Property<string>("LogoPath")
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("CreationByUserId");

                    b.HasIndex("LastModificationByUserId");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.CashRegister", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreationByUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamptz")
                        .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<int?>("LastModificationByUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("LastModificationDateTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamptz")
                        .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("StoreId")
                        .HasColumnType("integer");

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
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreationByUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamptz")
                        .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<int?>("LastModificationByUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("LastModificationDateTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamptz")
                        .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("CreationByUserId");

                    b.HasIndex("LastModificationByUserId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.Color", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreationByUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamptz")
                        .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<int?>("LastModificationByUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("LastModificationDateTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamptz")
                        .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("RgbHex")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.HasKey("Id");

                    b.HasIndex("CreationByUserId");

                    b.HasIndex("LastModificationByUserId");

                    b.ToTable("Colors");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.CountryState", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.HasKey("Id");

                    b.ToTable("CountryStates");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.Gender", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreationByUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamptz")
                        .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<int?>("LastModificationByUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("LastModificationDateTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamptz")
                        .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("CreationByUserId");

                    b.HasIndex("LastModificationByUserId");

                    b.ToTable("Genders");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamptz");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("timestamptz");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.Size", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreationByUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamptz")
                        .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<int?>("LastModificationByUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("LastModificationDateTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamptz")
                        .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.HasKey("Id");

                    b.HasIndex("CreationByUserId");

                    b.HasIndex("LastModificationByUserId");

                    b.ToTable("Sizes");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.Store", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasMaxLength(400)
                        .HasColumnType("varchar(400)");

                    b.Property<int?>("CreationByUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamptz")
                        .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<int?>("LastModificationByUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("LastModificationDateTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamptz")
                        .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)");

                    b.Property<string>("LogoPath")
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.Property<string>("MobilePhone")
                        .HasMaxLength(25)
                        .HasColumnType("varchar(25)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(25)
                        .HasColumnType("varchar(25)");

                    b.HasKey("Id");

                    b.HasIndex("CreationByUserId");

                    b.HasIndex("LastModificationByUserId");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreationByUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamptz")
                        .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<int?>("LastModificationByUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("LastModificationDateTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamptz")
                        .HasDefaultValueSql("TIMEZONE('utc', CURRENT_TIMESTAMP)");

                    b.Property<string>("MobilePhone")
                        .HasMaxLength(25)
                        .HasColumnType("varchar(25)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(25)
                        .HasColumnType("varchar(25)");

                    b.Property<string>("ProfileImagePath")
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.Property<string>("SecondSurname")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("CreationByUserId");

                    b.HasIndex("LastModificationByUserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("RiPOS.Domain.Entities.UserStoreRole", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("StoreId")
                        .HasColumnType("integer");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "StoreId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("StoreId");

                    b.ToTable("UserStoreRoles");
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

            modelBuilder.Entity("RiPOS.Domain.Entities.User", b =>
                {
                    b.Navigation("UserStoreRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
