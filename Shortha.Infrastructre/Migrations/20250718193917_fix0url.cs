using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shortha.Infrastructre.Migrations
{
    /// <inheritdoc />
    public partial class fix0url : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortCode",
                table: "Urls",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortCode",
                table: "Urls");
        }
    }
}
