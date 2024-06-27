using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class FixDonationReviewFKAddCurrentDevicesUpdateDonatedDevicesName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Donations_DonationId",
                table: "Devices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Devices",
                table: "Devices");

            migrationBuilder.RenameTable(
                name: "Devices",
                newName: "DonatedDevices");

            migrationBuilder.RenameIndex(
                name: "IX_Devices_DonationId",
                table: "DonatedDevices",
                newName: "IX_DonatedDevices_DonationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DonatedDevices",
                table: "DonatedDevices",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_DonatedDevices_Donations_DonationId",
                table: "DonatedDevices",
                column: "DonationId",
                principalTable: "Donations",
                principalColumn: "DonationId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonatedDevices_Donations_DonationId",
                table: "DonatedDevices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DonatedDevices",
                table: "DonatedDevices");

            migrationBuilder.RenameTable(
                name: "DonatedDevices",
                newName: "Devices");

            migrationBuilder.RenameIndex(
                name: "IX_DonatedDevices_DonationId",
                table: "Devices",
                newName: "IX_Devices_DonationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Devices",
                table: "Devices",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Donations_DonationId",
                table: "Devices",
                column: "DonationId",
                principalTable: "Donations",
                principalColumn: "DonationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
