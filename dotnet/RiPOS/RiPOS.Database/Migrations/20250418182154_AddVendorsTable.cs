using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RiPOS.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddVendorsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    SecondSurname = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    MobilePhone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "varchar(400)", maxLength: 400, nullable: true),
                    City = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    ZipCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    CountryStateId = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreationDateTime = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreationByUserId = table.Column<int>(type: "integer", nullable: true),
                    LastModificationDateTime = table.Column<DateTime>(type: "timestamptz", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastModificationByUserId = table.Column<int>(type: "integer", nullable: true)
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
                name: "Vendors");
        }
    }
}
