using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mneme.Integrations.Pluralsight.Migrations;

/// <inheritdoc />
public partial class Init2 : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		_ = migrationBuilder.AlterColumn<DateTime>(
			name: "CreationTime",
			table: "PluralsightSources",
			type: "TEXT",
			nullable: false,
			defaultValueSql: "CURRENT_TIMESTAMP",
			oldClrType: typeof(DateTime),
			oldType: "TEXT",
			oldDefaultValueSql: "GETDATE()");

		_ = migrationBuilder.AlterColumn<DateTime>(
			name: "CreationTime",
			table: "PluralsightNotes",
			type: "TEXT",
			nullable: false,
			defaultValueSql: "CURRENT_TIMESTAMP",
			oldClrType: typeof(DateTime),
			oldType: "TEXT",
			oldDefaultValueSql: "GETDATE()");
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		_ = migrationBuilder.AlterColumn<DateTime>(
			name: "CreationTime",
			table: "PluralsightSources",
			type: "TEXT",
			nullable: false,
			defaultValueSql: "GETDATE()",
			oldClrType: typeof(DateTime),
			oldType: "TEXT",
			oldDefaultValueSql: "CURRENT_TIMESTAMP");

		_ = migrationBuilder.AlterColumn<DateTime>(
			name: "CreationTime",
			table: "PluralsightNotes",
			type: "TEXT",
			nullable: false,
			defaultValueSql: "GETDATE()",
			oldClrType: typeof(DateTime),
			oldType: "TEXT",
			oldDefaultValueSql: "CURRENT_TIMESTAMP");
	}
}
