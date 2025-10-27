using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mneme.Integrations.Mneme.Migrations
{
    /// <inheritdoc />
    public partial class Init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MnemeSources_IntegrationId",
                table: "MnemeSources");

            migrationBuilder.DropIndex(
                name: "IX_MnemeNotes_IntegrationId",
                table: "MnemeNotes");

            migrationBuilder.DropColumn(
                name: "IntegrationId",
                table: "MnemeSources");

            migrationBuilder.DropColumn(
                name: "IntegrationId",
                table: "MnemeNotes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IntegrationId",
                table: "MnemeSources",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IntegrationId",
                table: "MnemeNotes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MnemeSources_IntegrationId",
                table: "MnemeSources",
                column: "IntegrationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MnemeNotes_IntegrationId",
                table: "MnemeNotes",
                column: "IntegrationId",
                unique: true);
        }
    }
}
