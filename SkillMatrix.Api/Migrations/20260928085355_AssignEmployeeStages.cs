using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillMatrix.Api.Migrations
{
    /// <inheritdoc />
    public partial class AssignEmployeeStages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "AssignEmployees");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "AssignEmployees",
                newName: "UStartDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "AStartDate",
                table: "AssignEmployees",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TEndDate",
                table: "AssignEmployees",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TStartDate",
                table: "AssignEmployees",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UEndDate",
                table: "AssignEmployees",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AStartDate",
                table: "AssignEmployees");

            migrationBuilder.DropColumn(
                name: "TEndDate",
                table: "AssignEmployees");

            migrationBuilder.DropColumn(
                name: "TStartDate",
                table: "AssignEmployees");

            migrationBuilder.DropColumn(
                name: "UEndDate",
                table: "AssignEmployees");

            migrationBuilder.RenameColumn(
                name: "UStartDate",
                table: "AssignEmployees",
                newName: "EndDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "AssignEmployees",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
