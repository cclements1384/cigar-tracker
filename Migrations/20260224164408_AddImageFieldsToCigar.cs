using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cigartracker.Migrations
{
    /// <inheritdoc />
    public partial class AddImageFieldsToCigar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageFileName",
                table: "Cigars",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ImageUploadedAt",
                table: "Cigars",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Cigars",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFileName",
                table: "Cigars");

            migrationBuilder.DropColumn(
                name: "ImageUploadedAt",
                table: "Cigars");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Cigars");
        }
    }
}
