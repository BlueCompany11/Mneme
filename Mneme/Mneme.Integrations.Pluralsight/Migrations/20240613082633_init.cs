using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mneme.Integrations.Pluralsight.Migrations
{
	/// <inheritdoc />
	public partial class init : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
					name: "PluralsightConfigs",
					columns: table => new
					{
						Id = table.Column<int>(type: "INTEGER", nullable: false)
									.Annotation("Sqlite:Autoincrement", true),
						FilePath = table.Column<string>(type: "TEXT", nullable: false)
					},
					constraints: table =>
					{
						table.PrimaryKey("PK_PluralsightConfigs", x => x.Id);
					});

			migrationBuilder.CreateTable(
					name: "PluralsightSources",
					columns: table => new
					{
						Id = table.Column<int>(type: "INTEGER", nullable: false)
									.Annotation("Sqlite:Autoincrement", true),
						IntegrationId = table.Column<string>(type: "TEXT", nullable: true),
						Title = table.Column<string>(type: "TEXT", nullable: true),
						Active = table.Column<bool>(type: "INTEGER", nullable: false)
					},
					constraints: table =>
					{
						table.PrimaryKey("PK_PluralsightSources", x => x.Id);
					});

			migrationBuilder.CreateTable(
					name: "PluralsightPreelaboration",
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
						CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
						Content = table.Column<string>(type: "TEXT", nullable: true)
					},
					constraints: table =>
					{
						table.PrimaryKey("PK_PluralsightPreelaboration", x => x.Id);
						table.ForeignKey(
											name: "FK_PluralsightPreelaboration_PluralsightSources_SourceId",
											column: x => x.SourceId,
											principalTable: "PluralsightSources",
											principalColumn: "Id",
											onDelete: ReferentialAction.Cascade);
					});

			migrationBuilder.CreateIndex(
					name: "IX_PluralsightPreelaboration_IntegrationId",
					table: "PluralsightPreelaboration",
					column: "IntegrationId",
					unique: true);

			migrationBuilder.CreateIndex(
					name: "IX_PluralsightPreelaboration_SourceId",
					table: "PluralsightPreelaboration",
					column: "SourceId");

			migrationBuilder.CreateIndex(
					name: "IX_PluralsightSources_IntegrationId",
					table: "PluralsightSources",
					column: "IntegrationId",
					unique: true);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
					name: "PluralsightConfigs");

			migrationBuilder.DropTable(
					name: "PluralsightPreelaboration");

			migrationBuilder.DropTable(
					name: "PluralsightSources");
		}
	}
}
