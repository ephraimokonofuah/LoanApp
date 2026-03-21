using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanApp.Migrations
{
    /// <inheritdoc />
    public partial class AddEligibilityCheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EligibilityChecks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmploymentStatus = table.Column<int>(type: "int", nullable: false),
                    MonthlyIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MonthlyExpenses = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreditScoreRange = table.Column<int>(type: "int", nullable: false),
                    LoanTypeId = table.Column<int>(type: "int", nullable: false),
                    DesiredLoanAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DurationMonths = table.Column<int>(type: "int", nullable: false),
                    IsEligible = table.Column<bool>(type: "bit", nullable: false),
                    EligibilityReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MonthlyRepayment = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalRepayment = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalInterest = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InterestRate = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EligibilityChecks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EligibilityChecks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EligibilityChecks_LoanTypes_LoanTypeId",
                        column: x => x.LoanTypeId,
                        principalTable: "LoanTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "5f534199-52b6-44c0-bde9-8628795bf56c", new DateTime(2026, 3, 18, 16, 23, 38, 34, DateTimeKind.Utc).AddTicks(980), "ea2bea87-5868-4fd1-8935-a9ec7f9ac853" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "32c463b2-d1cf-4e2f-b99a-2afdfc63e9af", new DateTime(2026, 3, 18, 16, 23, 38, 34, DateTimeKind.Utc).AddTicks(1110), "ac92f52e-3413-4cd3-b851-f80352a8ba04" });

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityChecks_LoanTypeId",
                table: "EligibilityChecks",
                column: "LoanTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityChecks_UserId",
                table: "EligibilityChecks",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EligibilityChecks");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "4e4fa387-de04-4a4b-a712-6808afcbc035", new DateTime(2026, 3, 18, 13, 38, 58, 462, DateTimeKind.Utc).AddTicks(5630), "9f46c875-f1ce-4cf7-8c0c-009c729c381b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "0e38a60e-9b71-42d8-83c6-463f333b7b2a", new DateTime(2026, 3, 18, 13, 38, 58, 462, DateTimeKind.Utc).AddTicks(5740), "38e29280-1f5b-48a7-9280-5c30c3bc379a" });
        }
    }
}
