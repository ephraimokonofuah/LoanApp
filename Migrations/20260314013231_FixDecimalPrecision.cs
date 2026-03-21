using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LoanApp.Migrations
{
    /// <inheritdoc />
    public partial class FixDecimalPrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "LoanApplications",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "LoanPurpose",
                table: "LoanApplications",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "LoanApplications",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanApplicationId = table.Column<int>(type: "int", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_LoanApplications_LoanApplicationId",
                        column: x => x.LoanApplicationId,
                        principalTable: "LoanApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoanApplicationId = table.Column<int>(type: "int", nullable: false),
                    PrincipalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    InterestRate = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    DurationMonths = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loans_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Loans_LoanApplications_LoanApplicationId",
                        column: x => x.LoanApplicationId,
                        principalTable: "LoanApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Repayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Repayments_Loans_LoanId",
                        column: x => x.LoanId,
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "seed-user-1", 0, "123 Main Street", "c260a60c-478f-40ad-ad19-f17650bf0c8e", new DateTime(2026, 3, 14, 1, 32, 30, 582, DateTimeKind.Utc).AddTicks(6050), "john@example.com", true, "John Doe", false, null, "JOHN@EXAMPLE.COM", "JOHN@EXAMPLE.COM", null, null, false, "a67c73fe-bf37-43f8-bde9-e28e002a85b0", false, "john@example.com" },
                    { "seed-user-2", 0, "45 Manchester Road", "a87d5d9f-d08d-4e03-81ca-2cced27f4dee", new DateTime(2026, 3, 14, 1, 32, 30, 582, DateTimeKind.Utc).AddTicks(6150), "mary@example.com", true, "Mary Smith", false, null, "MARY@EXAMPLE.COM", "MARY@EXAMPLE.COM", null, null, false, "286e1512-73ff-4c3d-a67d-237186325f40", false, "mary@example.com" }
                });

            migrationBuilder.UpdateData(
                table: "LoanApplications",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "DurationMonths", "InterestRate", "LoanPurpose", "UserId" },
                values: new object[] { new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, 8.5m, "Small Business Expansion", "seed-user-1" });

            migrationBuilder.UpdateData(
                table: "LoanApplications",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "DurationMonths", "InterestRate", "LoanAmount", "UserId" },
                values: new object[] { new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 24, 9.2m, 12000m, "seed-user-1" });

            migrationBuilder.UpdateData(
                table: "LoanApplications",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "DurationMonths", "InterestRate", "LoanAmount", "UserId" },
                values: new object[] { new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 7.5m, 2000m, "seed-user-2" });

            migrationBuilder.CreateIndex(
                name: "IX_LoanApplications_UserId",
                table: "LoanApplications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_LoanApplicationId",
                table: "Documents",
                column: "LoanApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_LoanApplicationId",
                table: "Loans",
                column: "LoanApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Loans_UserId",
                table: "Loans",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Repayments_LoanId",
                table: "Repayments",
                column: "LoanId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanApplications_AspNetUsers_UserId",
                table: "LoanApplications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanApplications_AspNetUsers_UserId",
                table: "LoanApplications");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Repayments");

            migrationBuilder.DropTable(
                name: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_LoanApplications_UserId",
                table: "LoanApplications");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-1");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-2");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "LoanApplications",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LoanPurpose",
                table: "LoanApplications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "LoanApplications",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "DurationMonths", "InterestRate", "LoanPurpose" },
                values: new object[] { new DateTime(2026, 3, 13, 20, 39, 23, 315, DateTimeKind.Utc).AddTicks(4440), 24, 5.5m, "Home Improvement" });

            migrationBuilder.UpdateData(
                table: "LoanApplications",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "DurationMonths", "InterestRate", "LoanAmount" },
                values: new object[] { new DateTime(2026, 3, 13, 20, 39, 23, 315, DateTimeKind.Utc).AddTicks(4450), 36, 4.2m, 15000m });

            migrationBuilder.UpdateData(
                table: "LoanApplications",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "DurationMonths", "InterestRate", "LoanAmount" },
                values: new object[] { new DateTime(2026, 3, 13, 20, 39, 23, 315, DateTimeKind.Utc).AddTicks(4450), 12, 6.0m, 3000m });
        }
    }
}
