using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiaB.IdentityServer.Migrations
{
    public partial class AddOtpRequestCountAndDateToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OtpRequestCount",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "OtpRequestDate",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtpRequestCount",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OtpRequestDate",
                table: "AspNetUsers");
        }
    }
}
