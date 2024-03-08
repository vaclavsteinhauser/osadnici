using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebOsadnici.Migrations
{
    /// <inheritdoc />
    public partial class SurovinyAStavby : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MapkaStavba",
                columns: table => new
                {
                    MapkaId = table.Column<Guid>(type: "char(36)", nullable: false),
                    StavbyId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapkaStavba", x => new { x.MapkaId, x.StavbyId });
                    table.ForeignKey(
                        name: "FK_MapkaStavba_Mapky_MapkaId",
                        column: x => x.MapkaId,
                        principalTable: "Mapky",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MapkaStavba_Stavby_StavbyId",
                        column: x => x.StavbyId,
                        principalTable: "Stavby",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MapkaSurovina",
                columns: table => new
                {
                    MapkaId = table.Column<Guid>(type: "char(36)", nullable: false),
                    SurovinyId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapkaSurovina", x => new { x.MapkaId, x.SurovinyId });
                    table.ForeignKey(
                        name: "FK_MapkaSurovina_Mapky_MapkaId",
                        column: x => x.MapkaId,
                        principalTable: "Mapky",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MapkaSurovina_Suroviny_SurovinyId",
                        column: x => x.SurovinyId,
                        principalTable: "Suroviny",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_MapkaStavba_StavbyId",
                table: "MapkaStavba",
                column: "StavbyId");

            migrationBuilder.CreateIndex(
                name: "IX_MapkaSurovina_SurovinyId",
                table: "MapkaSurovina",
                column: "SurovinyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MapkaStavba");

            migrationBuilder.DropTable(
                name: "MapkaSurovina");
        }
    }
}
