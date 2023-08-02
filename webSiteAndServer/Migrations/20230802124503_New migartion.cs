using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webSiteAndServer.Migrations
{
    /// <inheritdoc />
    public partial class Newmigartion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    PlayerWon = table.Column<bool>(type: "bit", nullable: true),
                    GameFinished = table.Column<bool>(type: "bit", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimePlayedSeconds = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
