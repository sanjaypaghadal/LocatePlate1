using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LocatePlate.Repository.Migrations
{
    public partial class diount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Promotions");
           
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Discounts",
                newName: "PromoCode");

            migrationBuilder.AddColumn<string>(
                name: "TermAndCondition",
                table: "Discounts",
                type: "VARCHAR(5000)",
                maxLength: 5000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidFrom",
                table: "Discounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidTo",
                table: "Discounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discounts_Restaurants_RestaurantId",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "TermAndCondition",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "ValidFrom",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "ValidTo",
                table: "Discounts");

            migrationBuilder.RenameColumn(
                name: "PromoCode",
                table: "Discounts",
                newName: "Name");

            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSoftDelete = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedBy = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PromoCode = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true),
                    PromoType = table.Column<int>(type: "int", nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    TermAndCondition = table.Column<string>(type: "VARCHAR(2000)", maxLength: 2000, nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Promotions_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_RestaurantId",
                table: "Promotions",
                column: "RestaurantId");
        }
    }
}
