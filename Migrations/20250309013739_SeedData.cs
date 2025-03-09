using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BasicGameBackend.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Maps",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    AuthorTime = table.Column<double>(type: "REAL", nullable: false),
                    SPlusTime = table.Column<double>(type: "REAL", nullable: false),
                    STime = table.Column<double>(type: "REAL", nullable: false),
                    ATime = table.Column<double>(type: "REAL", nullable: false),
                    BTime = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    AccountCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastLoggedIn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Plays",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    PlayerId = table.Column<string>(type: "TEXT", nullable: false),
                    MapId = table.Column<string>(type: "TEXT", nullable: false),
                    PlayLength = table.Column<double>(type: "REAL", nullable: false),
                    TimeSubmitted = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plays_Maps_MapId",
                        column: x => x.MapId,
                        principalTable: "Maps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Plays_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Maps",
                columns: new[] { "Id", "ATime", "AuthorTime", "BTime", "SPlusTime", "STime" },
                values: new object[,]
                {
                    { "movement_v2", 30.0, 24.899999999999999, 38.0, 25.5, 27.0 },
                    { "pk_pylons", 40.0, 31.75, 50.0, 32.100000000000001, 33.5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Plays_MapId",
                table: "Plays",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_Plays_PlayerId",
                table: "Plays",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Plays_PlayLength_PlayerId",
                table: "Plays",
                columns: new[] { "PlayLength", "PlayerId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Plays");

            migrationBuilder.DropTable(
                name: "Maps");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
