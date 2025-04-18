using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RiPOS.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddSizesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    ShortName = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreationDateTime = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreationByUserId = table.Column<int>(type: "integer", nullable: true),
                    LastModificationDateTime = table.Column<DateTime>(type: "timestamptz", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastModificationByUserId = table.Column<int>(type: "integer", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_Sizes_CreationByUserId",
                table: "Sizes",
                column: "CreationByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sizes_LastModificationByUserId",
                table: "Sizes",
                column: "LastModificationByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sizes");
        }
    }
}
