using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppBookingTour.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Addingfieldtobooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AdultPrice",
                schema: "dbo",
                table: "Bookings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ChildPrice",
                schema: "dbo",
                table: "Bookings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "InfantPrice",
                schema: "dbo",
                table: "Bookings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "NumSingleRooms",
                schema: "dbo",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "SingleRoomPrice",
                schema: "dbo",
                table: "Bookings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdultPrice",
                schema: "dbo",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "ChildPrice",
                schema: "dbo",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "InfantPrice",
                schema: "dbo",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "NumSingleRooms",
                schema: "dbo",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "SingleRoomPrice",
                schema: "dbo",
                table: "Bookings");
        }
    }
}
