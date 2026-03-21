using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LoanApp.Migrations
{
    /// <inheritdoc />
    public partial class AddBankAndDisbursement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoanDisbursements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanApplicationId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BankId = table.Column<int>(type: "int", nullable: true),
                    AccountName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    SortCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ApprovedAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PaymentReference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanDisbursements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoanDisbursements_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoanDisbursements_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoanDisbursements_LoanApplications_LoanApplicationId",
                        column: x => x.LoanApplicationId,
                        principalTable: "LoanApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "5f72e1c7-25f6-466e-b0da-da6f81928bb3", new DateTime(2026, 3, 20, 9, 48, 3, 845, DateTimeKind.Utc).AddTicks(9390), "6d38bf72-189d-4c6d-8ed3-38613ead894a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "1e2487fe-619e-4580-a70e-bcae7a8ddbed", new DateTime(2026, 3, 20, 9, 48, 3, 845, DateTimeKind.Utc).AddTicks(9510), "8b22f14a-d192-46f0-935a-e61ebbd23d50" });

            migrationBuilder.InsertData(
                table: "Banks",
                columns: new[] { "Id", "Code", "CreatedAt", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, "BARC", new DateTime(2026, 3, 20, 9, 48, 3, 847, DateTimeKind.Utc).AddTicks(5000), true, "Barclays" },
                    { 2, "HSBC", new DateTime(2026, 3, 20, 9, 48, 3, 847, DateTimeKind.Utc).AddTicks(5010), true, "HSBC" },
                    { 3, "LLOY", new DateTime(2026, 3, 20, 9, 48, 3, 847, DateTimeKind.Utc).AddTicks(5010), true, "Lloyds Banking Group" },
                    { 4, "NATW", new DateTime(2026, 3, 20, 9, 48, 3, 847, DateTimeKind.Utc).AddTicks(5010), true, "NatWest" },
                    { 5, "SANT", new DateTime(2026, 3, 20, 9, 48, 3, 847, DateTimeKind.Utc).AddTicks(5010), true, "Santander UK" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoanDisbursements_BankId",
                table: "LoanDisbursements",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanDisbursements_LoanApplicationId",
                table: "LoanDisbursements",
                column: "LoanApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoanDisbursements_UserId",
                table: "LoanDisbursements",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoanDisbursements");

            migrationBuilder.DropTable(
                name: "Banks");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "d1181bd2-73b4-4df7-93b0-f4a9d6a3dade", new DateTime(2026, 3, 18, 18, 16, 47, 354, DateTimeKind.Utc).AddTicks(8240), "0708a94d-4c20-4b22-8251-a2f37b923e94" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "f8ba4487-9f27-4a59-aee5-ef3ea0fb30ba", new DateTime(2026, 3, 18, 18, 16, 47, 354, DateTimeKind.Utc).AddTicks(8390), "1a46a4e8-de20-47d4-ac89-b654aceb162c" });
        }
    }
}
