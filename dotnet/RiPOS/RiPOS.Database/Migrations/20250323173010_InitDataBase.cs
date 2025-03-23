using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiPOS.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitDataBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CountryStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SecondSurname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    MobilePhone = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    ProfileImagePath = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationByUserId = table.Column<int>(type: "int", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModificationByUserId = table.Column<int>(type: "int", nullable: true)
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
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LogoPath = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationByUserId = table.Column<int>(type: "int", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModificationByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Brands_Users_CreationByUserId",
                        column: x => x.CreationByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Brands_Users_LastModificationByUserId",
                        column: x => x.LastModificationByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationByUserId = table.Column<int>(type: "int", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModificationByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Users_CreationByUserId",
                        column: x => x.CreationByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Categories_Users_LastModificationByUserId",
                        column: x => x.LastModificationByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RgbHex = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationByUserId = table.Column<int>(type: "int", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModificationByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Colors_Users_CreationByUserId",
                        column: x => x.CreationByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Colors_Users_LastModificationByUserId",
                        column: x => x.LastModificationByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SecondSurname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MobilePhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Rfc = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    CountryStateId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationByUserId = table.Column<int>(type: "int", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModificationByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_CountryStates_CountryStateId",
                        column: x => x.CountryStateId,
                        principalTable: "CountryStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Customers_Users_CreationByUserId",
                        column: x => x.CreationByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Customers_Users_LastModificationByUserId",
                        column: x => x.LastModificationByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationByUserId = table.Column<int>(type: "int", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModificationByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Genders_Users_CreationByUserId",
                        column: x => x.CreationByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Genders_Users_LastModificationByUserId",
                        column: x => x.LastModificationByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Sizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationByUserId = table.Column<int>(type: "int", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModificationByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sizes_Users_CreationByUserId",
                        column: x => x.CreationByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Sizes_Users_LastModificationByUserId",
                        column: x => x.LastModificationByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    MobilePhone = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    LogoPath = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationByUserId = table.Column<int>(type: "int", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModificationByUserId = table.Column<int>(type: "int", nullable: true)
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
                name: "Vendors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SecondSurname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MobilePhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CountryStateId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationByUserId = table.Column<int>(type: "int", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModificationByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vendors_CountryStates_CountryStateId",
                        column: x => x.CountryStateId,
                        principalTable: "CountryStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vendors_Users_CreationByUserId",
                        column: x => x.CreationByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Vendors_Users_LastModificationByUserId",
                        column: x => x.LastModificationByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CashRegisters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationByUserId = table.Column<int>(type: "int", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModificationByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashRegisters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashRegisters_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CashRegisters_Users_CreationByUserId",
                        column: x => x.CreationByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CashRegisters_Users_LastModificationByUserId",
                        column: x => x.LastModificationByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserStoreRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
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
                name: "IX_Brands_CreationByUserId",
                table: "Brands",
                column: "CreationByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_LastModificationByUserId",
                table: "Brands",
                column: "LastModificationByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CashRegisters_CreationByUserId",
                table: "CashRegisters",
                column: "CreationByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CashRegisters_LastModificationByUserId",
                table: "CashRegisters",
                column: "LastModificationByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CashRegisters_StoreId",
                table: "CashRegisters",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CreationByUserId",
                table: "Categories",
                column: "CreationByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_LastModificationByUserId",
                table: "Categories",
                column: "LastModificationByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Colors_CreationByUserId",
                table: "Colors",
                column: "CreationByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Colors_LastModificationByUserId",
                table: "Colors",
                column: "LastModificationByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CountryStateId",
                table: "Customers",
                column: "CountryStateId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CreationByUserId",
                table: "Customers",
                column: "CreationByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_LastModificationByUserId",
                table: "Customers",
                column: "LastModificationByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Genders_CreationByUserId",
                table: "Genders",
                column: "CreationByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Genders_LastModificationByUserId",
                table: "Genders",
                column: "LastModificationByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sizes_CreationByUserId",
                table: "Sizes",
                column: "CreationByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sizes_LastModificationByUserId",
                table: "Sizes",
                column: "LastModificationByUserId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_CountryStateId",
                table: "Vendors",
                column: "CountryStateId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_CreationByUserId",
                table: "Vendors",
                column: "CreationByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_LastModificationByUserId",
                table: "Vendors",
                column: "LastModificationByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "CashRegisters");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Genders");

            migrationBuilder.DropTable(
                name: "Sizes");

            migrationBuilder.DropTable(
                name: "UserStoreRoles");

            migrationBuilder.DropTable(
                name: "Vendors");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "CountryStates");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
