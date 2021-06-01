using Microsoft.EntityFrameworkCore.Migrations;

namespace LocatePlate.Repository.Migrations
{
    public partial class add : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MealTypeName",
                table: "Restaurants",
                type: "VARCHAR(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResturantTypeName",
                table: "Restaurants",
                type: "VARCHAR(50)",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MealTypeName",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "ResturantTypeName",
                table: "Restaurants");
        }
    }
}
