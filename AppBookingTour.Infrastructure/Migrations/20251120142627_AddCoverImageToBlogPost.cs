using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppBookingTour.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCoverImageToBlogPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomInventory_RoomTypes_RoomTypeId",
                schema: "dbo",
                table: "RoomInventory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomInventory",
                schema: "dbo",
                table: "RoomInventory");

            migrationBuilder.RenameTable(
                name: "RoomInventory",
                schema: "dbo",
                newName: "RoomInventories",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_RoomInventory_RoomTypeId",
                schema: "dbo",
                table: "RoomInventories",
                newName: "IX_RoomInventories_RoomTypeId");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                schema: "dbo",
                table: "RoomTypes",
                type: "bit",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoverImage",
                schema: "dbo",
                table: "BlogPosts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomInventories",
                schema: "dbo",
                table: "RoomInventories",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ItemDiscounts",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    DiscountId = table.Column<int>(type: "int", nullable: true),
                    ItemType = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemDiscounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemDiscounts_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalSchema: "dbo",
                        principalTable: "Discounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemDiscounts_DiscountId",
                schema: "dbo",
                table: "ItemDiscounts",
                column: "DiscountId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomInventories_RoomTypes_RoomTypeId",
                schema: "dbo",
                table: "RoomInventories",
                column: "RoomTypeId",
                principalSchema: "dbo",
                principalTable: "RoomTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomInventories_RoomTypes_RoomTypeId",
                schema: "dbo",
                table: "RoomInventories");

            migrationBuilder.DropTable(
                name: "ItemDiscounts",
                schema: "dbo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomInventories",
                schema: "dbo",
                table: "RoomInventories");

            migrationBuilder.DropColumn(
                name: "CoverImage",
                schema: "dbo",
                table: "BlogPosts");

            migrationBuilder.RenameTable(
                name: "RoomInventories",
                schema: "dbo",
                newName: "RoomInventory",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_RoomInventories_RoomTypeId",
                schema: "dbo",
                table: "RoomInventory",
                newName: "IX_RoomInventory_RoomTypeId");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                schema: "dbo",
                table: "RoomTypes",
                type: "int",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomInventory",
                schema: "dbo",
                table: "RoomInventory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomInventory_RoomTypes_RoomTypeId",
                schema: "dbo",
                table: "RoomInventory",
                column: "RoomTypeId",
                principalSchema: "dbo",
                principalTable: "RoomTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
