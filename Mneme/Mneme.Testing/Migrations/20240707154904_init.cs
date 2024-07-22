using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mneme.Testing.Migrations;

/// <inheritdoc />
public partial class init : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		_ = migrationBuilder.CreateTable(
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
				_ = table.PrimaryKey("PK_TestInfos", x => x.Id);
			});

		_ = migrationBuilder.CreateTable(
			name: "TestMultipleChoices",
			columns: table => new
			{
				Id = table.Column<int>(type: "INTEGER", nullable: false)
					.Annotation("Sqlite:Autoincrement", true),
				NoteId = table.Column<int>(type: "INTEGER", nullable: false),
				Question = table.Column<string>(type: "TEXT", nullable: false),
				Importance = table.Column<int>(type: "INTEGER", nullable: false),
				Created = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
				TestInfoId = table.Column<int>(type: "INTEGER", nullable: false)
			},
			constraints: table =>
			{
				_ = table.PrimaryKey("PK_TestMultipleChoices", x => x.Id);
				_ = table.ForeignKey(
					name: "FK_TestMultipleChoices_TestInfos_TestInfoId",
					column: x => x.TestInfoId,
					principalTable: "TestInfos",
					principalColumn: "Id",
					onDelete: ReferentialAction.Cascade);
			});

		_ = migrationBuilder.CreateTable(
			name: "TestShortAnswers",
			columns: table => new
			{
				Id = table.Column<int>(type: "INTEGER", nullable: false)
					.Annotation("Sqlite:Autoincrement", true),
				NoteId = table.Column<int>(type: "INTEGER", nullable: false),
				Question = table.Column<string>(type: "TEXT", nullable: false),
				Answer = table.Column<string>(type: "TEXT", nullable: false),
				Hint = table.Column<string>(type: "TEXT", nullable: true),
				Importance = table.Column<int>(type: "INTEGER", nullable: false),
				Created = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
				TestInfoId = table.Column<int>(type: "INTEGER", nullable: false)
			},
			constraints: table =>
			{
				_ = table.PrimaryKey("PK_TestShortAnswers", x => x.Id);
				_ = table.ForeignKey(
					name: "FK_TestShortAnswers_TestInfos_TestInfoId",
					column: x => x.TestInfoId,
					principalTable: "TestInfos",
					principalColumn: "Id",
					onDelete: ReferentialAction.Cascade);
			});

		_ = migrationBuilder.CreateTable(
			name: "TestMultipleChoice",
			columns: table => new
			{
				Id = table.Column<int>(type: "INTEGER", nullable: false)
					.Annotation("Sqlite:Autoincrement", true),
				Answer = table.Column<string>(type: "TEXT", nullable: false),
				IsCorrect = table.Column<bool>(type: "INTEGER", nullable: false),
				TestId = table.Column<int>(type: "INTEGER", nullable: false),
				TestMultipleChoicesId = table.Column<int>(type: "INTEGER", nullable: true)
			},
			constraints: table =>
			{
				_ = table.PrimaryKey("PK_TestMultipleChoice", x => x.Id);
				_ = table.ForeignKey(
					name: "FK_TestMultipleChoice_TestMultipleChoices_TestId",
					column: x => x.TestId,
					principalTable: "TestMultipleChoices",
					principalColumn: "Id",
					onDelete: ReferentialAction.Cascade);
				_ = table.ForeignKey(
					name: "FK_TestMultipleChoice_TestMultipleChoices_TestMultipleChoicesId",
					column: x => x.TestMultipleChoicesId,
					principalTable: "TestMultipleChoices",
					principalColumn: "Id");
			});

		_ = migrationBuilder.CreateIndex(
			name: "IX_TestMultipleChoice_TestId",
			table: "TestMultipleChoice",
			column: "TestId");

		_ = migrationBuilder.CreateIndex(
			name: "IX_TestMultipleChoice_TestMultipleChoicesId",
			table: "TestMultipleChoice",
			column: "TestMultipleChoicesId");

		_ = migrationBuilder.CreateIndex(
			name: "IX_TestMultipleChoices_TestInfoId",
			table: "TestMultipleChoices",
			column: "TestInfoId");

		_ = migrationBuilder.CreateIndex(
			name: "IX_TestShortAnswers_TestInfoId",
			table: "TestShortAnswers",
			column: "TestInfoId");
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		_ = migrationBuilder.DropTable(
			name: "TestMultipleChoice");

		_ = migrationBuilder.DropTable(
			name: "TestShortAnswers");

		_ = migrationBuilder.DropTable(
			name: "TestMultipleChoices");

		_ = migrationBuilder.DropTable(
			name: "TestInfos");
	}
}
