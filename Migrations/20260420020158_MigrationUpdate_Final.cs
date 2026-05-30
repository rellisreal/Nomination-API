using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace nomination_api.Migrations
{
    /// <inheritdoc />
    public partial class MigrationUpdate_Final : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleAccessList",
                table: "Roles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoleAccessList",
                table: "Roles",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
