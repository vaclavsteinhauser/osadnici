using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebOsadnici.Migrations
{
    /// <inheritdoc />
    public partial class poradi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "poradi",
                table: "StavyHracu",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "poradi",
                table: "StavyHracu");
        }
    }
}
