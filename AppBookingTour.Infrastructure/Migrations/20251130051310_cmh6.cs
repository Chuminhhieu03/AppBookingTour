using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppBookingTour.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class cmh6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Excludes",
                schema: "dbo",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "Excludes",
                schema: "dbo",
                table: "Combos");

            migrationBuilder.DropColumn(
                name: "Includes",
                schema: "dbo",
                table: "Combos");

            migrationBuilder.DropColumn(
                name: "TermsConditions",
                schema: "dbo",
                table: "Combos");

            migrationBuilder.RenameColumn(
                name: "TermsConditions",
                schema: "dbo",
                table: "Tours",
                newName: "ImportantInfo");

            migrationBuilder.RenameColumn(
                name: "Includes",
                schema: "dbo",
                table: "Tours",
                newName: "AdditionalInfo");

            migrationBuilder.AddColumn<decimal>(
                name: "BasePrice",
                schema: "dbo",
                table: "RoomInventories",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Coordinates",
                schema: "dbo",
                table: "Accommodations",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasePrice",
                schema: "dbo",
                table: "RoomInventories");

            migrationBuilder.DropColumn(
                name: "Coordinates",
                schema: "dbo",
                table: "Accommodations");

            migrationBuilder.RenameColumn(
                name: "ImportantInfo",
                schema: "dbo",
                table: "Tours",
                newName: "TermsConditions");

            migrationBuilder.RenameColumn(
                name: "AdditionalInfo",
                schema: "dbo",
                table: "Tours",
                newName: "Includes");

            migrationBuilder.AddColumn<string>(
                name: "Excludes",
                schema: "dbo",
                table: "Tours",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Excludes",
                schema: "dbo",
                table: "Combos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Includes",
                schema: "dbo",
                table: "Combos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TermsConditions",
                schema: "dbo",
                table: "Combos",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
