using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SkillMatrix.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    AreaCode = table.Column<string>(type: "text", nullable: false),
                    AreaName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.AreaCode);
                });

            migrationBuilder.CreateTable(
                name: "Contractors",
                columns: table => new
                {
                    ContractorId = table.Column<string>(type: "text", nullable: false),
                    ContractorName = table.Column<string>(type: "text", nullable: false),
                    ContractorPhoto = table.Column<string>(type: "text", nullable: true),
                    DateOfJoining = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contractors", x => x.ContractorId);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DeptCode = table.Column<string>(type: "text", nullable: false),
                    DeptName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DeptCode);
                });

            migrationBuilder.CreateTable(
                name: "Eatons",
                columns: table => new
                {
                    EatonId = table.Column<string>(type: "text", nullable: false),
                    EatonName = table.Column<string>(type: "text", nullable: false),
                    Photo = table.Column<string>(type: "text", nullable: true),
                    DateOfJoining = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eatons", x => x.EatonId);
                });

            migrationBuilder.CreateTable(
                name: "WorkStations",
                columns: table => new
                {
                    WorkStationCode = table.Column<string>(type: "text", nullable: false),
                    WorkStationName = table.Column<string>(type: "text", nullable: false),
                    AreaCode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkStations", x => x.WorkStationCode);
                    table.ForeignKey(
                        name: "FK_WorkStations_Areas_AreaCode",
                        column: x => x.AreaCode,
                        principalTable: "Areas",
                        principalColumn: "AreaCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmpId = table.Column<string>(type: "text", nullable: false),
                    EmpName = table.Column<string>(type: "text", nullable: false),
                    EmpCategory = table.Column<string>(type: "text", nullable: false),
                    Photo = table.Column<string>(type: "text", nullable: true),
                    PhoneNo = table.Column<string>(type: "text", nullable: false),
                    DeptCode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmpId);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DeptCode",
                        column: x => x.DeptCode,
                        principalTable: "Departments",
                        principalColumn: "DeptCode",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DeptCode = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    PasswordSalt = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Departments_DeptCode",
                        column: x => x.DeptCode,
                        principalTable: "Departments",
                        principalColumn: "DeptCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssignEmployees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpId = table.Column<string>(type: "text", nullable: false),
                    AreaCode = table.Column<string>(type: "text", nullable: false),
                    WorkStationCode = table.Column<string>(type: "text", nullable: false),
                    C = table.Column<bool>(type: "boolean", nullable: false),
                    A = table.Column<bool>(type: "boolean", nullable: false),
                    U = table.Column<bool>(type: "boolean", nullable: false),
                    T = table.Column<bool>(type: "boolean", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssignEmployees_Areas_AreaCode",
                        column: x => x.AreaCode,
                        principalTable: "Areas",
                        principalColumn: "AreaCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssignEmployees_Employees_EmpId",
                        column: x => x.EmpId,
                        principalTable: "Employees",
                        principalColumn: "EmpId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignEmployees_WorkStations_WorkStationCode",
                        column: x => x.WorkStationCode,
                        principalTable: "WorkStations",
                        principalColumn: "WorkStationCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssignEmployees_AreaCode",
                table: "AssignEmployees",
                column: "AreaCode");

            migrationBuilder.CreateIndex(
                name: "IX_AssignEmployees_EmpId",
                table: "AssignEmployees",
                column: "EmpId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignEmployees_WorkStationCode",
                table: "AssignEmployees",
                column: "WorkStationCode");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DeptCode",
                table: "Employees",
                column: "DeptCode");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DeptCode",
                table: "Users",
                column: "DeptCode");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkStations_AreaCode",
                table: "WorkStations",
                column: "AreaCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignEmployees");

            migrationBuilder.DropTable(
                name: "Contractors");

            migrationBuilder.DropTable(
                name: "Eatons");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "WorkStations");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Areas");
        }
    }
}
