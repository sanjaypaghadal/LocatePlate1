using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LocatePlate.Repository.Migrations
{
    public partial class booing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Bookings");

            migrationBuilder.AddColumn<string>(
                name: "TermAndCondition",
                table: "Promotions",
                type: "VARCHAR(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartySize",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TermAndCondition",
                table: "Promotions");

            migrationBuilder.DropColumn(
                name: "PartySize",
                table: "Bookings");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
