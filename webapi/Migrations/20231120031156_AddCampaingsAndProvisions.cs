using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class AddCampaingsAndProvisions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CampaignId",
                table: "Donations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceGrade",
                table: "DonatedDevices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Campaigns",
                columns: table => new
                {
                    CampaignId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampaignName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: false),
                    EndDate = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.CampaignId);
                });

            migrationBuilder.CreateTable(
                name: "Provisions",
                columns: table => new
                {
                    ProvisionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProvisionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProvisionDate = table.Column<DateTime>(type: "date", nullable: false),
                    RecipientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalDeviceCount = table.Column<int>(type: "int", nullable: false),
                    CampaignId = table.Column<int>(type: "int", nullable: false)
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
                name: "ProvisionedDevices",
                columns: table => new
                {
                    DeviceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceCategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceSize = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceGrade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceCount = table.Column<int>(type: "int", nullable: false),
                    ProvisionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProvisionedDevices", x => x.DeviceId);
                    table.ForeignKey(
                        name: "FK_ProvisionedDevices_Provisions_ProvisionId",
                        column: x => x.ProvisionId,
                        principalTable: "Provisions",
                        principalColumn: "ProvisionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Donations_CampaignId",
                table: "Donations",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_ProvisionedDevices_ProvisionId",
                table: "ProvisionedDevices",
                column: "ProvisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Provisions_CampaignId",
                table: "Provisions",
                column: "CampaignId");

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_Campaigns_CampaignId",
                table: "Donations",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "CampaignId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donations_Campaigns_CampaignId",
                table: "Donations");

            migrationBuilder.DropTable(
                name: "ProvisionedDevices");

            migrationBuilder.DropTable(
                name: "Provisions");

            migrationBuilder.DropTable(
                name: "Campaigns");

            migrationBuilder.DropIndex(
                name: "IX_Donations_CampaignId",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "CampaignId",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "DeviceGrade",
                table: "DonatedDevices");
        }
    }
}
