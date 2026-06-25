using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FocusTrack.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAppClassificationRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppClassificationRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ApplicationName = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppClassificationRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppClassificationRules_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppClassificationRules_ApplicationName",
                table: "AppClassificationRules",
                column: "ApplicationName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppClassificationRules_CategoryId",
                table: "AppClassificationRules",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppClassificationRules");
        }
    }
}
