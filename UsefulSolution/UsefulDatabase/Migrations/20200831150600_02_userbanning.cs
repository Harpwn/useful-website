using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UsefulDatabase.Migrations
{
    public partial class _02_userbanning : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BannedReason",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BannedUntilDate",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BannedReason",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BannedUntilDate",
                table: "AspNetUsers");
        }
    }
}
