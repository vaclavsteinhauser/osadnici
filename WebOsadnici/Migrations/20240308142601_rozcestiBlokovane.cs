using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebOsadnici.Migrations
{
    /// <inheritdoc />
    public partial class rozcestiBlokovane : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Blokovane",
                table: "Rozcesti");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Blokovane",
                table: "Rozcesti",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
