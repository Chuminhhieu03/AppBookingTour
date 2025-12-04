using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppBookingTour.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removedestinationandbusinessentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Promotions_Business_BusinessId",
                schema: "dbo",
                table: "Promotions");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Destinations_DestinationId",
                schema: "dbo",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_TourItineraryDestinations_Destinations_DestinationId",
                schema: "dbo",
                table: "TourItineraryDestinations");

            migrationBuilder.DropForeignKey(
                name: "FK_Tours_Business_BusinessId",
                schema: "dbo",
                table: "Tours");

            migrationBuilder.DropTable(
                name: "Business",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Destinations",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Tours_BusinessId",
                schema: "dbo",
                table: "Tours");

            migrationBuilder.DropIndex(
                name: "IX_TourItineraryDestinations_DestinationId",
                schema: "dbo",
                table: "TourItineraryDestinations");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_DestinationId",
                schema: "dbo",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Promotions_BusinessId",
                schema: "dbo",
                table: "Promotions");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                schema: "dbo",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "DestinationId",
                schema: "dbo",
                table: "TourItineraryDestinations");

            migrationBuilder.DropColumn(
                name: "DestinationId",
                schema: "dbo",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                schema: "dbo",
                table: "Promotions");

            migrationBuilder.AddColumn<string>(
                name: "Image_Main",
                schema: "dbo",
                table: "Tours",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoverImage",
                schema: "dbo",
                table: "RoomTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExtraAdultPrice",
                schema: "dbo",
                table: "RoomTypes",
                type: "decimal(12,2)",
                precision: 12,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ExtraChildrenPrice",
                schema: "dbo",
                table: "RoomTypes",
                type: "decimal(12,2)",
                precision: 12,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                schema: "dbo",
                table: "RoomTypes",
                type: "decimal(12,2)",
                precision: 12,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                schema: "dbo",
                table: "RoomTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "MinimumDiscount",
                schema: "dbo",
                table: "Promotions",
                type: "decimal(12,2)",
                precision: 12,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "dbo",
                table: "Combos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ComboImageCover",
                schema: "dbo",
                table: "Combos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image_Main",
                schema: "dbo",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "CoverImage",
                schema: "dbo",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "ExtraAdultPrice",
                schema: "dbo",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "ExtraChildrenPrice",
                schema: "dbo",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "Price",
                schema: "dbo",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "Quantity",
                schema: "dbo",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "MinimumDiscount",
                schema: "dbo",
                table: "Promotions");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "dbo",
                table: "Combos");

            migrationBuilder.DropColumn(
                name: "ComboImageCover",
                schema: "dbo",
                table: "Combos");

            migrationBuilder.AddColumn<int>(
                name: "BusinessId",
                schema: "dbo",
                table: "Tours",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DestinationId",
                schema: "dbo",
                table: "TourItineraryDestinations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DestinationId",
                schema: "dbo",
                table: "Reviews",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BusinessId",
                schema: "dbo",
                table: "Promotions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Business",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BusinessName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LicenseNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TaxCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Business", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Business_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Destinations",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageGallery = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destinations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Destinations_Cities_CityId",
                        column: x => x.CityId,
                        principalSchema: "dbo",
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tours_BusinessId",
                schema: "dbo",
                table: "Tours",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_TourItineraryDestinations_DestinationId",
                schema: "dbo",
                table: "TourItineraryDestinations",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_DestinationId",
                schema: "dbo",
                table: "Reviews",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_BusinessId",
                schema: "dbo",
                table: "Promotions",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_Business_UserId",
                schema: "dbo",
                table: "Business",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Destinations_CityId",
                schema: "dbo",
                table: "Destinations",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Promotions_Business_BusinessId",
                schema: "dbo",
                table: "Promotions",
                column: "BusinessId",
                principalSchema: "dbo",
                principalTable: "Business",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Destinations_DestinationId",
                schema: "dbo",
                table: "Reviews",
                column: "DestinationId",
                principalSchema: "dbo",
                principalTable: "Destinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TourItineraryDestinations_Destinations_DestinationId",
                schema: "dbo",
                table: "TourItineraryDestinations",
                column: "DestinationId",
                principalSchema: "dbo",
                principalTable: "Destinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tours_Business_BusinessId",
                schema: "dbo",
                table: "Tours",
                column: "BusinessId",
                principalSchema: "dbo",
                principalTable: "Business",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
