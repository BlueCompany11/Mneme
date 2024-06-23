﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mneme.Integrations.Mneme.Migrations
{
	/// <inheritdoc />
	public partial class init : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
					name: "MnemeSources",
					columns: table => new
					{
						Id = table.Column<int>(type: "INTEGER", nullable: false)
									.Annotation("Sqlite:Autoincrement", true),
						Details = table.Column<string>(type: "TEXT", nullable: true),
						IntegrationId = table.Column<string>(type: "TEXT", nullable: true),
						Title = table.Column<string>(type: "TEXT", nullable: true),
						Active = table.Column<bool>(type: "INTEGER", nullable: false)
					},
					constraints: table =>
					{
						table.PrimaryKey("PK_MnemeSources", x => x.Id);
					});

			migrationBuilder.CreateTable(
					name: "MnemePreelaboration",
					columns: table => new
					{
						Id = table.Column<int>(type: "INTEGER", nullable: false)
									.Annotation("Sqlite:Autoincrement", true),
						SourceId = table.Column<int>(type: "INTEGER", nullable: true),
						IntegrationId = table.Column<string>(type: "TEXT", nullable: true),
						Title = table.Column<string>(type: "TEXT", nullable: true),
						Path = table.Column<string>(type: "TEXT", nullable: true),
						CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
						Content = table.Column<string>(type: "TEXT", nullable: true)
					},
					constraints: table =>
					{
						table.PrimaryKey("PK_MnemePreelaboration", x => x.Id);
						table.ForeignKey(
											name: "FK_MnemePreelaboration_MnemeSources_SourceId",
											column: x => x.SourceId,
											principalTable: "MnemeSources",
											principalColumn: "Id");
					});

			migrationBuilder.CreateIndex(
					name: "IX_MnemePreelaboration_IntegrationId",
					table: "MnemePreelaboration",
					column: "IntegrationId",
					unique: true);

			migrationBuilder.CreateIndex(
					name: "IX_MnemePreelaboration_SourceId",
					table: "MnemePreelaboration",
					column: "SourceId");

			migrationBuilder.CreateIndex(
					name: "IX_MnemeSources_IntegrationId",
					table: "MnemeSources",
					column: "IntegrationId",
					unique: true);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
					name: "MnemePreelaboration");

			migrationBuilder.DropTable(
					name: "MnemeSources");
		}
	}
}
