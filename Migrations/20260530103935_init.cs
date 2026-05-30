using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace nomination_api.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        static readonly Guid roleGuid = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        static readonly Guid userGuid = new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901");
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoleName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 16, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    UserPassword = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserCreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserLastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "TEXT", nullable: false),
                    EventAction = table.Column<string>(type: "TEXT", nullable: true),
                    EventMessage = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    EventDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_Events_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Nominations",
                columns: table => new
                {
                    NominationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Formal = table.Column<bool>(type: "INTEGER", nullable: false),
                    NominationMessage = table.Column<string>(type: "TEXT", nullable: false),
                    NominatorId = table.Column<Guid>(type: "TEXT", nullable: false),
                    NominatedId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CategoryId = table.Column<long>(type: "INTEGER", nullable: false),
                    NominationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NominationLastModified = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nominations", x => x.NominationId);
                    table.ForeignKey(
                        name: "FK_Nominations_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Nominations_Users_NominatedId",
                        column: x => x.NominatedId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Nominations_Users_NominatorId",
                        column: x => x.NominatorId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_UserId",
                table: "Events",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Nominations_CategoryId",
                table: "Nominations",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Nominations_NominatedId",
                table: "Nominations",
                column: "NominatedId");

            migrationBuilder.CreateIndex(
                name: "IX_Nominations_NominatorId",
                table: "Nominations",
                column: "NominatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);

            migrationBuilder.Sql($@"
                INSERT INTO Roles (RoleId, RoleName) 
                VALUES ('{roleGuid}', 'Admin')");

            migrationBuilder.Sql($@"
                INSERT INTO Users (UserId, UserName, Email, UserPassword, RoleId, UserCreatedDate, UserLastUpdated) 
                VALUES ('{userGuid}', 'Admin', 'Admin@admin.com', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', '{roleGuid}', '{DateTime.Now}', '{DateTime.Now}')");
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: $@"{roleGuid}"
            ); 
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: $@"{userGuid}"
            ); 
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Nominations");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
