﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mneme.Integrations.Pluralsight.Migrations;

/// <inheritdoc />
public partial class Init : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		_ = migrationBuilder.CreateTable(
			name: "PluralsightConfigs",
			columns: table => new
			{
				Id = table.Column<int>(type: "INTEGER", nullable: false)
					.Annotation("Sqlite:Autoincrement", true),
				FilePath = table.Column<string>(type: "TEXT", nullable: false)
			},
			constraints: table =>
			{
				_ = table.PrimaryKey("PK_PluralsightConfigs", x => x.Id);
			});

		_ = migrationBuilder.CreateTable(
			name: "PluralsightSources",
			columns: table => new
			{
				Id = table.Column<int>(type: "INTEGER", nullable: false)
					.Annotation("Sqlite:Autoincrement", true),
				IntegrationId = table.Column<string>(type: "TEXT", nullable: true),
				Title = table.Column<string>(type: "TEXT", nullable: true),
				CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()"),
				Active = table.Column<bool>(type: "INTEGER", nullable: false)
			},
			constraints: table =>
			{
				_ = table.PrimaryKey("PK_PluralsightSources", x => x.Id);
			});

		_ = migrationBuilder.CreateTable(
			name: "PluralsightNotes",
			columns: table => new
			{
				Id = table.Column<int>(type: "INTEGER", nullable: false)
					.Annotation("Sqlite:Autoincrement", true),
				SourceId = table.Column<int>(type: "INTEGER", nullable: false),
				Module = table.Column<string>(type: "TEXT", nullable: false),
				Clip = table.Column<string>(type: "TEXT", nullable: false),
				TimeInClip = table.Column<string>(type: "TEXT", nullable: false),
				IntegrationId = table.Column<string>(type: "TEXT", nullable: true),
				Title = table.Column<string>(type: "TEXT", nullable: true),
				Path = table.Column<string>(type: "TEXT", nullable: true),
				CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()"),
				Content = table.Column<string>(type: "TEXT", nullable: true)
			},
			constraints: table =>
			{
				_ = table.PrimaryKey("PK_PluralsightNotes", x => x.Id);
				_ = table.ForeignKey(
					name: "FK_PluralsightNotes_PluralsightSources_SourceId",
					column: x => x.SourceId,
					principalTable: "PluralsightSources",
					principalColumn: "Id",
					onDelete: ReferentialAction.Cascade);
			});

		_ = migrationBuilder.CreateIndex(
			name: "IX_PluralsightNotes_IntegrationId",
			table: "PluralsightNotes",
			column: "IntegrationId",
			unique: true);

		_ = migrationBuilder.CreateIndex(
			name: "IX_PluralsightNotes_SourceId",
			table: "PluralsightNotes",
			column: "SourceId");

		_ = migrationBuilder.CreateIndex(
			name: "IX_PluralsightSources_IntegrationId",
			table: "PluralsightSources",
			column: "IntegrationId",
			unique: true);
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		_ = migrationBuilder.DropTable(
			name: "PluralsightConfigs");

		_ = migrationBuilder.DropTable(
			name: "PluralsightNotes");

		_ = migrationBuilder.DropTable(
			name: "PluralsightSources");
	}
}
