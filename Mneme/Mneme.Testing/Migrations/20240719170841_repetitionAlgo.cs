using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mneme.Testing.Migrations;

/// <inheritdoc />
public partial class repetitionAlgo : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		_ = migrationBuilder.DropForeignKey(
			name: "FK_TestMultipleChoices_TestInfos_TestInfoId",
			table: "TestMultipleChoices");

		_ = migrationBuilder.DropForeignKey(
			name: "FK_TestShortAnswers_TestInfos_TestInfoId",
			table: "TestShortAnswers");

		_ = migrationBuilder.DropTable(
			name: "TestInfos");

		_ = migrationBuilder.DropIndex(
			name: "IX_TestShortAnswers_TestInfoId",
			table: "TestShortAnswers");

		_ = migrationBuilder.DropIndex(
			name: "IX_TestMultipleChoices_TestInfoId",
			table: "TestMultipleChoices");

		_ = migrationBuilder.RenameColumn(
			name: "TestInfoId",
			table: "TestShortAnswers",
			newName: "Interval");

		_ = migrationBuilder.RenameColumn(
			name: "TestInfoId",
			table: "TestMultipleChoices",
			newName: "Interval");

		_ = migrationBuilder.AlterColumn<int>(
			name: "NoteId",
			table: "TestShortAnswers",
			type: "INTEGER",
			nullable: true,
			oldClrType: typeof(int),
			oldType: "INTEGER");

		_ = migrationBuilder.AddColumn<DateTime>(
			name: "Updated",
			table: "TestShortAnswers",
			type: "TEXT",
			nullable: false,
			defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

		_ = migrationBuilder.AlterColumn<int>(
			name: "NoteId",
			table: "TestMultipleChoices",
			type: "INTEGER",
			nullable: true,
			oldClrType: typeof(int),
			oldType: "INTEGER");

		_ = migrationBuilder.AddColumn<DateTime>(
			name: "Updated",
			table: "TestMultipleChoices",
			type: "TEXT",
			nullable: false,
			defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		_ = migrationBuilder.DropColumn(
			name: "Updated",
			table: "TestShortAnswers");

		_ = migrationBuilder.DropColumn(
			name: "Updated",
			table: "TestMultipleChoices");

		_ = migrationBuilder.RenameColumn(
			name: "Interval",
			table: "TestShortAnswers",
			newName: "TestInfoId");

		_ = migrationBuilder.RenameColumn(
			name: "Interval",
			table: "TestMultipleChoices",
			newName: "TestInfoId");

		_ = migrationBuilder.AlterColumn<int>(
			name: "NoteId",
			table: "TestShortAnswers",
			type: "INTEGER",
			nullable: false,
			defaultValue: 0,
			oldClrType: typeof(int),
			oldType: "INTEGER",
			oldNullable: true);

		_ = migrationBuilder.AlterColumn<int>(
			name: "NoteId",
			table: "TestMultipleChoices",
			type: "INTEGER",
			nullable: false,
			defaultValue: 0,
			oldClrType: typeof(int),
			oldType: "INTEGER",
			oldNullable: true);

		_ = migrationBuilder.CreateTable(
			name: "TestInfos",
			columns: table => new
			{
				Id = table.Column<int>(type: "INTEGER", nullable: false)
					.Annotation("Sqlite:Autoincrement", true),
				Occurrence = table.Column<int>(type: "INTEGER", nullable: false),
				Updated = table.Column<DateTime>(type: "TEXT", nullable: false)
			},
			constraints: table =>
			{
				_ = table.PrimaryKey("PK_TestInfos", x => x.Id);
			});

		_ = migrationBuilder.CreateIndex(
			name: "IX_TestShortAnswers_TestInfoId",
			table: "TestShortAnswers",
			column: "TestInfoId");

		_ = migrationBuilder.CreateIndex(
			name: "IX_TestMultipleChoices_TestInfoId",
			table: "TestMultipleChoices",
			column: "TestInfoId");

		_ = migrationBuilder.AddForeignKey(
			name: "FK_TestMultipleChoices_TestInfos_TestInfoId",
			table: "TestMultipleChoices",
			column: "TestInfoId",
			principalTable: "TestInfos",
			principalColumn: "Id",
			onDelete: ReferentialAction.Cascade);

		_ = migrationBuilder.AddForeignKey(
			name: "FK_TestShortAnswers_TestInfos_TestInfoId",
			table: "TestShortAnswers",
			column: "TestInfoId",
			principalTable: "TestInfos",
			principalColumn: "Id",
			onDelete: ReferentialAction.Cascade);
	}
}
