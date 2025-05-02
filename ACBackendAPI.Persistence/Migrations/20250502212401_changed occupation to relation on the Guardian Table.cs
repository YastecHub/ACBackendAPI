using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ACBackendAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class changedoccupationtorelationontheGuardianTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Occupation",
                table: "Guardians",
                newName: "RelationShip");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RelationShip",
                table: "Guardians",
                newName: "Occupation");
        }
    }
}
