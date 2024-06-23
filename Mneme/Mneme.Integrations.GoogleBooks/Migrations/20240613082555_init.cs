using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mneme.Integrations.GoogleBooks.Migrations
{
	/// <inheritdoc />
	public partial class init : Migration
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
						Active = table.Column<bool>(type: "INTEGER", nullable: false)
					},
					constraints: table =>
					{
						table.PrimaryKey("PK_GoogleBooksSources", x => x.Id);
					});

			migrationBuilder.CreateTable(
					name: "GoogleBooksPreelaborations",
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
						table.PrimaryKey("PK_GoogleBooksPreelaborations", x => x.Id);
						table.ForeignKey(
											name: "FK_GoogleBooksPreelaborations_GoogleBooksSources_SourceId",
											column: x => x.SourceId,
											principalTable: "GoogleBooksSources",
											principalColumn: "Id",
											onDelete: ReferentialAction.Cascade);
					});

			migrationBuilder.CreateIndex(
					name: "IX_GoogleBooksPreelaborations_IntegrationId",
					table: "GoogleBooksPreelaborations",
					column: "IntegrationId",
					unique: true);

			migrationBuilder.CreateIndex(
					name: "IX_GoogleBooksPreelaborations_SourceId",
					table: "GoogleBooksPreelaborations",
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
					name: "GoogleBooksPreelaborations");

			migrationBuilder.DropTable(
					name: "GoogleBooksSources");
		}
	}
}
