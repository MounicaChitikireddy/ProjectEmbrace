using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class AddUserSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    UserSettingsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TableDensity = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "comfortable"),
                    AddInventoryDefaultGrade = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "B-FRN"),
                    RemoveInventoryDefaultGrade = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "B-FRN"),
                    TableFullScreenByDefault = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AvatarColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvatarText = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.UserSettingsId);
                    table.ForeignKey(
                        name: "FK_UserSettings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_UserId",
                table: "UserSettings",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSettings");
        }
    }
}
