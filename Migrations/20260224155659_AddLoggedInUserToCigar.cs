using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cigartracker.Migrations
{
    /// <inheritdoc />
    public partial class AddLoggedInUserToCigar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LoggedInUser",
                table: "Cigars",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoggedInUser",
                table: "Cigars");
        }
    }
}
