using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace WebOsadnici.Migrations
{
    /// <inheritdoc />
    public partial class nova : Migration
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
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
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
                    stavHry = table.Column<int>(type: "int", nullable: false),
                    hracNaTahu = table.Column<int>(type: "int", nullable: false)
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
                    Nazev = table.Column<string>(type: "longtext", nullable: false),
                    Zisk = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "longtext", nullable: false),
                    Body = table.Column<int>(type: "int", nullable: false)
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
                name: "Aktivity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    HracId = table.Column<string>(type: "varchar(255)", nullable: false),
                    Akce = table.Column<int>(type: "int", nullable: false),
                    CisloAktivity = table.Column<int>(type: "int", nullable: false),
                    Probiha = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    HraId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aktivity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Aktivity_Hraci_HracId",
                        column: x => x.HracId,
                        principalTable: "Hraci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Aktivity_Hry_HraId",
                        column: x => x.HraId,
                        principalTable: "Hry",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Mapky",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    HraId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mapky", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mapky_Hry_HraId",
                        column: x => x.HraId,
                        principalTable: "Hry",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Smeny",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    hracId = table.Column<string>(type: "varchar(255)", nullable: false),
                    HraId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Smeny", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Smeny_Hraci_hracId",
                        column: x => x.hracId,
                        principalTable: "Hraci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Smeny_Hry_HraId",
                        column: x => x.HraId,
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
                    poradi = table.Column<int>(type: "int", nullable: false),
                    nejdelsiCesta = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    nejvetsiVojsko = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    zahranychRytiru = table.Column<int>(type: "int", nullable: false)
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
                name: "SurovinaKarty",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    SurovinaId = table.Column<Guid>(type: "char(36)", nullable: false),
                    StavbaId = table.Column<Guid>(type: "char(36)", nullable: true),
                    Pocet = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurovinaKarty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurovinaKarty_Stavby_StavbaId",
                        column: x => x.StavbaId,
                        principalTable: "Stavby",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SurovinaKarty_Suroviny_SurovinaId",
                        column: x => x.SurovinaId,
                        principalTable: "Suroviny",
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
                    poziceX = table.Column<int>(type: "int", nullable: false),
                    poziceY = table.Column<int>(type: "int", nullable: false),
                    hracId = table.Column<string>(type: "varchar(255)", nullable: true),
                    natoceni = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Policka",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    MapkaId = table.Column<Guid>(type: "char(36)", nullable: true),
                    PoziceX = table.Column<int>(type: "int", nullable: false),
                    PoziceY = table.Column<int>(type: "int", nullable: false),
                    SurovinaId = table.Column<Guid>(type: "char(36)", nullable: true),
                    Cislo = table.Column<int>(type: "int", nullable: false),
                    Blokovane = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Policka", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Policka_Mapky_MapkaId",
                        column: x => x.MapkaId,
                        principalTable: "Mapky",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Policka_Suroviny_SurovinaId",
                        column: x => x.SurovinaId,
                        principalTable: "Suroviny",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Rozcesti",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    MapkaId = table.Column<Guid>(type: "char(36)", nullable: true),
                    PoziceX = table.Column<int>(type: "int", nullable: false),
                    PoziceY = table.Column<int>(type: "int", nullable: false),
                    HracId = table.Column<string>(type: "varchar(255)", nullable: true),
                    StavbaId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rozcesti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rozcesti_Hraci_HracId",
                        column: x => x.HracId,
                        principalTable: "Hraci",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rozcesti_Mapky_MapkaId",
                        column: x => x.MapkaId,
                        principalTable: "Mapky",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rozcesti_Stavby_StavbaId",
                        column: x => x.StavbaId,
                        principalTable: "Stavby",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AkcniKarty",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Nazev = table.Column<string>(type: "longtext", nullable: false),
                    ImageUrl = table.Column<string>(type: "longtext", nullable: false),
                    Discriminator = table.Column<string>(type: "longtext", nullable: false),
                    HraId = table.Column<Guid>(type: "char(36)", nullable: true),
                    StavHraceId = table.Column<Guid>(type: "char(36)", nullable: true),
                    Body = table.Column<int>(type: "int", nullable: true),
                    HraId1 = table.Column<Guid>(type: "char(36)", nullable: true),
                    StavHraceId1 = table.Column<Guid>(type: "char(36)", nullable: true),
                    Pocet = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkcniKarty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkcniKarty_Hry_HraId",
                        column: x => x.HraId,
                        principalTable: "Hry",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkcniKarty_Hry_HraId1",
                        column: x => x.HraId1,
                        principalTable: "Hry",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkcniKarty_StavyHracu_StavHraceId",
                        column: x => x.StavHraceId,
                        principalTable: "StavyHracu",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkcniKarty_StavyHracu_StavHraceId1",
                        column: x => x.StavHraceId1,
                        principalTable: "StavyHracu",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

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

            migrationBuilder.CreateTable(
                name: "CestaRozcesti",
                columns: table => new
                {
                    CestaId = table.Column<Guid>(type: "char(36)", nullable: false),
                    rozcestiId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CestaRozcesti", x => new { x.CestaId, x.rozcestiId });
                    table.ForeignKey(
                        name: "FK_CestaRozcesti_Cesty_CestaId",
                        column: x => x.CestaId,
                        principalTable: "Cesty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CestaRozcesti_Rozcesti_rozcestiId",
                        column: x => x.rozcestiId,
                        principalTable: "Rozcesti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PoleRozcesti",
                columns: table => new
                {
                    PoleId = table.Column<Guid>(type: "char(36)", nullable: false),
                    RozcestiId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoleRozcesti", x => new { x.PoleId, x.RozcestiId });
                    table.ForeignKey(
                        name: "FK_PoleRozcesti_Policka_PoleId",
                        column: x => x.PoleId,
                        principalTable: "Policka",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PoleRozcesti_Rozcesti_RozcestiId",
                        column: x => x.RozcestiId,
                        principalTable: "Rozcesti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AkcniKarty_HraId",
                table: "AkcniKarty",
                column: "HraId");

            migrationBuilder.CreateIndex(
                name: "IX_AkcniKarty_HraId1",
                table: "AkcniKarty",
                column: "HraId1");

            migrationBuilder.CreateIndex(
                name: "IX_AkcniKarty_StavHraceId",
                table: "AkcniKarty",
                column: "StavHraceId");

            migrationBuilder.CreateIndex(
                name: "IX_AkcniKarty_StavHraceId1",
                table: "AkcniKarty",
                column: "StavHraceId1");

            migrationBuilder.CreateIndex(
                name: "IX_Aktivity_HracId",
                table: "Aktivity",
                column: "HracId");

            migrationBuilder.CreateIndex(
                name: "IX_Aktivity_HraId",
                table: "Aktivity",
                column: "HraId");

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
                name: "IX_CestaRozcesti_rozcestiId",
                table: "CestaRozcesti",
                column: "rozcestiId");

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
                name: "IX_MapkaStavba_StavbyId",
                table: "MapkaStavba",
                column: "StavbyId");

            migrationBuilder.CreateIndex(
                name: "IX_MapkaSurovina_SurovinyId",
                table: "MapkaSurovina",
                column: "SurovinyId");

            migrationBuilder.CreateIndex(
                name: "IX_Mapky_HraId",
                table: "Mapky",
                column: "HraId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PoleRozcesti_RozcestiId",
                table: "PoleRozcesti",
                column: "RozcestiId");

            migrationBuilder.CreateIndex(
                name: "IX_Policka_MapkaId",
                table: "Policka",
                column: "MapkaId");

            migrationBuilder.CreateIndex(
                name: "IX_Policka_SurovinaId",
                table: "Policka",
                column: "SurovinaId");

            migrationBuilder.CreateIndex(
                name: "IX_Rozcesti_HracId",
                table: "Rozcesti",
                column: "HracId");

            migrationBuilder.CreateIndex(
                name: "IX_Rozcesti_MapkaId",
                table: "Rozcesti",
                column: "MapkaId");

            migrationBuilder.CreateIndex(
                name: "IX_Rozcesti_StavbaId",
                table: "Rozcesti",
                column: "StavbaId");

            migrationBuilder.CreateIndex(
                name: "IX_SmenaSurovinaKarta_nabidkaId",
                table: "SmenaSurovinaKarta",
                column: "nabidkaId");

            migrationBuilder.CreateIndex(
                name: "IX_SmenaSurovinaKarta1_poptavkaId",
                table: "SmenaSurovinaKarta1",
                column: "poptavkaId");

            migrationBuilder.CreateIndex(
                name: "IX_Smeny_hracId",
                table: "Smeny",
                column: "hracId");

            migrationBuilder.CreateIndex(
                name: "IX_Smeny_HraId",
                table: "Smeny",
                column: "HraId");

            migrationBuilder.CreateIndex(
                name: "IX_StavHraceSurovinaKarta_SurovinaKartyId",
                table: "StavHraceSurovinaKarta",
                column: "SurovinaKartyId");

            migrationBuilder.CreateIndex(
                name: "IX_StavyHracu_hracId",
                table: "StavyHracu",
                column: "hracId");

            migrationBuilder.CreateIndex(
                name: "IX_StavyHracu_hraId",
                table: "StavyHracu",
                column: "hraId");

            migrationBuilder.CreateIndex(
                name: "IX_SurovinaKarty_StavbaId",
                table: "SurovinaKarty",
                column: "StavbaId");

            migrationBuilder.CreateIndex(
                name: "IX_SurovinaKarty_SurovinaId",
                table: "SurovinaKarty",
                column: "SurovinaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AkcniKarty");

            migrationBuilder.DropTable(
                name: "Aktivity");

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
                name: "CestaRozcesti");

            migrationBuilder.DropTable(
                name: "MapkaStavba");

            migrationBuilder.DropTable(
                name: "MapkaSurovina");

            migrationBuilder.DropTable(
                name: "PoleRozcesti");

            migrationBuilder.DropTable(
                name: "SmenaSurovinaKarta");

            migrationBuilder.DropTable(
                name: "SmenaSurovinaKarta1");

            migrationBuilder.DropTable(
                name: "StavHraceSurovinaKarta");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Cesty");

            migrationBuilder.DropTable(
                name: "Policka");

            migrationBuilder.DropTable(
                name: "Rozcesti");

            migrationBuilder.DropTable(
                name: "Smeny");

            migrationBuilder.DropTable(
                name: "StavyHracu");

            migrationBuilder.DropTable(
                name: "SurovinaKarty");

            migrationBuilder.DropTable(
                name: "Mapky");

            migrationBuilder.DropTable(
                name: "Hraci");

            migrationBuilder.DropTable(
                name: "Stavby");

            migrationBuilder.DropTable(
                name: "Suroviny");

            migrationBuilder.DropTable(
                name: "Hry");
        }
    }
}
