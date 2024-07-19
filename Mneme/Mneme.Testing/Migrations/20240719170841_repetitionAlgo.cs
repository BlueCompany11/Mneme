using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mneme.Testing.Migrations
{
    /// <inheritdoc />
    public partial class repetitionAlgo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestMultipleChoices_TestInfos_TestInfoId",
                table: "TestMultipleChoices");

            migrationBuilder.DropForeignKey(
                name: "FK_TestShortAnswers_TestInfos_TestInfoId",
                table: "TestShortAnswers");

            migrationBuilder.DropTable(
                name: "TestInfos");

            migrationBuilder.DropIndex(
                name: "IX_TestShortAnswers_TestInfoId",
                table: "TestShortAnswers");

            migrationBuilder.DropIndex(
                name: "IX_TestMultipleChoices_TestInfoId",
                table: "TestMultipleChoices");

            migrationBuilder.RenameColumn(
                name: "TestInfoId",
                table: "TestShortAnswers",
                newName: "Interval");

            migrationBuilder.RenameColumn(
                name: "TestInfoId",
                table: "TestMultipleChoices",
                newName: "Interval");

            migrationBuilder.AlterColumn<int>(
                name: "NoteId",
                table: "TestShortAnswers",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "TestShortAnswers",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "NoteId",
                table: "TestMultipleChoices",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "TestMultipleChoices",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Updated",
                table: "TestShortAnswers");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "TestMultipleChoices");

            migrationBuilder.RenameColumn(
                name: "Interval",
                table: "TestShortAnswers",
                newName: "TestInfoId");

            migrationBuilder.RenameColumn(
                name: "Interval",
                table: "TestMultipleChoices",
                newName: "TestInfoId");

            migrationBuilder.AlterColumn<int>(
                name: "NoteId",
                table: "TestShortAnswers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NoteId",
                table: "TestMultipleChoices",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.CreateTable(
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
                    table.PrimaryKey("PK_TestInfos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestShortAnswers_TestInfoId",
                table: "TestShortAnswers",
                column: "TestInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_TestMultipleChoices_TestInfoId",
                table: "TestMultipleChoices",
                column: "TestInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestMultipleChoices_TestInfos_TestInfoId",
                table: "TestMultipleChoices",
                column: "TestInfoId",
                principalTable: "TestInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestShortAnswers_TestInfos_TestInfoId",
                table: "TestShortAnswers",
                column: "TestInfoId",
                principalTable: "TestInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
