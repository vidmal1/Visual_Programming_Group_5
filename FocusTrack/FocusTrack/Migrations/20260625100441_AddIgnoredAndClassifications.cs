using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FocusTrack.Migrations
{
    /// <inheritdoc />
    public partial class AddIgnoredAndClassifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppClassifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AppName = table.Column<string>(type: "TEXT", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppClassifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppClassifications_AppCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "AppCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IgnoredApps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AppName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IgnoredApps", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppCategories_Name",
                table: "AppCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppClassifications_AppName",
                table: "AppClassifications",
                column: "AppName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppClassifications_CategoryId",
                table: "AppClassifications",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_IgnoredApps_AppName",
                table: "IgnoredApps",
                column: "AppName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppClassifications");

            migrationBuilder.DropTable(
                name: "IgnoredApps");

            migrationBuilder.DropIndex(
                name: "IX_AppCategories_Name",
                table: "AppCategories");
        }
    }
}
