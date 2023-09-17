using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SavingsManagementSystem.Data.Migrations
{
    public partial class connectVerifcationTokenModeltoappUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VerificationTokens_Members_MemberId",
                table: "VerificationTokens");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "VerificationTokens",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_VerificationTokens_MemberId",
                table: "VerificationTokens",
                newName: "IX_VerificationTokens_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_VerificationTokens_AspNetUsers_UserId",
                table: "VerificationTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VerificationTokens_AspNetUsers_UserId",
                table: "VerificationTokens");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "VerificationTokens",
                newName: "MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_VerificationTokens_UserId",
                table: "VerificationTokens",
                newName: "IX_VerificationTokens_MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_VerificationTokens_Members_MemberId",
                table: "VerificationTokens",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
