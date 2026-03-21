using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanApp.Migrations
{
    /// <inheritdoc />
    public partial class LoanApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "LoanApplications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "LoanApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
