using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppBookingTour.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitCICDmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InfantPrice",
                schema: "dbo",
                table: "Bookings");

            migrationBuilder.AddColumn<decimal>(
                name: "Area",
                schema: "dbo",
                table: "RoomTypes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CancelPolicy",
                schema: "dbo",
                table: "RoomTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "CheckinHour",
                schema: "dbo",
                table: "RoomTypes",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "CheckoutHour",
                schema: "dbo",
                table: "RoomTypes",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<decimal>(
                name: "ManagementFee",
                schema: "dbo",
                table: "RoomTypes",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "VAT",
                schema: "dbo",
                table: "RoomTypes",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "View",
                schema: "dbo",
                table: "RoomTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoomInventoryIds",
                schema: "dbo",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BookingRoomDetails",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    RoomInventoryId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BasePriceAdult = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    BasePriceChildren = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    BasePrice = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingRoomDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingRoomDetails_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalSchema: "dbo",
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingRoomDetails_BookingId",
                schema: "dbo",
                table: "BookingRoomDetails",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingRoomDetails_BookingId_Date",
                schema: "dbo",
                table: "BookingRoomDetails",
                columns: new[] { "BookingId", "Date" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingRoomDetails",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "Area",
                schema: "dbo",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "CancelPolicy",
                schema: "dbo",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "CheckinHour",
                schema: "dbo",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "CheckoutHour",
                schema: "dbo",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "ManagementFee",
                schema: "dbo",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "VAT",
                schema: "dbo",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "View",
                schema: "dbo",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "RoomInventoryIds",
                schema: "dbo",
                table: "Bookings");

            migrationBuilder.AddColumn<decimal>(
                name: "InfantPrice",
                schema: "dbo",
                table: "Bookings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
