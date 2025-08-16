using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shortha.Infrastructre.Migrations
{
    /// <inheritdoc />
    public partial class mj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Visits",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApiKey",
                table: "Urls",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreationSource",
                table: "Urls",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Api",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    Key = table.Column<string>(type: "character varying(64)", unicode: false, maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    LastUsed = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Api", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Api_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Visits_AppUserId",
                table: "Visits",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Urls_ApiKey",
                table: "Urls",
                column: "ApiKey");

            migrationBuilder.CreateIndex(
                name: "IX_Api_UserId",
                table: "Api",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Urls_Api_ApiKey",
                table: "Urls",
                column: "ApiKey",
                principalTable: "Api",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Visits_Users_AppUserId",
                table: "Visits",
                column: "AppUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Urls_Api_ApiKey",
                table: "Urls");

            migrationBuilder.DropForeignKey(
                name: "FK_Visits_Users_AppUserId",
                table: "Visits");

            migrationBuilder.DropTable(
                name: "Api");

            migrationBuilder.DropIndex(
                name: "IX_Visits_AppUserId",
                table: "Visits");

            migrationBuilder.DropIndex(
                name: "IX_Urls_ApiKey",
                table: "Urls");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Visits");

            migrationBuilder.DropColumn(
                name: "ApiKey",
                table: "Urls");

            migrationBuilder.DropColumn(
                name: "CreationSource",
                table: "Urls");
        }
    }
}
