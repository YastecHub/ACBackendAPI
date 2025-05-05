using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ACBackendAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addnewfieldstotheprogrammetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Programmes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ProgrammeDescription",
                table: "Programmes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Programmes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Programmes");

            migrationBuilder.DropColumn(
                name: "ProgrammeDescription",
                table: "Programmes");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Programmes");
        }
    }
}
