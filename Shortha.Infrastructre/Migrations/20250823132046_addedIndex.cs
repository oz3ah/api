using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shortha.Infrastructre.Migrations
{
    /// <inheritdoc />
    public partial class addedIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppConnection_UserId",
                table: "AppConnection");

            migrationBuilder.CreateIndex(
                name: "IX_AppConnection_UserId_ConnectKey",
                table: "AppConnection",
                columns: new[] { "UserId", "ConnectKey" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppConnection_UserId_ConnectKey",
                table: "AppConnection");

            migrationBuilder.CreateIndex(
                name: "IX_AppConnection_UserId",
                table: "AppConnection",
                column: "UserId");
        }
    }
}
