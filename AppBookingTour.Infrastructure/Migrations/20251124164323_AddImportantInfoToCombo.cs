using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppBookingTour.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddImportantInfoToCombo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImportantInfo",
                schema: "dbo",
                table: "Combos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImportantInfo",
                schema: "dbo",
                table: "Combos");
        }
    }
}
