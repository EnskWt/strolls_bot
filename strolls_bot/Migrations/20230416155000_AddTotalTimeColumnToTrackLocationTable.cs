using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace strolls_bot.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalTimeColumnToTrackLocationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalTime",
                table: "TrackLocation",
                type: "float",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalTime",
                table: "TrackLocation");
        }
    }
}
