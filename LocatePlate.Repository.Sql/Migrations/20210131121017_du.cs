using Microsoft.EntityFrameworkCore.Migrations;

namespace LocatePlate.Repository.Migrations
{
    public partial class du : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CapacityId",
                table: "BookingXMenus",
                newName: "MenuId");

            migrationBuilder.AddColumn<double>(
                name: "MinimumPrice",
                table: "Discounts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "SpecialInstruction",
                table: "BookingXMenus",
                type: "VARCHAR(1000)",
                maxLength: 1000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinimumPrice",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "SpecialInstruction",
                table: "BookingXMenus");

            migrationBuilder.RenameColumn(
                name: "MenuId",
                table: "BookingXMenus",
                newName: "CapacityId");
        }
    }
}
