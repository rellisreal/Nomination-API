using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace nomination_api.Migrations
{
    /// <inheritdoc />
    public partial class further_changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Format",
                table: "Nominations",
                newName: "Formal");

            migrationBuilder.AddColumn<long>(
                name: "CategoryId",
                table: "Nominations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "NominationMessage",
                table: "Nominations",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Nominations_CategoryId",
                table: "Nominations",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Nominations_Categories_CategoryId",
                table: "Nominations",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nominations_Categories_CategoryId",
                table: "Nominations");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Nominations_CategoryId",
                table: "Nominations");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Nominations");

            migrationBuilder.DropColumn(
                name: "NominationMessage",
                table: "Nominations");

            migrationBuilder.RenameColumn(
                name: "Formal",
                table: "Nominations",
                newName: "Format");
        }
    }
}
