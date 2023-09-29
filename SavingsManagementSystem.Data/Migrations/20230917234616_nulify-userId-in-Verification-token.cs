using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SavingsManagementSystem.Data.Migrations
{
    public partial class nulifyuserIdinVerificationtoken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VerificationTokens_AspNetUsers_UserId",
                table: "VerificationTokens");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "VerificationTokens",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_VerificationTokens_AspNetUsers_UserId",
                table: "VerificationTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VerificationTokens_AspNetUsers_UserId",
                table: "VerificationTokens");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "VerificationTokens",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VerificationTokens_AspNetUsers_UserId",
                table: "VerificationTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
