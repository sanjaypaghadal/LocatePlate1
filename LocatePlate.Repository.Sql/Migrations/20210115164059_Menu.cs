using Microsoft.EntityFrameworkCore.Migrations;

namespace LocatePlate.Repository.Migrations
{
    public partial class Menu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FoodType",
                table: "Menus",
                newName: "FoodNature");

            migrationBuilder.AlterColumn<string>(
                name: "Images",
                table: "Menus",
                type: "VARCHAR(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Calories",
                table: "Menus",
                type: "VARCHAR(200)",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Calories",
                table: "Menus");

            migrationBuilder.RenameColumn(
                name: "FoodNature",
                table: "Menus",
                newName: "FoodType");

            migrationBuilder.AlterColumn<string>(
                name: "Images",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(500)",
                oldMaxLength: 500,
                oldNullable: true);
        }
    }
}
