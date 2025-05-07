using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ACBackendAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addnewentitiestotheprogrammetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProgrammeImage",
                table: "Programmes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProgrammeStatus",
                table: "Programmes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProgrammeStatusDesc",
                table: "Programmes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProgrammeImage",
                table: "Programmes");

            migrationBuilder.DropColumn(
                name: "ProgrammeStatus",
                table: "Programmes");

            migrationBuilder.DropColumn(
                name: "ProgrammeStatusDesc",
                table: "Programmes");
        }
    }
}
