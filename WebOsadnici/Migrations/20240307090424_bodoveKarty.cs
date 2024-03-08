using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebOsadnici.Migrations
{
    /// <inheritdoc />
    public partial class bodoveKarty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Body",
                table: "AkcniKarty",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AkcniKarty",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: "StavHraceId1",
                table: "AkcniKarty",
                type: "char(36)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AkcniKarty_StavHraceId1",
                table: "AkcniKarty",
                column: "StavHraceId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AkcniKarty_StavyHracu_StavHraceId1",
                table: "AkcniKarty",
                column: "StavHraceId1",
                principalTable: "StavyHracu",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkcniKarty_StavyHracu_StavHraceId1",
                table: "AkcniKarty");

            migrationBuilder.DropIndex(
                name: "IX_AkcniKarty_StavHraceId1",
                table: "AkcniKarty");

            migrationBuilder.DropColumn(
                name: "Body",
                table: "AkcniKarty");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AkcniKarty");

            migrationBuilder.DropColumn(
                name: "StavHraceId1",
                table: "AkcniKarty");
        }
    }
}
