using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Authentication.Persistance.Migrations
{
    public partial class refresh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "JwtIssuedAt",
                table: "ApplicationUsers",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.AddColumn<string>(
                name: "RegisteredJWT",
                table: "ApplicationUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegisteredRefreshToken",
                table: "ApplicationUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JwtIssuedAt",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "RegisteredJWT",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "RegisteredRefreshToken",
                table: "ApplicationUsers");
        }
    }
}
