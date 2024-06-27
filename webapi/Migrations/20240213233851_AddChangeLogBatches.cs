using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class AddChangeLogBatches : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrentDevicesHistory_AspNetUsers_CurrentHistoryCreatedById",
                table: "CurrentDevicesHistory");

            migrationBuilder.DropIndex(
                name: "IX_CurrentDevicesHistory_CurrentHistoryCreatedById",
                table: "CurrentDevicesHistory");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "CurrentDevicesHistory");

            migrationBuilder.DropColumn(
                name: "CurrentHistoryCreatedById",
                table: "CurrentDevicesHistory");

            migrationBuilder.AlterColumn<int>(
                name: "Action",
                table: "CurrentDevicesHistory",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "BatchId",
                table: "CurrentDevicesHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "CurrentDevicesHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DeviceHistoryBatches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchCreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: false),
                    ApprovedOn = table.Column<DateTime>(type: "date", nullable: true),
                    BatchApprovedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReviewStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceHistoryBatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceHistoryBatches_AspNetUsers_BatchApprovedById",
                        column: x => x.BatchApprovedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DeviceHistoryBatches_AspNetUsers_BatchCreatedById",
                        column: x => x.BatchCreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurrentDevicesHistory_BatchId",
                table: "CurrentDevicesHistory",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceHistoryBatches_BatchApprovedById",
                table: "DeviceHistoryBatches",
                column: "BatchApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceHistoryBatches_BatchCreatedById",
                table: "DeviceHistoryBatches",
                column: "BatchCreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrentDevicesHistory_DeviceHistoryBatches_BatchId",
                table: "CurrentDevicesHistory",
                column: "BatchId",
                principalTable: "DeviceHistoryBatches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrentDevicesHistory_DeviceHistoryBatches_BatchId",
                table: "CurrentDevicesHistory");

            migrationBuilder.DropTable(
                name: "DeviceHistoryBatches");

            migrationBuilder.DropIndex(
                name: "IX_CurrentDevicesHistory_BatchId",
                table: "CurrentDevicesHistory");

            migrationBuilder.DropColumn(
                name: "BatchId",
                table: "CurrentDevicesHistory");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "CurrentDevicesHistory");

            migrationBuilder.AlterColumn<string>(
                name: "Action",
                table: "CurrentDevicesHistory",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "CurrentDevicesHistory",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CurrentHistoryCreatedById",
                table: "CurrentDevicesHistory",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CurrentDevicesHistory_CurrentHistoryCreatedById",
                table: "CurrentDevicesHistory",
                column: "CurrentHistoryCreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrentDevicesHistory_AspNetUsers_CurrentHistoryCreatedById",
                table: "CurrentDevicesHistory",
                column: "CurrentHistoryCreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
