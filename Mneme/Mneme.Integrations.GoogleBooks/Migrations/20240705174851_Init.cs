using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mneme.Integrations.GoogleBooks.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GoogleBooksSources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IntegrationId = table.Column<string>(type: "TEXT", nullable: true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoogleBooksSources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoogleBooksNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NoteType = table.Column<string>(type: "TEXT", nullable: false),
                    SourceId = table.Column<int>(type: "INTEGER", nullable: false),
                    IntegrationId = table.Column<string>(type: "TEXT", nullable: true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Path = table.Column<string>(type: "TEXT", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoogleBooksNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoogleBooksNotes_GoogleBooksSources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "GoogleBooksSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GoogleBooksNotes_IntegrationId",
                table: "GoogleBooksNotes",
                column: "IntegrationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GoogleBooksNotes_SourceId",
                table: "GoogleBooksNotes",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_GoogleBooksSources_IntegrationId",
                table: "GoogleBooksSources",
                column: "IntegrationId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoogleBooksNotes");

            migrationBuilder.DropTable(
                name: "GoogleBooksSources");
        }
    }
}
