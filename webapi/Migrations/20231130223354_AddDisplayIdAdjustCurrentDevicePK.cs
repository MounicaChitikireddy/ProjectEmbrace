using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class AddDisplayIdAdjustCurrentDevicePK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrentDevices",
                table: "CurrentDevices");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "CurrentDevices",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Size",
                table: "CurrentDevices",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "CurrentDevices",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "CurrentDevices",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DisplayId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CurrentDevices_Id",
                table: "CurrentDevices",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrentDevices",
                table: "CurrentDevices",
                columns: new[] { "Category", "Size", "Type", "Location" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AspNetUsers_DisplayId",
                table: "AspNetUsers",
                column: "DisplayId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_CurrentDevices_Id",
                table: "CurrentDevices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrentDevices",
                table: "CurrentDevices");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AspNetUsers_DisplayId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "CurrentDevices");

            migrationBuilder.DropColumn(
                name: "DisplayId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "CurrentDevices",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Size",
                table: "CurrentDevices",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "CurrentDevices",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrentDevices",
                table: "CurrentDevices",
                column: "Id");
        }
    }
}
