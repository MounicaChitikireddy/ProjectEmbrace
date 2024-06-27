using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class AddContactsAndCampaignToBatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CampaignId",
                table: "ChangelogBatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ContactId",
                table: "ChangelogBatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ChangelogBatches_CampaignId",
                table: "ChangelogBatches",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangelogBatches_ContactId",
                table: "ChangelogBatches",
                column: "ContactId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChangelogBatches_Campaigns_CampaignId",
                table: "ChangelogBatches",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "CampaignId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChangelogBatches_Contacts_ContactId",
                table: "ChangelogBatches",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "ContactId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChangelogBatches_Campaigns_CampaignId",
                table: "ChangelogBatches");

            migrationBuilder.DropForeignKey(
                name: "FK_ChangelogBatches_Contacts_ContactId",
                table: "ChangelogBatches");

            migrationBuilder.DropIndex(
                name: "IX_ChangelogBatches_CampaignId",
                table: "ChangelogBatches");

            migrationBuilder.DropIndex(
                name: "IX_ChangelogBatches_ContactId",
                table: "ChangelogBatches");

            migrationBuilder.DropColumn(
                name: "CampaignId",
                table: "ChangelogBatches");

            migrationBuilder.DropColumn(
                name: "ContactId",
                table: "ChangelogBatches");
        }
    }
}
