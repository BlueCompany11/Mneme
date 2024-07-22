using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mneme.Integrations.Mneme.Migrations;

/// <inheritdoc />
public partial class Init2 : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		_ = migrationBuilder.AlterColumn<DateTime>(
			name: "CreationTime",
			table: "MnemeSources",
			type: "TEXT",
			nullable: false,
			defaultValueSql: "CURRENT_TIMESTAMP",
			oldClrType: typeof(DateTime),
			oldType: "TEXT",
			oldDefaultValueSql: "GETDATE()");

		_ = migrationBuilder.AlterColumn<DateTime>(
			name: "CreationTime",
			table: "MnemeNotes",
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
			table: "MnemeSources",
			type: "TEXT",
			nullable: false,
			defaultValueSql: "GETDATE()",
			oldClrType: typeof(DateTime),
			oldType: "TEXT",
			oldDefaultValueSql: "CURRENT_TIMESTAMP");

		_ = migrationBuilder.AlterColumn<DateTime>(
			name: "CreationTime",
			table: "MnemeNotes",
			type: "TEXT",
			nullable: false,
			defaultValueSql: "GETDATE()",
			oldClrType: typeof(DateTime),
			oldType: "TEXT",
			oldDefaultValueSql: "CURRENT_TIMESTAMP");
	}
}
