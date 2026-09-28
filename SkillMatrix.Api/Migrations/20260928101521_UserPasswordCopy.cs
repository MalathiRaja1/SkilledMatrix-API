using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillMatrix.Api.Migrations
{
    /// <inheritdoc />
    public partial class UserPasswordCopy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordEncrypted",
                table: "Users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordEncrypted",
                table: "Users");
        }
    }
}
