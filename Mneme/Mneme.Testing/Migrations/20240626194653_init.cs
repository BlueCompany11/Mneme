using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mneme.Testing.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Occurrence = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestClozeDeletions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NoteId = table.Column<string>(type: "TEXT", nullable: true),
                    Text = table.Column<string>(type: "TEXT", nullable: true),
                    Importance = table.Column<int>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TestInfoId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestClozeDeletions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestClozeDeletions_TestInfos_TestInfoId",
                        column: x => x.TestInfoId,
                        principalTable: "TestInfos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TestMultipleChoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NoteId = table.Column<string>(type: "TEXT", nullable: true),
                    Question = table.Column<string>(type: "TEXT", nullable: true),
                    Importance = table.Column<int>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TestInfoId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestMultipleChoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestMultipleChoices_TestInfos_TestInfoId",
                        column: x => x.TestInfoId,
                        principalTable: "TestInfos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TestShortAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NoteId = table.Column<string>(type: "TEXT", nullable: true),
                    Question = table.Column<string>(type: "TEXT", nullable: true),
                    Answer = table.Column<string>(type: "TEXT", nullable: true),
                    Hint = table.Column<string>(type: "TEXT", nullable: true),
                    Importance = table.Column<int>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TestInfoId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestShortAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestShortAnswers_TestInfos_TestInfoId",
                        column: x => x.TestInfoId,
                        principalTable: "TestInfos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClozeDeletionDataStructure",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Start = table.Column<int>(type: "INTEGER", nullable: false),
                    End = table.Column<int>(type: "INTEGER", nullable: false),
                    TestClozeDeletionId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClozeDeletionDataStructure", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClozeDeletionDataStructure_TestClozeDeletions_TestClozeDeletionId",
                        column: x => x.TestClozeDeletionId,
                        principalTable: "TestClozeDeletions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TestMultipleChoice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Answer = table.Column<string>(type: "TEXT", nullable: true),
                    IsCorrect = table.Column<bool>(type: "INTEGER", nullable: false),
                    TestMultipleChoicesId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestMultipleChoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestMultipleChoice_TestMultipleChoices_TestMultipleChoicesId",
                        column: x => x.TestMultipleChoicesId,
                        principalTable: "TestMultipleChoices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClozeDeletionDataStructure_TestClozeDeletionId",
                table: "ClozeDeletionDataStructure",
                column: "TestClozeDeletionId");

            migrationBuilder.CreateIndex(
                name: "IX_TestClozeDeletions_TestInfoId",
                table: "TestClozeDeletions",
                column: "TestInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_TestMultipleChoice_TestMultipleChoicesId",
                table: "TestMultipleChoice",
                column: "TestMultipleChoicesId");

            migrationBuilder.CreateIndex(
                name: "IX_TestMultipleChoices_TestInfoId",
                table: "TestMultipleChoices",
                column: "TestInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_TestShortAnswers_TestInfoId",
                table: "TestShortAnswers",
                column: "TestInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClozeDeletionDataStructure");

            migrationBuilder.DropTable(
                name: "TestMultipleChoice");

            migrationBuilder.DropTable(
                name: "TestShortAnswers");

            migrationBuilder.DropTable(
                name: "TestClozeDeletions");

            migrationBuilder.DropTable(
                name: "TestMultipleChoices");

            migrationBuilder.DropTable(
                name: "TestInfos");
        }
    }
}
