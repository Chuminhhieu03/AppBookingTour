using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppBookingTour.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class migration3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Hotels_HotelId",
                schema: "dbo",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomTypes_Hotels_HotelId",
                schema: "dbo",
                table: "RoomTypes");

            migrationBuilder.DropTable(
                name: "Hotels",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TourItineraryDestinations",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Tours_DepartureCityId_TypeId_Status",
                schema: "dbo",
                table: "Tours");

            migrationBuilder.DropIndex(
                name: "IX_RoomTypes_HotelId",
                schema: "dbo",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "dbo",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "BasePriceAdult",
                schema: "dbo",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "BasePriceChildren",
                schema: "dbo",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "CoverImage",
                schema: "dbo",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "HotelId",
                schema: "dbo",
                table: "RoomTypes");

            migrationBuilder.RenameColumn(
                name: "MaxOccupancy",
                schema: "dbo",
                table: "RoomTypes",
                newName: "AccommodationId");

            migrationBuilder.RenameColumn(
                name: "BedCount",
                schema: "dbo",
                table: "RoomTypes",
                newName: "MaxChildren");

            migrationBuilder.RenameColumn(
                name: "HotelId",
                schema: "dbo",
                table: "Reviews",
                newName: "AccommodationId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_HotelId",
                schema: "dbo",
                table: "Reviews",
                newName: "IX_Reviews_AccommodationId");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                schema: "dbo",
                table: "RoomTypes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                schema: "dbo",
                table: "RoomTypes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                schema: "dbo",
                table: "RoomTypes",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)",
                oldPrecision: 12,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "ExtraChildrenPrice",
                schema: "dbo",
                table: "RoomTypes",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)",
                oldPrecision: 12,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "ExtraAdultPrice",
                schema: "dbo",
                table: "RoomTypes",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)",
                oldPrecision: 12,
                oldScale: 2);

            migrationBuilder.AddColumn<string>(
                name: "CoverImageUrl",
                schema: "dbo",
                table: "RoomTypes",
                type: "nvarchar(max)",
                precision: 12,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxAdult",
                schema: "dbo",
                table: "RoomTypes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Accommodations",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StarRating = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Regulation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amenities = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CoverImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accommodations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accommodations_Cities_CityId",
                        column: x => x.CityId,
                        principalSchema: "dbo",
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityType = table.Column<int>(type: "int", nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemParameters",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FeatureCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemParameters", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tours_DepartureCityId_TypeId",
                schema: "dbo",
                table: "Tours",
                columns: new[] { "DepartureCityId", "TypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_RoomTypes_AccommodationId",
                schema: "dbo",
                table: "RoomTypes",
                column: "AccommodationId");

            migrationBuilder.CreateIndex(
                name: "IX_Accommodations_CityId",
                schema: "dbo",
                table: "Accommodations",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Accommodations_AccommodationId",
                schema: "dbo",
                table: "Reviews",
                column: "AccommodationId",
                principalSchema: "dbo",
                principalTable: "Accommodations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomTypes_Accommodations_AccommodationId",
                schema: "dbo",
                table: "RoomTypes",
                column: "AccommodationId",
                principalSchema: "dbo",
                principalTable: "Accommodations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Accommodations_AccommodationId",
                schema: "dbo",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomTypes_Accommodations_AccommodationId",
                schema: "dbo",
                table: "RoomTypes");

            migrationBuilder.DropTable(
                name: "Accommodations",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Images",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SystemParameters",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Tours_DepartureCityId_TypeId",
                schema: "dbo",
                table: "Tours");

            migrationBuilder.DropIndex(
                name: "IX_RoomTypes_AccommodationId",
                schema: "dbo",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "CoverImageUrl",
                schema: "dbo",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "MaxAdult",
                schema: "dbo",
                table: "RoomTypes");

            migrationBuilder.RenameColumn(
                name: "MaxChildren",
                schema: "dbo",
                table: "RoomTypes",
                newName: "BedCount");

            migrationBuilder.RenameColumn(
                name: "AccommodationId",
                schema: "dbo",
                table: "RoomTypes",
                newName: "MaxOccupancy");

            migrationBuilder.RenameColumn(
                name: "AccommodationId",
                schema: "dbo",
                table: "Reviews",
                newName: "HotelId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_AccommodationId",
                schema: "dbo",
                table: "Reviews",
                newName: "IX_Reviews_HotelId");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "dbo",
                table: "Tours",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                schema: "dbo",
                table: "RoomTypes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                schema: "dbo",
                table: "RoomTypes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                schema: "dbo",
                table: "RoomTypes",
                type: "decimal(12,2)",
                precision: 12,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ExtraChildrenPrice",
                schema: "dbo",
                table: "RoomTypes",
                type: "decimal(12,2)",
                precision: 12,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ExtraAdultPrice",
                schema: "dbo",
                table: "RoomTypes",
                type: "decimal(12,2)",
                precision: 12,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<decimal>(
                name: "BasePriceAdult",
                schema: "dbo",
                table: "RoomTypes",
                type: "decimal(12,2)",
                precision: 12,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BasePriceChildren",
                schema: "dbo",
                table: "RoomTypes",
                type: "decimal(12,2)",
                precision: 12,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CoverImage",
                schema: "dbo",
                table: "RoomTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HotelId",
                schema: "dbo",
                table: "RoomTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Hotels",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Amenities = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HotelImageCoverUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Rating = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: true),
                    Regulation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    StarRating = table.Column<int>(type: "int", nullable: false),
                    TypeHotel = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hotels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hotels_Cities_CityId",
                        column: x => x.CityId,
                        principalSchema: "dbo",
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TourItineraryDestinations",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TourItineraryId = table.Column<int>(type: "int", nullable: false),
                    ArrivalTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DepartureTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourItineraryDestinations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TourItineraryDestinations_TourItineraries_TourItineraryId",
                        column: x => x.TourItineraryId,
                        principalSchema: "dbo",
                        principalTable: "TourItineraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tours_DepartureCityId_TypeId_Status",
                schema: "dbo",
                table: "Tours",
                columns: new[] { "DepartureCityId", "TypeId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_RoomTypes_HotelId",
                schema: "dbo",
                table: "RoomTypes",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_CityId_StarRating",
                schema: "dbo",
                table: "Hotels",
                columns: new[] { "CityId", "StarRating" });

            migrationBuilder.CreateIndex(
                name: "IX_TourItineraryDestinations_TourItineraryId",
                schema: "dbo",
                table: "TourItineraryDestinations",
                column: "TourItineraryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Hotels_HotelId",
                schema: "dbo",
                table: "Reviews",
                column: "HotelId",
                principalSchema: "dbo",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomTypes_Hotels_HotelId",
                schema: "dbo",
                table: "RoomTypes",
                column: "HotelId",
                principalSchema: "dbo",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
