using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnusedTablesAddInfoForBatches : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DonatedDevices");

            migrationBuilder.DropTable(
                name: "DonationReviews");

            migrationBuilder.DropTable(
                name: "ProvisionedDevices");

            migrationBuilder.DropTable(
                name: "Donations");

            migrationBuilder.DropTable(
                name: "Provisions");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalInformation",
                table: "ChangelogBatches",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Campaigns",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalInformation",
                table: "ChangelogBatches");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Campaigns",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Donations",
                columns: table => new
                {
                    DonationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampaignId = table.Column<int>(type: "int", nullable: true),
                    DonationDate = table.Column<DateTime>(type: "date", nullable: false),
                    DonationType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DonorLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DonorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalDeviceCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donations", x => x.DonationId);
                    table.ForeignKey(
                        name: "FK_Donations_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "CampaignId");
                });

            migrationBuilder.CreateTable(
                name: "Provisions",
                columns: table => new
                {
                    ProvisionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampaignId = table.Column<int>(type: "int", nullable: false),
                    ProvisionDate = table.Column<DateTime>(type: "date", nullable: false),
                    ProvisionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecipientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalDeviceCount = table.Column<int>(type: "int", nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provisions", x => x.ProvisionId);
                    table.ForeignKey(
                        name: "FK_Provisions_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "CampaignId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DonatedDevices",
                columns: table => new
                {
                    DeviceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceTypeId = table.Column<int>(type: "int", nullable: false),
                    DonationId = table.Column<int>(type: "int", nullable: false),
                    DeviceCount = table.Column<int>(type: "int", nullable: false),
                    DeviceGrade = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonatedDevices", x => x.DeviceId);
                    table.ForeignKey(
                        name: "FK_DonatedDevices_DeviceTypes_DeviceTypeId",
                        column: x => x.DeviceTypeId,
                        principalTable: "DeviceTypes",
                        principalColumn: "DeviceTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DonatedDevices_Donations_DonationId",
                        column: x => x.DonationId,
                        principalTable: "Donations",
                        principalColumn: "DonationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DonationReviews",
                columns: table => new
                {
                    DonationReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DonationApprovedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DonationCreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DonationId = table.Column<int>(type: "int", nullable: false),
                    ApprovedOn = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: false),
                    DonationStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonationReviews", x => x.DonationReviewId);
                    table.ForeignKey(
                        name: "FK_DonationReviews_AspNetUsers_DonationApprovedById",
                        column: x => x.DonationApprovedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DonationReviews_AspNetUsers_DonationCreatedById",
                        column: x => x.DonationCreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DonationReviews_Donations_DonationId",
                        column: x => x.DonationId,
                        principalTable: "Donations",
                        principalColumn: "DonationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProvisionedDevices",
                columns: table => new
                {
                    DeviceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceTypeId = table.Column<int>(type: "int", nullable: false),
                    ProvisionId = table.Column<int>(type: "int", nullable: false),
                    DeviceCount = table.Column<int>(type: "int", nullable: false),
                    DeviceGrade = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProvisionedDevices", x => x.DeviceId);
                    table.ForeignKey(
                        name: "FK_ProvisionedDevices_DeviceTypes_DeviceTypeId",
                        column: x => x.DeviceTypeId,
                        principalTable: "DeviceTypes",
                        principalColumn: "DeviceTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProvisionedDevices_Provisions_ProvisionId",
                        column: x => x.ProvisionId,
                        principalTable: "Provisions",
                        principalColumn: "ProvisionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DonatedDevices_DeviceTypeId",
                table: "DonatedDevices",
                column: "DeviceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DonatedDevices_DonationId",
                table: "DonatedDevices",
                column: "DonationId");

            migrationBuilder.CreateIndex(
                name: "IX_DonationReviews_DonationApprovedById",
                table: "DonationReviews",
                column: "DonationApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_DonationReviews_DonationCreatedById",
                table: "DonationReviews",
                column: "DonationCreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DonationReviews_DonationId",
                table: "DonationReviews",
                column: "DonationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Donations_CampaignId",
                table: "Donations",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_ProvisionedDevices_DeviceTypeId",
                table: "ProvisionedDevices",
                column: "DeviceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProvisionedDevices_ProvisionId",
                table: "ProvisionedDevices",
                column: "ProvisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Provisions_CampaignId",
                table: "Provisions",
                column: "CampaignId");
        }
    }
}
