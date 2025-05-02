using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ACBackendAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updatetingtheDbwithneccessarydata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicInformations_Programmes_ProgrammeId",
                table: "AcademicInformations");

            migrationBuilder.AddColumn<long>(
                name: "ProgrammeFee",
                table: "Programmes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicInformations_Programmes_ProgrammeId",
                table: "AcademicInformations");

            migrationBuilder.DropColumn(
                name: "ProgrammeFee",
                table: "Programmes");

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
    }
}
