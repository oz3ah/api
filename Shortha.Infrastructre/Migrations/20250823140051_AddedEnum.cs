using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shortha.Infrastructre.Migrations
{
    /// <inheritdoc />
    public partial class AddedEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActivated",
                table: "AppConnection");

            migrationBuilder.DropColumn(
                name: "IsRevoked",
                table: "AppConnection");

            migrationBuilder.DropColumn(
                name: "SecretKey",
                table: "AppConnection");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "AppConnection",
                type: "text",
                nullable: false,
                defaultValue: "Pending");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "AppConnection");

            migrationBuilder.AddColumn<bool>(
                name: "IsActivated",
                table: "AppConnection",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRevoked",
                table: "AppConnection",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecretKey",
                table: "AppConnection",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
