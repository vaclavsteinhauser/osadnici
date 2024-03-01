using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace WebOsadnici.Migrations
{
    /// <inheritdoc />
    public partial class inicializace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Hraci",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    UserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: true),
                    SecurityStamp = table.Column<string>(type: "longtext", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hraci", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Hry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    hracNaTahu = table.Column<int>(type: "int", nullable: false),
                    stavHry = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hry", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Stavby",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    ImageUrl = table.Column<string>(type: "longtext", nullable: false),
                    Nazev = table.Column<string>(type: "longtext", nullable: false),
                    zisk = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stavby", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Suroviny",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Nazev = table.Column<string>(type: "longtext", nullable: false),
                    ImageUrl = table.Column<string>(type: "longtext", nullable: false),
                    BackColor = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suroviny", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(type: "varchar(255)", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_Hraci_UserId",
                        column: x => x.UserId,
                        principalTable: "Hraci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "longtext", nullable: true),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_Hraci_UserId",
                        column: x => x.UserId,
                        principalTable: "Hraci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    RoleId = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Hraci_UserId",
                        column: x => x.UserId,
                        principalTable: "Hraci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    LoginProvider = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_Hraci_UserId",
                        column: x => x.UserId,
                        principalTable: "Hraci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HraHrac",
                columns: table => new
                {
                    HraId = table.Column<Guid>(type: "char(36)", nullable: false),
                    hraciId = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HraHrac", x => new { x.HraId, x.hraciId });
                    table.ForeignKey(
                        name: "FK_HraHrac_Hraci_hraciId",
                        column: x => x.hraciId,
                        principalTable: "Hraci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HraHrac_Hry_HraId",
                        column: x => x.HraId,
                        principalTable: "Hry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Mapky",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    hraId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mapky", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mapky_Hry_hraId",
                        column: x => x.hraId,
                        principalTable: "Hry",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StavyHracu",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    hraId = table.Column<Guid>(type: "char(36)", nullable: false),
                    hracId = table.Column<string>(type: "varchar(255)", nullable: false),
                    barva = table.Column<string>(type: "longtext", nullable: false),
                    poradi = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StavyHracu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StavyHracu_Hraci_hracId",
                        column: x => x.hracId,
                        principalTable: "Hraci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StavyHracu_Hry_hraId",
                        column: x => x.hraId,
                        principalTable: "Hry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Cesty",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    MapkaId = table.Column<Guid>(type: "char(36)", nullable: true),
                    hracId = table.Column<string>(type: "varchar(255)", nullable: true),
                    natoceni = table.Column<int>(type: "int", nullable: false),
                    poziceX = table.Column<int>(type: "int", nullable: false),
                    poziceY = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cesty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cesty_Hraci_hracId",
                        column: x => x.hracId,
                        principalTable: "Hraci",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Cesty_Mapky_MapkaId",
                        column: x => x.MapkaId,
                        principalTable: "Mapky",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Policka",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    blokovane = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    cislo = table.Column<int>(type: "int", nullable: false),
                    mapkaId = table.Column<Guid>(type: "char(36)", nullable: true),
                    poziceX = table.Column<int>(type: "int", nullable: false),
                    poziceY = table.Column<int>(type: "int", nullable: false),
                    surovinaId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Policka", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Policka_Mapky_mapkaId",
                        column: x => x.mapkaId,
                        principalTable: "Mapky",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Policka_Suroviny_surovinaId",
                        column: x => x.surovinaId,
                        principalTable: "Suroviny",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AkcniKarty",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Nazev = table.Column<string>(type: "longtext", nullable: false),
                    StavHraceId = table.Column<Guid>(type: "char(36)", nullable: true),
                    pocet = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkcniKarty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkcniKarty_StavyHracu_StavHraceId",
                        column: x => x.StavHraceId,
                        principalTable: "StavyHracu",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SurovinaKarty",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    StavHraceId = table.Column<Guid>(type: "char(36)", nullable: true),
                    pocet = table.Column<int>(type: "int", nullable: false),
                    surovinaId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurovinaKarty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurovinaKarty_StavyHracu_StavHraceId",
                        column: x => x.StavHraceId,
                        principalTable: "StavyHracu",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SurovinaKarty_Suroviny_surovinaId",
                        column: x => x.surovinaId,
                        principalTable: "Suroviny",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Rozcesti",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    CestaId = table.Column<Guid>(type: "char(36)", nullable: true),
                    MapkaId = table.Column<Guid>(type: "char(36)", nullable: true),
                    PoleId = table.Column<Guid>(type: "char(36)", nullable: true),
                    blokovane = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    hracId = table.Column<string>(type: "varchar(255)", nullable: true),
                    poziceX = table.Column<int>(type: "int", nullable: false),
                    poziceY = table.Column<int>(type: "int", nullable: false),
                    stavbaId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rozcesti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rozcesti_Cesty_CestaId",
                        column: x => x.CestaId,
                        principalTable: "Cesty",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rozcesti_Hraci_hracId",
                        column: x => x.hracId,
                        principalTable: "Hraci",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rozcesti_Mapky_MapkaId",
                        column: x => x.MapkaId,
                        principalTable: "Mapky",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rozcesti_Policka_PoleId",
                        column: x => x.PoleId,
                        principalTable: "Policka",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rozcesti_Stavby_stavbaId",
                        column: x => x.stavbaId,
                        principalTable: "Stavby",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AkcniKarty_StavHraceId",
                table: "AkcniKarty",
                column: "StavHraceId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Cesty_hracId",
                table: "Cesty",
                column: "hracId");

            migrationBuilder.CreateIndex(
                name: "IX_Cesty_MapkaId",
                table: "Cesty",
                column: "MapkaId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Hraci",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Hraci",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HraHrac_hraciId",
                table: "HraHrac",
                column: "hraciId");

            migrationBuilder.CreateIndex(
                name: "IX_Mapky_hraId",
                table: "Mapky",
                column: "hraId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Policka_mapkaId",
                table: "Policka",
                column: "mapkaId");

            migrationBuilder.CreateIndex(
                name: "IX_Policka_surovinaId",
                table: "Policka",
                column: "surovinaId");

            migrationBuilder.CreateIndex(
                name: "IX_Rozcesti_CestaId",
                table: "Rozcesti",
                column: "CestaId");

            migrationBuilder.CreateIndex(
                name: "IX_Rozcesti_hracId",
                table: "Rozcesti",
                column: "hracId");

            migrationBuilder.CreateIndex(
                name: "IX_Rozcesti_MapkaId",
                table: "Rozcesti",
                column: "MapkaId");

            migrationBuilder.CreateIndex(
                name: "IX_Rozcesti_PoleId",
                table: "Rozcesti",
                column: "PoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Rozcesti_stavbaId",
                table: "Rozcesti",
                column: "stavbaId");

            migrationBuilder.CreateIndex(
                name: "IX_StavyHracu_hracId",
                table: "StavyHracu",
                column: "hracId");

            migrationBuilder.CreateIndex(
                name: "IX_StavyHracu_hraId",
                table: "StavyHracu",
                column: "hraId");

            migrationBuilder.CreateIndex(
                name: "IX_SurovinaKarty_StavHraceId",
                table: "SurovinaKarty",
                column: "StavHraceId");

            migrationBuilder.CreateIndex(
                name: "IX_SurovinaKarty_surovinaId",
                table: "SurovinaKarty",
                column: "surovinaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AkcniKarty");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "HraHrac");

            migrationBuilder.DropTable(
                name: "Rozcesti");

            migrationBuilder.DropTable(
                name: "SurovinaKarty");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Cesty");

            migrationBuilder.DropTable(
                name: "Policka");

            migrationBuilder.DropTable(
                name: "Stavby");

            migrationBuilder.DropTable(
                name: "StavyHracu");

            migrationBuilder.DropTable(
                name: "Mapky");

            migrationBuilder.DropTable(
                name: "Suroviny");

            migrationBuilder.DropTable(
                name: "Hraci");

            migrationBuilder.DropTable(
                name: "Hry");
        }
    }
}
