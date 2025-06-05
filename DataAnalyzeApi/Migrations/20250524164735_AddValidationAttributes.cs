using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAnalyzeApi.Migrations;

/// <inheritdoc />
public partial class AddValidationAttributes : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "Value",
            table: "ParameterValues",
            type: "character varying(50)",
            maxLength: 50,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text");

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "Parameters",
            type: "character varying(50)",
            maxLength: 50,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text");

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "Datasets",
            type: "character varying(50)",
            maxLength: 50,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text");

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "DataObjects",
            type: "character varying(50)",
            maxLength: 50,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "Value",
            table: "ParameterValues",
            type: "text",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(50)",
            oldMaxLength: 50);

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "Parameters",
            type: "text",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(50)",
            oldMaxLength: 50);

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "Datasets",
            type: "text",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(50)",
            oldMaxLength: 50);

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "DataObjects",
            type: "text",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(50)",
            oldMaxLength: 50);
    }
}
