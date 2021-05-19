using Microsoft.EntityFrameworkCore.Migrations;

namespace LocatePlate.Repository.Migrations
{
    public partial class nw : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FoodNatureName",
                table: "Menus",
                type: "VARCHAR(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RestaurantName",
                table: "Menus",
                type: "VARCHAR(200)",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FoodNatureName",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "RestaurantName",
                table: "Menus");
        }
    }
}
