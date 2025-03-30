using FluentMigrator;
using RiPOS.Shared.Enums;

namespace RiPOS.Migrations.Migrations;

[Migration(1)]
public class InitialMigration : Migration
{
    public override void Up()
    {
        Create.Table("Users")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Name").AsString(50).NotNullable()
            .WithColumn("Surname").AsString(50).NotNullable()
            .WithColumn("SecondSurname").AsString(50)
            .WithColumn("Username").AsString(50).NotNullable()
            .WithColumn("Email").AsString(100)
            .WithColumn("PasswordHash").AsString(500).NotNullable()
            .WithColumn("PhoneNumber").AsString(25)
            .WithColumn("MobilePhone").AsString(25)
            .WithColumn("ProfileImagePath").AsString(300)
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);
        
        Create.Table("Store")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Name").AsString(50).NotNullable()
            .WithColumn("Address").AsString(400)
            .WithColumn("PhoneNumber").AsString(25)
            .WithColumn("MobilePhone").AsString(25)
            .WithColumn("LogoPath").AsString(300)
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);
        
        Create.Table("Roles")
            .WithColumn("Id").AsInt32().PrimaryKey()
            .WithColumn("Code").AsString(10).NotNullable()
            .WithColumn("Name").AsString(50).NotNullable()
            .WithColumn("Description").AsString(100).NotNullable();
        
        Create.Table("UserStoreRoles")
            .WithColumn("UserId").AsInt32().NotNullable().ForeignKey("FK_userStoreRoles_user_id","Users", "Id")
            .WithColumn("StoreId").AsInt32().NotNullable().ForeignKey("FK_userStoreRoles_store_id", "Store", "Id")
            .WithColumn("RoleId").AsInt32().NotNullable().ForeignKey("FK_userStoreRoles_role_id","Roles", "Id");
        
        // Inserting initial data
        Insert.IntoTable("Roles")
            .Row(new { Id = (int)RoleEnum.SuperAdmin, Code = "SUPERADM", Name = "Super Admin", Description = "SuperAdmin" })
            .Row(new { Id = (int)RoleEnum.Admin, Code = "ADM", Name = "Admin", Description = "Administrator" });


        Insert.IntoTable("Users")
            .Row(new { Name = "Ricardo", Surname = "Esqueda", Username = "ricesqgue", PasswordHash = "" });
        
        Insert.IntoTable("Store")
            .Row(new { Name = "RiPOS Store", Address = "1234 Main St" });
        
        Insert.IntoTable("UserStoreRoles")
            .Row(new { UserId = 1, StoreId = 1, RoleId = (int)RoleEnum.SuperAdmin });
    }

    public override void Down()
    {
        Delete.Table("UserStoreRoles");
        Delete.Table("Roles");
        Delete.Table("Store");
        Delete.Table("Users");
    }
}