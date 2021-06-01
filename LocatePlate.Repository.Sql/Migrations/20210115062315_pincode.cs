using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LocatePlate.Repository.Migrations
{
    public partial class pincode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "LocationId",
                table: "Restaurants",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier ");

            migrationBuilder.AddColumn<string>(
                name: "PinCode",
                table: "Restaurants",
                type: "VARCHAR(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PinCode",
                table: "Restaurants");

            migrationBuilder.AlterColumn<Guid>(
                name: "LocationId",
                table: "Restaurants",
                type: "uniqueidentifier ",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }
    }
}
