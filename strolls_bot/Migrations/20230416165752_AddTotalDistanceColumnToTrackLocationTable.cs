using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace strolls_bot.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalDistanceColumnToTrackLocationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TotalDistance",
                table: "TrackLocation",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalDistance",
                table: "TrackLocation");
        }
    }
}
