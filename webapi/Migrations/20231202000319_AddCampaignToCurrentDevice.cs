using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class AddCampaignToCurrentDevice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Contact",
                table: "Contact");

            migrationBuilder.RenameTable(
                name: "Contact",
                newName: "Contacts");

            migrationBuilder.AddColumn<int>(
                name: "CampaignId",
                table: "CurrentDevices",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contacts",
                table: "Contacts",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrentDevices_CampaignId",
                table: "CurrentDevices",
                column: "CampaignId");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrentDevices_Campaigns_CampaignId",
                table: "CurrentDevices",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "CampaignId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrentDevices_Campaigns_CampaignId",
                table: "CurrentDevices");

            migrationBuilder.DropIndex(
                name: "IX_CurrentDevices_CampaignId",
                table: "CurrentDevices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contacts",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "CampaignId",
                table: "CurrentDevices");

            migrationBuilder.RenameTable(
                name: "Contacts",
                newName: "Contact");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contact",
                table: "Contact",
                column: "ContactId");
        }
    }
}
