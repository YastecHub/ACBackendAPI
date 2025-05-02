using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ACBackendAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class testingMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LessonType",
                table: "AcademicInformations");

            migrationBuilder.DropColumn(
                name: "LessonTypeDesc",
                table: "AcademicInformations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LessonType",
                table: "AcademicInformations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LessonTypeDesc",
                table: "AcademicInformations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
