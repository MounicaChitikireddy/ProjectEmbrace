using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNamingScheme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonationDetails_Donation_DonationId",
                table: "DonationDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Donation",
                table: "Donation");

            migrationBuilder.RenameTable(
                name: "Donation",
                newName: "Donations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Donations",
                table: "Donations",
                column: "DonationId");

            migrationBuilder.AddForeignKey(
                name: "FK_DonationDetails_Donations_DonationId",
                table: "DonationDetails",
                column: "DonationId",
                principalTable: "Donations",
                principalColumn: "DonationId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonationDetails_Donations_DonationId",
                table: "DonationDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Donations",
                table: "Donations");

            migrationBuilder.RenameTable(
                name: "Donations",
                newName: "Donation");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Donation",
                table: "Donation",
                column: "DonationId");

            migrationBuilder.AddForeignKey(
                name: "FK_DonationDetails_Donation_DonationId",
                table: "DonationDetails",
                column: "DonationId",
                principalTable: "Donation",
                principalColumn: "DonationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
