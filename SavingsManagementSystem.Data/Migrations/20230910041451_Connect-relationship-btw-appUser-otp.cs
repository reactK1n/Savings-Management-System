using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SavingsManagementSystem.Data.Migrations
{
    public partial class ConnectrelationshipbtwappUserotp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OTPs_Members_MemberId",
                table: "OTPs");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "OTPs",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_OTPs_MemberId",
                table: "OTPs",
                newName: "IX_OTPs_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OTPs_AspNetUsers_UserId",
                table: "OTPs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OTPs_AspNetUsers_UserId",
                table: "OTPs");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "OTPs",
                newName: "MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_OTPs_UserId",
                table: "OTPs",
                newName: "IX_OTPs_MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_OTPs_Members_MemberId",
                table: "OTPs",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
