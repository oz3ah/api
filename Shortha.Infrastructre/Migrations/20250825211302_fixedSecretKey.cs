using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shortha.Infrastructre.Migrations
{
    /// <inheritdoc />
    public partial class fixedSecretKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SecretKey",
                table: "AppConnection",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecretKey",
                table: "AppConnection");
        }
    }
}
