using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebOsadnici.Migrations
{
    /// <inheritdoc />
    public partial class upravazavislosti : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurovinaKarty_Smeny_SmenaId",
                table: "SurovinaKarty");

            migrationBuilder.DropForeignKey(
                name: "FK_SurovinaKarty_Smeny_SmenaId1",
                table: "SurovinaKarty");

            migrationBuilder.DropForeignKey(
                name: "FK_SurovinaKarty_StavyHracu_StavHraceId",
                table: "SurovinaKarty");

            migrationBuilder.DropIndex(
                name: "IX_SurovinaKarty_SmenaId",
                table: "SurovinaKarty");

            migrationBuilder.DropIndex(
                name: "IX_SurovinaKarty_SmenaId1",
                table: "SurovinaKarty");

            migrationBuilder.DropIndex(
                name: "IX_SurovinaKarty_StavHraceId",
                table: "SurovinaKarty");

            migrationBuilder.DropColumn(
                name: "SmenaId",
                table: "SurovinaKarty");

            migrationBuilder.DropColumn(
                name: "SmenaId1",
                table: "SurovinaKarty");

            migrationBuilder.DropColumn(
                name: "StavHraceId",
                table: "SurovinaKarty");

            migrationBuilder.CreateTable(
                name: "SmenaSurovinaKarta",
                columns: table => new
                {
                    SmenaId = table.Column<Guid>(type: "char(36)", nullable: false),
                    nabidkaId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmenaSurovinaKarta", x => new { x.SmenaId, x.nabidkaId });
                    table.ForeignKey(
                        name: "FK_SmenaSurovinaKarta_Smeny_SmenaId",
                        column: x => x.SmenaId,
                        principalTable: "Smeny",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmenaSurovinaKarta_SurovinaKarty_nabidkaId",
                        column: x => x.nabidkaId,
                        principalTable: "SurovinaKarty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SmenaSurovinaKarta1",
                columns: table => new
                {
                    Smena1Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    poptavkaId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmenaSurovinaKarta1", x => new { x.Smena1Id, x.poptavkaId });
                    table.ForeignKey(
                        name: "FK_SmenaSurovinaKarta1_Smeny_Smena1Id",
                        column: x => x.Smena1Id,
                        principalTable: "Smeny",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmenaSurovinaKarta1_SurovinaKarty_poptavkaId",
                        column: x => x.poptavkaId,
                        principalTable: "SurovinaKarty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StavHraceSurovinaKarta",
                columns: table => new
                {
                    StavHraceId = table.Column<Guid>(type: "char(36)", nullable: false),
                    SurovinaKartyId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StavHraceSurovinaKarta", x => new { x.StavHraceId, x.SurovinaKartyId });
                    table.ForeignKey(
                        name: "FK_StavHraceSurovinaKarta_StavyHracu_StavHraceId",
                        column: x => x.StavHraceId,
                        principalTable: "StavyHracu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StavHraceSurovinaKarta_SurovinaKarty_SurovinaKartyId",
                        column: x => x.SurovinaKartyId,
                        principalTable: "SurovinaKarty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_SmenaSurovinaKarta_nabidkaId",
                table: "SmenaSurovinaKarta",
                column: "nabidkaId");

            migrationBuilder.CreateIndex(
                name: "IX_SmenaSurovinaKarta1_poptavkaId",
                table: "SmenaSurovinaKarta1",
                column: "poptavkaId");

            migrationBuilder.CreateIndex(
                name: "IX_StavHraceSurovinaKarta_SurovinaKartyId",
                table: "StavHraceSurovinaKarta",
                column: "SurovinaKartyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SmenaSurovinaKarta");

            migrationBuilder.DropTable(
                name: "SmenaSurovinaKarta1");

            migrationBuilder.DropTable(
                name: "StavHraceSurovinaKarta");

            migrationBuilder.AddColumn<Guid>(
                name: "SmenaId",
                table: "SurovinaKarty",
                type: "char(36)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SmenaId1",
                table: "SurovinaKarty",
                type: "char(36)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StavHraceId",
                table: "SurovinaKarty",
                type: "char(36)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SurovinaKarty_SmenaId",
                table: "SurovinaKarty",
                column: "SmenaId");

            migrationBuilder.CreateIndex(
                name: "IX_SurovinaKarty_SmenaId1",
                table: "SurovinaKarty",
                column: "SmenaId1");

            migrationBuilder.CreateIndex(
                name: "IX_SurovinaKarty_StavHraceId",
                table: "SurovinaKarty",
                column: "StavHraceId");

            migrationBuilder.AddForeignKey(
                name: "FK_SurovinaKarty_Smeny_SmenaId",
                table: "SurovinaKarty",
                column: "SmenaId",
                principalTable: "Smeny",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SurovinaKarty_Smeny_SmenaId1",
                table: "SurovinaKarty",
                column: "SmenaId1",
                principalTable: "Smeny",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SurovinaKarty_StavyHracu_StavHraceId",
                table: "SurovinaKarty",
                column: "StavHraceId",
                principalTable: "StavyHracu",
                principalColumn: "Id");
        }
    }
}
