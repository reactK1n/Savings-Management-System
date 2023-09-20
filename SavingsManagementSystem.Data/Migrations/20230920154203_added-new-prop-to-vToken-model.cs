using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SavingsManagementSystem.Data.Migrations
{
    public partial class addednewproptovTokenmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "VerificationTokens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "VerificationTokens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "VerificationTokens");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "VerificationTokens");
        }
    }
}
