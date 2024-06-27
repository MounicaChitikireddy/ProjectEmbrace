using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class AdjustChangelogBatchColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrentDevicesHistory");

            migrationBuilder.DropTable(
                name: "DeviceHistoryBatches");

            migrationBuilder.CreateTable(
                name: "ChangelogBatches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchCreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: false),
                    ApprovedOn = table.Column<DateTime>(type: "date", nullable: true),
                    BatchApprovedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReviewStatus = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangelogBatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChangelogBatches_AspNetUsers_BatchApprovedById",
                        column: x => x.BatchApprovedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ChangelogBatches_AspNetUsers_BatchCreatedById",
                        column: x => x.BatchCreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChangelogBatchDevices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceTypeId = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Grade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BatchId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangelogBatchDevices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChangelogBatchDevices_ChangelogBatches_BatchId",
                        column: x => x.BatchId,
                        principalTable: "ChangelogBatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChangelogBatchDevices_DeviceTypes_DeviceTypeId",
                        column: x => x.DeviceTypeId,
                        principalTable: "DeviceTypes",
                        principalColumn: "DeviceTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChangelogBatchDevices_BatchId",
                table: "ChangelogBatchDevices",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangelogBatchDevices_DeviceTypeId",
                table: "ChangelogBatchDevices",
                column: "DeviceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangelogBatches_BatchApprovedById",
                table: "ChangelogBatches",
                column: "BatchApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_ChangelogBatches_BatchCreatedById",
                table: "ChangelogBatches",
                column: "BatchCreatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChangelogBatchDevices");

            migrationBuilder.DropTable(
                name: "ChangelogBatches");

            migrationBuilder.CreateTable(
                name: "DeviceHistoryBatches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchApprovedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BatchCreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApprovedOn = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "CurrentDevicesHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchId = table.Column<int>(type: "int", nullable: false),
                    DeviceTypeId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Grade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentDevicesHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrentDevicesHistory_DeviceHistoryBatches_BatchId",
                        column: x => x.BatchId,
                        principalTable: "DeviceHistoryBatches",
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
                name: "IX_CurrentDevicesHistory_BatchId",
                table: "CurrentDevicesHistory",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrentDevicesHistory_DeviceTypeId",
                table: "CurrentDevicesHistory",
                column: "DeviceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceHistoryBatches_BatchApprovedById",
                table: "DeviceHistoryBatches",
                column: "BatchApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceHistoryBatches_BatchCreatedById",
                table: "DeviceHistoryBatches",
                column: "BatchCreatedById");
        }
    }
}
