using Microsoft.EntityFrameworkCore.Migrations;

namespace DiaB.IdentityServer.Migrations
{
    public partial class AddGoogleEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GoogleEmail",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoogleEmail",
                table: "AspNetUsers");
        }
    }
}
