using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppBookingTour.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fixcol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageGallery",
                schema: "dbo",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "Image_Main",
                schema: "dbo",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "Order",
                schema: "dbo",
                table: "TourItineraries");

            migrationBuilder.DropColumn(
                name: "ComboImageCover",
                schema: "dbo",
                table: "Combos");

            migrationBuilder.RenameColumn(
                name: "Itinerary",
                schema: "dbo",
                table: "Tours",
                newName: "ImageMainUrl");

            migrationBuilder.RenameColumn(
                name: "ImageGallery",
                schema: "dbo",
                table: "Hotels",
                newName: "HotelImageCoverUrl");

            migrationBuilder.RenameColumn(
                name: "HotelImages",
                schema: "dbo",
                table: "Combos",
                newName: "ComboImageCoverUrl");

            migrationBuilder.AlterColumn<int>(
                name: "DayNumber",
                schema: "dbo",
                table: "TourItineraries",
                type: "int",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageMainUrl",
                schema: "dbo",
                table: "Tours",
                newName: "Itinerary");

            migrationBuilder.RenameColumn(
                name: "HotelImageCoverUrl",
                schema: "dbo",
                table: "Hotels",
                newName: "ImageGallery");

            migrationBuilder.RenameColumn(
                name: "ComboImageCoverUrl",
                schema: "dbo",
                table: "Combos",
                newName: "HotelImages");

            migrationBuilder.AddColumn<string>(
                name: "ImageGallery",
                schema: "dbo",
                table: "Tours",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image_Main",
                schema: "dbo",
                table: "Tours",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DayNumber",
                schema: "dbo",
                table: "TourItineraries",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                schema: "dbo",
                table: "TourItineraries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ComboImageCover",
                schema: "dbo",
                table: "Combos",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
