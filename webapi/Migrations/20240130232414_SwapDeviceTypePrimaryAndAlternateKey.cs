using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class SwapDeviceTypePrimaryAndAlternateKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonationReviews_AspNetUsers_ApprovedById",
                table: "DonationReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_DonationReviews_AspNetUsers_CreatedById",
                table: "DonationReviews");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CurrentDevices_Id",
                table: "CurrentDevices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrentDevices",
                table: "CurrentDevices");

            migrationBuilder.DropColumn(
                name: "DeviceCategory",
                table: "ProvisionedDevices");

            migrationBuilder.DropColumn(
                name: "DeviceSize",
                table: "ProvisionedDevices");

            migrationBuilder.DropColumn(
                name: "DeviceType",
                table: "ProvisionedDevices");

            migrationBuilder.DropColumn(
                name: "DeviceCategory",
                table: "DonatedDevices");

            migrationBuilder.DropColumn(
                name: "DeviceSize",
                table: "DonatedDevices");

            migrationBuilder.DropColumn(
                name: "DeviceType",
                table: "DonatedDevices");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "CurrentDevices");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "CurrentDevices");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "CurrentDevices");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "DonationReviews",
                newName: "DonationCreatedById");

            migrationBuilder.RenameColumn(
                name: "ApprovedById",
                table: "DonationReviews",
                newName: "DonationApprovedById");

            migrationBuilder.RenameIndex(
                name: "IX_DonationReviews_CreatedById",
                table: "DonationReviews",
                newName: "IX_DonationReviews_DonationCreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_DonationReviews_ApprovedById",
                table: "DonationReviews",
                newName: "IX_DonationReviews_DonationApprovedById");

            migrationBuilder.RenameColumn(
                name: "Count",
                table: "CurrentDevices",
                newName: "DeviceTypeId");

            migrationBuilder.AddColumn<int>(
                name: "DeviceTypeId",
                table: "ProvisionedDevices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DeviceTypeId",
                table: "DonatedDevices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "CurrentDevices",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "AspNetUsers",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Department",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrentDevices",
                table: "CurrentDevices",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DeviceTypes",
                columns: table => new
                {
                    DeviceTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryNormalized = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeNormalized = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SizeNormalized = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceTypes", x => x.DeviceTypeId);
                });

            migrationBuilder.CreateTable(
                name: "CurrentDevicesHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrentHistoryCreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceTypeId = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Grade = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentDevicesHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrentDevicesHistory_AspNetUsers_CurrentHistoryCreatedById",
                        column: x => x.CurrentHistoryCreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CurrentDevicesHistory_DeviceTypes_DeviceTypeId",
                        column: x => x.DeviceTypeId,
                        principalTable: "DeviceTypes",
                        principalColumn: "DeviceTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProvisionedDevices_DeviceTypeId",
                table: "ProvisionedDevices",
                column: "DeviceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DonatedDevices_DeviceTypeId",
                table: "DonatedDevices",
                column: "DeviceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrentDevices_DeviceTypeId",
                table: "CurrentDevices",
                column: "DeviceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrentDevicesHistory_CurrentHistoryCreatedById",
                table: "CurrentDevicesHistory",
                column: "CurrentHistoryCreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CurrentDevicesHistory_DeviceTypeId",
                table: "CurrentDevicesHistory",
                column: "DeviceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceTypes_CategoryNormalized_TypeNormalized_SizeNormalized",
                table: "DeviceTypes",
                columns: new[] { "CategoryNormalized", "TypeNormalized", "SizeNormalized" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrentDevices_DeviceTypes_DeviceTypeId",
                table: "CurrentDevices",
                column: "DeviceTypeId",
                principalTable: "DeviceTypes",
                principalColumn: "DeviceTypeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DonatedDevices_DeviceTypes_DeviceTypeId",
                table: "DonatedDevices",
                column: "DeviceTypeId",
                principalTable: "DeviceTypes",
                principalColumn: "DeviceTypeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DonationReviews_AspNetUsers_DonationApprovedById",
                table: "DonationReviews",
                column: "DonationApprovedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DonationReviews_AspNetUsers_DonationCreatedById",
                table: "DonationReviews",
                column: "DonationCreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProvisionedDevices_DeviceTypes_DeviceTypeId",
                table: "ProvisionedDevices",
                column: "DeviceTypeId",
                principalTable: "DeviceTypes",
                principalColumn: "DeviceTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrentDevices_DeviceTypes_DeviceTypeId",
                table: "CurrentDevices");

            migrationBuilder.DropForeignKey(
                name: "FK_DonatedDevices_DeviceTypes_DeviceTypeId",
                table: "DonatedDevices");

            migrationBuilder.DropForeignKey(
                name: "FK_DonationReviews_AspNetUsers_DonationApprovedById",
                table: "DonationReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_DonationReviews_AspNetUsers_DonationCreatedById",
                table: "DonationReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_ProvisionedDevices_DeviceTypes_DeviceTypeId",
                table: "ProvisionedDevices");

            migrationBuilder.DropTable(
                name: "CurrentDevicesHistory");

            migrationBuilder.DropTable(
                name: "DeviceTypes");

            migrationBuilder.DropIndex(
                name: "IX_ProvisionedDevices_DeviceTypeId",
                table: "ProvisionedDevices");

            migrationBuilder.DropIndex(
                name: "IX_DonatedDevices_DeviceTypeId",
                table: "DonatedDevices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrentDevices",
                table: "CurrentDevices");

            migrationBuilder.DropIndex(
                name: "IX_CurrentDevices_DeviceTypeId",
                table: "CurrentDevices");

            migrationBuilder.DropColumn(
                name: "DeviceTypeId",
                table: "ProvisionedDevices");

            migrationBuilder.DropColumn(
                name: "DeviceTypeId",
                table: "DonatedDevices");

            migrationBuilder.RenameColumn(
                name: "DonationCreatedById",
                table: "DonationReviews",
                newName: "CreatedById");

            migrationBuilder.RenameColumn(
                name: "DonationApprovedById",
                table: "DonationReviews",
                newName: "ApprovedById");

            migrationBuilder.RenameIndex(
                name: "IX_DonationReviews_DonationCreatedById",
                table: "DonationReviews",
                newName: "IX_DonationReviews_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_DonationReviews_DonationApprovedById",
                table: "DonationReviews",
                newName: "IX_DonationReviews_ApprovedById");

            migrationBuilder.RenameColumn(
                name: "DeviceTypeId",
                table: "CurrentDevices",
                newName: "Count");

            migrationBuilder.AddColumn<string>(
                name: "DeviceCategory",
                table: "ProvisionedDevices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeviceSize",
                table: "ProvisionedDevices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeviceType",
                table: "ProvisionedDevices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeviceCategory",
                table: "DonatedDevices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeviceSize",
                table: "DonatedDevices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeviceType",
                table: "DonatedDevices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "CurrentDevices",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "CurrentDevices",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "CurrentDevices",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "CurrentDevices",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "AspNetUsers",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Department",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CurrentDevices_Id",
                table: "CurrentDevices",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrentDevices",
                table: "CurrentDevices",
                columns: new[] { "Category", "Size", "Type", "Location" });

            migrationBuilder.AddForeignKey(
                name: "FK_DonationReviews_AspNetUsers_ApprovedById",
                table: "DonationReviews",
                column: "ApprovedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DonationReviews_AspNetUsers_CreatedById",
                table: "DonationReviews",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
