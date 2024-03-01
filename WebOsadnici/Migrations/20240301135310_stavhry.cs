using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebOsadnici.Migrations
{
    /// <inheritdoc />
    public partial class stavhry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "stavHry",
                table: "Hry",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "stavHry",
                table: "Hry");
        }
    }
}
