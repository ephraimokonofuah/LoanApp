using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanApp.Migrations
{
    /// <inheritdoc />
    public partial class RepaymentOverhaul : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Repayments_Loans_LoanId",
                table: "Repayments");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Repayments");

            migrationBuilder.DropColumn(
                name: "Reference",
                table: "Repayments");

            migrationBuilder.RenameColumn(
                name: "PaymentDate",
                table: "Repayments",
                newName: "DueDate");

            migrationBuilder.AddColumn<int>(
                name: "InstallmentNumber",
                table: "Repayments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "InterestPortion",
                table: "Repayments",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsReadByAdmin",
                table: "Repayments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReadByUser",
                table: "Repayments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidAt",
                table: "Repayments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentDetails",
                table: "Repayments",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDetailsSentAt",
                table: "Repayments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodRequested",
                table: "Repayments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentRequestedAt",
                table: "Repayments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PrincipalPortion",
                table: "Repayments",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Repayments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TransactionReference",
                table: "Repayments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Repayments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.CreateIndex(
                name: "IX_Repayments_UserId",
                table: "Repayments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Repayments_AspNetUsers_UserId",
                table: "Repayments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Repayments_Loans_LoanId",
                table: "Repayments",
                column: "LoanId",
                principalTable: "Loans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Repayments_AspNetUsers_UserId",
                table: "Repayments");

            migrationBuilder.DropForeignKey(
                name: "FK_Repayments_Loans_LoanId",
                table: "Repayments");

            migrationBuilder.DropIndex(
                name: "IX_Repayments_UserId",
                table: "Repayments");

            migrationBuilder.DropColumn(
                name: "InstallmentNumber",
                table: "Repayments");

            migrationBuilder.DropColumn(
                name: "InterestPortion",
                table: "Repayments");

            migrationBuilder.DropColumn(
                name: "IsReadByAdmin",
                table: "Repayments");

            migrationBuilder.DropColumn(
                name: "IsReadByUser",
                table: "Repayments");

            migrationBuilder.DropColumn(
                name: "PaidAt",
                table: "Repayments");

            migrationBuilder.DropColumn(
                name: "PaymentDetails",
                table: "Repayments");

            migrationBuilder.DropColumn(
                name: "PaymentDetailsSentAt",
                table: "Repayments");

            migrationBuilder.DropColumn(
                name: "PaymentMethodRequested",
                table: "Repayments");

            migrationBuilder.DropColumn(
                name: "PaymentRequestedAt",
                table: "Repayments");

            migrationBuilder.DropColumn(
                name: "PrincipalPortion",
                table: "Repayments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Repayments");

            migrationBuilder.DropColumn(
                name: "TransactionReference",
                table: "Repayments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Repayments");

            migrationBuilder.RenameColumn(
                name: "DueDate",
                table: "Repayments",
                newName: "PaymentDate");

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "Repayments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "Repayments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "3a8d4dba-06de-490e-80a2-de14ebe0ad88", new DateTime(2026, 3, 20, 11, 50, 7, 844, DateTimeKind.Utc).AddTicks(5030), "5206481f-7ed0-4bb6-9254-5b3a509443b5" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "1895f275-abb0-4a67-9b32-42f6d52e4fc2", new DateTime(2026, 3, 20, 11, 50, 7, 844, DateTimeKind.Utc).AddTicks(5150), "10c68971-b1a9-4a24-8317-f52568b05151" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 20, 11, 50, 7, 845, DateTimeKind.Utc).AddTicks(3580));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 20, 11, 50, 7, 845, DateTimeKind.Utc).AddTicks(3580));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 20, 11, 50, 7, 845, DateTimeKind.Utc).AddTicks(3580));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 20, 11, 50, 7, 845, DateTimeKind.Utc).AddTicks(3590));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 20, 11, 50, 7, 845, DateTimeKind.Utc).AddTicks(3590));

            migrationBuilder.AddForeignKey(
                name: "FK_Repayments_Loans_LoanId",
                table: "Repayments",
                column: "LoanId",
                principalTable: "Loans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
