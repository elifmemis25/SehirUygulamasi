using Microsoft.EntityFrameworkCore.Migrations;

namespace SehirUygulamasi.Data.Migrations
{
    public partial class identityExtended : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CetUserId",
                table: "GezilecekSehirlers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GezilecekSehirlers_CetUserId",
                table: "GezilecekSehirlers",
                column: "CetUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GezilecekSehirlers_AspNetUsers_CetUserId",
                table: "GezilecekSehirlers",
                column: "CetUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GezilecekSehirlers_AspNetUsers_CetUserId",
                table: "GezilecekSehirlers");

            migrationBuilder.DropIndex(
                name: "IX_GezilecekSehirlers_CetUserId",
                table: "GezilecekSehirlers");

            migrationBuilder.DropColumn(
                name: "CetUserId",
                table: "GezilecekSehirlers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "AspNetUsers");
        }
    }
}
