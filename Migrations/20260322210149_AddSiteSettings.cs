using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanApp.Migrations
{
    /// <inheritdoc />
    public partial class AddSiteSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SiteSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SecondaryEmail = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SecondaryPhone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    AddressLine1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AddressLine2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BusinessHours = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FooterDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FacebookUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TwitterUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LinkedInUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    InstagramUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NmlsNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteSettings", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "4cdb4c70-c311-4608-ab24-708a8aa55beb", new DateTime(2026, 3, 22, 21, 1, 47, 940, DateTimeKind.Utc).AddTicks(2310), "2e06508b-8c1b-4480-bb28-f9e2e9b60b18" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "1b362c3d-2fa6-40fe-9afe-d0c066985978", new DateTime(2026, 3, 22, 21, 1, 47, 940, DateTimeKind.Utc).AddTicks(2490), "44d3ef06-22a3-401f-9a4a-18bf89670388" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 22, 21, 1, 47, 941, DateTimeKind.Utc).AddTicks(5820));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 22, 21, 1, 47, 941, DateTimeKind.Utc).AddTicks(5820));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 22, 21, 1, 47, 941, DateTimeKind.Utc).AddTicks(5820));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 22, 21, 1, 47, 941, DateTimeKind.Utc).AddTicks(5830));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 22, 21, 1, 47, 941, DateTimeKind.Utc).AddTicks(5830));

            migrationBuilder.InsertData(
                table: "SiteSettings",
                columns: new[] { "Id", "AddressLine1", "AddressLine2", "BusinessHours", "Email", "FacebookUrl", "FooterDescription", "InstagramUrl", "LinkedInUrl", "NmlsNumber", "Phone", "SecondaryEmail", "SecondaryPhone", "SiteName", "TwitterUrl", "UpdatedAt" },
                values: new object[] { 1, "123 Finance Street, Suite 400", "New York, NY 10001", "Mon – Fri: 8AM – 6PM EST", "support@loanapp.com", null, "Trusted lending solutions with competitive rates and transparent terms. We're committed to helping you achieve your financial goals with fast approvals and flexible repayment options.", null, null, "123456", "(800) 555-LOAN", null, null, "LoanApp", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SiteSettings");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "7770dc8c-a73b-4ebb-b2ac-0ba3ecbccc84", new DateTime(2026, 3, 22, 18, 18, 56, 914, DateTimeKind.Utc).AddTicks(1320), "0779cf6a-2475-4e5d-9e21-312a98d71eeb" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "71178110-d4d1-4a29-ac69-f07961801dce", new DateTime(2026, 3, 22, 18, 18, 56, 914, DateTimeKind.Utc).AddTicks(1430), "b21251ba-e51a-4d49-8a7e-d3435794cd61" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 22, 18, 18, 56, 915, DateTimeKind.Utc).AddTicks(2500));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 22, 18, 18, 56, 915, DateTimeKind.Utc).AddTicks(2500));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 22, 18, 18, 56, 915, DateTimeKind.Utc).AddTicks(2500));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 22, 18, 18, 56, 915, DateTimeKind.Utc).AddTicks(2500));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 22, 18, 18, 56, 915, DateTimeKind.Utc).AddTicks(2500));
        }
    }
}
