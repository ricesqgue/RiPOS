using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RiPOS.Shared.Enums;

#nullable disable

namespace RiPOS.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddVendorDebtAndPaymentTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "PurchaseOrders",
                newName: "PurchaseOrderStatusId");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "PurchaseOrderNotes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VendorDebts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PurchaseOrderId = table.Column<int>(type: "integer", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    RemainingAmount = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    VendorId = table.Column<int>(type: "integer", nullable: true),
                    CreationDateTime = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreationByUserId = table.Column<int>(type: "integer", nullable: true),
                    LastModificationDateTime = table.Column<DateTime>(type: "timestamptz", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastModificationByUserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorDebts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VendorDebts_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VendorDebts_Users_CreationByUserId",
                        column: x => x.CreationByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VendorDebts_Users_LastModificationByUserId",
                        column: x => x.LastModificationByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VendorDebts_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VendorPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VendorDebtId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    PaymentDateTime = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    ReferenceCode = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    PaymentMethodId = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreationDateTime = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreationByUserId = table.Column<int>(type: "integer", nullable: true),
                    LastModificationDateTime = table.Column<DateTime>(type: "timestamptz", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastModificationByUserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VendorPayments_PaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VendorPayments_Users_CreationByUserId",
                        column: x => x.CreationByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VendorPayments_Users_LastModificationByUserId",
                        column: x => x.LastModificationByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VendorPayments_VendorDebts_VendorDebtId",
                        column: x => x.VendorDebtId,
                        principalTable: "VendorDebts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_PurchaseOrderStatusId",
                table: "PurchaseOrders",
                column: "PurchaseOrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorDebts_CreationByUserId",
                table: "VendorDebts",
                column: "CreationByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorDebts_LastModificationByUserId",
                table: "VendorDebts",
                column: "LastModificationByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorDebts_PurchaseOrderId",
                table: "VendorDebts",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorDebts_VendorId",
                table: "VendorDebts",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorPayments_CreationByUserId",
                table: "VendorPayments",
                column: "CreationByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorPayments_LastModificationByUserId",
                table: "VendorPayments",
                column: "LastModificationByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorPayments_PaymentMethodId",
                table: "VendorPayments",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorPayments_VendorDebtId",
                table: "VendorPayments",
                column: "VendorDebtId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_PurchaseOrderStatus_PurchaseOrderStatusId",
                table: "PurchaseOrders",
                column: "PurchaseOrderStatusId",
                principalTable: "PurchaseOrderStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            
            // Insert data for Payment Methods
            migrationBuilder.InsertData(table: "PaymentMethods", columns: ["Id", "Name"], values: new object[,]
            {
                { (int)PaymentMethodEnum.Cash, "Efectivo" },
                { (int)PaymentMethodEnum.BankTransfer, "Transferencia" },
                { (int)PaymentMethodEnum.Check, "Cheque" },
                { (int)PaymentMethodEnum.CreditCard, "Tarjeta de crédito" },
                { (int)PaymentMethodEnum.DebitCard, "Tarjeta de débito" },
            });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_PurchaseOrderStatus_PurchaseOrderStatusId",
                table: "PurchaseOrders");

            migrationBuilder.DropTable(
                name: "VendorPayments");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.DropTable(
                name: "VendorDebts");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_PurchaseOrderStatusId",
                table: "PurchaseOrders");

            migrationBuilder.RenameColumn(
                name: "PurchaseOrderStatusId",
                table: "PurchaseOrders",
                newName: "Status");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "PurchaseOrderNotes",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
