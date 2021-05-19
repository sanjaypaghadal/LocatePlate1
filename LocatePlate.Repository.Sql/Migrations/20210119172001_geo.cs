using Microsoft.EntityFrameworkCore.Migrations;

namespace LocatePlate.Repository.Migrations
{
    public partial class geo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "Zones",
                table: "Restaurants");

            migrationBuilder.AddColumn<string>(
                name: "CityName",
                table: "Restaurants",
                type: "VARCHAR(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CostForTwo",
                table: "Restaurants",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "CountryName",
                table: "Restaurants",
                type: "VARCHAR(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProvinceName",
                table: "Restaurants",
                type: "VARCHAR(100)",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CityName",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "CostForTwo",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "CountryName",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "ProvinceName",
                table: "Restaurants");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Restaurants",
                type: "VARCHAR(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zones",
                table: "Restaurants",
                type: "VARCHAR(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}
