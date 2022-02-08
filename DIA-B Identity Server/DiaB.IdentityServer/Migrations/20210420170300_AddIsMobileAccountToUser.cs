using Microsoft.EntityFrameworkCore.Migrations;

namespace DiaB.IdentityServer.Migrations
{
    public partial class AddIsMobileAccountToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMobileAccount",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMobileAccount",
                table: "AspNetUsers");
        }
    }
}
