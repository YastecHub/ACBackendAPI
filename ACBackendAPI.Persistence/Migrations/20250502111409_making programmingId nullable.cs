using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ACBackendAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class makingprogrammingIdnullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicInformations_Programmes_ProgrammeId",
                table: "AcademicInformations");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProgrammeId",
                table: "AcademicInformations",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicInformations_Programmes_ProgrammeId",
                table: "AcademicInformations",
                column: "ProgrammeId",
                principalTable: "Programmes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicInformations_Programmes_ProgrammeId",
                table: "AcademicInformations");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProgrammeId",
                table: "AcademicInformations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicInformations_Programmes_ProgrammeId",
                table: "AcademicInformations",
                column: "ProgrammeId",
                principalTable: "Programmes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
