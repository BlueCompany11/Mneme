using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mneme.Integrations.Mneme.Migrations;

/// <inheritdoc />
public partial class Init : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		_ = migrationBuilder.CreateTable(
			name: "MnemeSources",
			columns: table => new
			{
				Id = table.Column<int>(type: "INTEGER", nullable: false)
					.Annotation("Sqlite:Autoincrement", true),
				Details = table.Column<string>(type: "TEXT", nullable: true),
				IntegrationId = table.Column<string>(type: "TEXT", nullable: true),
				Title = table.Column<string>(type: "TEXT", nullable: true),
				CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()"),
				Active = table.Column<bool>(type: "INTEGER", nullable: false)
			},
			constraints: table =>
			{
				_ = table.PrimaryKey("PK_MnemeSources", x => x.Id);
			});

		_ = migrationBuilder.CreateTable(
			name: "MnemeNotes",
			columns: table => new
			{
				Id = table.Column<int>(type: "INTEGER", nullable: false)
					.Annotation("Sqlite:Autoincrement", true),
				SourceId = table.Column<int>(type: "INTEGER", nullable: true),
				IntegrationId = table.Column<string>(type: "TEXT", nullable: true),
				Title = table.Column<string>(type: "TEXT", nullable: true),
				Path = table.Column<string>(type: "TEXT", nullable: true),
				CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()"),
				Content = table.Column<string>(type: "TEXT", nullable: true)
			},
			constraints: table =>
			{
				_ = table.PrimaryKey("PK_MnemeNotes", x => x.Id);
				_ = table.ForeignKey(
					name: "FK_MnemeNotes_MnemeSources_SourceId",
					column: x => x.SourceId,
					principalTable: "MnemeSources",
					principalColumn: "Id");
			});

		_ = migrationBuilder.CreateIndex(
			name: "IX_MnemeNotes_IntegrationId",
			table: "MnemeNotes",
			column: "IntegrationId",
			unique: true);

		_ = migrationBuilder.CreateIndex(
			name: "IX_MnemeNotes_SourceId",
			table: "MnemeNotes",
			column: "SourceId");

		_ = migrationBuilder.CreateIndex(
			name: "IX_MnemeSources_IntegrationId",
			table: "MnemeSources",
			column: "IntegrationId",
			unique: true);
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		_ = migrationBuilder.DropTable(
			name: "MnemeNotes");

		_ = migrationBuilder.DropTable(
			name: "MnemeSources");
	}
}
