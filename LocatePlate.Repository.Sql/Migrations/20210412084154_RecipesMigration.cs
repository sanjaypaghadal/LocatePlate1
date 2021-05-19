using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LocatePlate.Repository.Migrations
{
    public partial class RecipesMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Recipies",
                table: "Menus");

            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "RestaurantStoreProcedure",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Recipes",
                table: "Menus",
                type: "VARCHAR",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillId",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "RestaurantStoreProcedure");

            migrationBuilder.DropColumn(
                name: "Recipes",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "BillId",
                table: "Bookings");

            migrationBuilder.AddColumn<string>(
                name: "Recipies",
                table: "Menus",
                type: "VARCHAR(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
