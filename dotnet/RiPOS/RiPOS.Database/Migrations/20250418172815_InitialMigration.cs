using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RiPOS.Shared.Enums;

#nullable disable

namespace RiPOS.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    SecondSurname = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Username = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PasswordHash = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true),
                    MobilePhone = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true),
                    ProfileImagePath = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreationDateTime = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreationByUserId = table.Column<int>(type: "integer", nullable: true),
                    LastModificationDateTime = table.Column<DateTime>(type: "timestamptz", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastModificationByUserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Users_CreationByUserId",
                        column: x => x.CreationByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_Users_LastModificationByUserId",
                        column: x => x.LastModificationByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "varchar(400)", maxLength: 400, nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true),
                    MobilePhone = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true),
                    LogoPath = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreationDateTime = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreationByUserId = table.Column<int>(type: "integer", nullable: true),
                    LastModificationDateTime = table.Column<DateTime>(type: "timestamptz", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastModificationByUserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stores_Users_CreationByUserId",
                        column: x => x.CreationByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Stores_Users_LastModificationByUserId",
                        column: x => x.LastModificationByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserStoreRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    StoreId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStoreRoles", x => new { x.UserId, x.StoreId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserStoreRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserStoreRoles_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserStoreRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stores_CreationByUserId",
                table: "Stores",
                column: "CreationByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_LastModificationByUserId",
                table: "Stores",
                column: "LastModificationByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreationByUserId",
                table: "Users",
                column: "CreationByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_LastModificationByUserId",
                table: "Users",
                column: "LastModificationByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStoreRoles_RoleId",
                table: "UserStoreRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStoreRoles_StoreId",
                table: "UserStoreRoles",
                column: "StoreId");
            
            // insert data
            migrationBuilder.InsertData(table: "Roles", columns: ["Id", "Code", "Name", "Description"], values: new object[,]
            {
                { (int)RoleEnum.SuperAdmin, "SUPERADM", "Super Admin", "SuperAdmin" },
                { (int)RoleEnum.Admin, "ADM", "Admin", "Administrator" }
            });
            
            migrationBuilder.InsertData(table: "Users", columns: ["Name", "Surname", "Username", "PasswordHash"], values: new object[,]
            {
                { "Ricardo", "Esqueda", "ricesqgue", "" }
            });
            
            migrationBuilder.InsertData(table: "Stores", columns: ["Name", "Address"], values: new object[,]
            {
                { "RiPOS Store", "1234 Main St" }
            });
            
            migrationBuilder.InsertData(table: "UserStoreRoles", columns: ["UserId", "StoreId", "RoleId"], values: new object[,]
            {
                { 1, 1, (int)RoleEnum.SuperAdmin }
            });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserStoreRoles");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
