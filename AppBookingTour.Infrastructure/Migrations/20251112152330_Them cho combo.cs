using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppBookingTour.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Themchocombo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DestinationCityId",
                schema: "dbo",
                table: "Tours",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "SingleRoomSurcharge",
                schema: "dbo",
                table: "TourDepartures",
                type: "decimal(12,2)",
                precision: 12,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MaximumDiscount",
                schema: "dbo",
                table: "Discounts",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MinimumOrderAmount",
                schema: "dbo",
                table: "Discounts",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SingleRoomSupplement",
                schema: "dbo",
                table: "ComboSchedules",
                type: "decimal(12,2)",
                precision: 12,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "DiscountUsage",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscountId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UsedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountUsage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscountUsage_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiscountUsage_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalSchema: "dbo",
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiscountUsage_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalSchema: "dbo",
                        principalTable: "Discounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tours_DestinationCityId",
                schema: "dbo",
                table: "Tours",
                column: "DestinationCityId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountUsage_BookingId",
                schema: "dbo",
                table: "DiscountUsage",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountUsage_DiscountId",
                schema: "dbo",
                table: "DiscountUsage",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountUsage_UserId",
                schema: "dbo",
                table: "DiscountUsage",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tours_Cities_DestinationCityId",
                schema: "dbo",
                table: "Tours",
                column: "DestinationCityId",
                principalSchema: "dbo",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tours_Cities_DestinationCityId",
                schema: "dbo",
                table: "Tours");

            migrationBuilder.DropTable(
                name: "DiscountUsage",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Tours_DestinationCityId",
                schema: "dbo",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "DestinationCityId",
                schema: "dbo",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "SingleRoomSurcharge",
                schema: "dbo",
                table: "TourDepartures");

            migrationBuilder.DropColumn(
                name: "MaximumDiscount",
                schema: "dbo",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "MinimumOrderAmount",
                schema: "dbo",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "SingleRoomSupplement",
                schema: "dbo",
                table: "ComboSchedules");
        }
    }
}
