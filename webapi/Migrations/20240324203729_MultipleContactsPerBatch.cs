using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class MultipleContactsPerBatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChangelogBatches_Campaigns_CampaignId",
                table: "ChangelogBatches");

            migrationBuilder.DropForeignKey(
                name: "FK_ChangelogBatches_Contacts_ContactId",
                table: "ChangelogBatches");

            migrationBuilder.DropIndex(
                name: "IX_ChangelogBatches_ContactId",
                table: "ChangelogBatches");

            migrationBuilder.DropColumn(
                name: "ContactId",
                table: "ChangelogBatches");

            migrationBuilder.AlterColumn<int>(
                name: "CampaignId",
                table: "ChangelogBatches",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "ChangelogBatchContact",
                columns: table => new
                {
                    ChangelogBatchesId = table.Column<int>(type: "int", nullable: false),
                    ContactsContactId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangelogBatchContact", x => new { x.ChangelogBatchesId, x.ContactsContactId });
                    table.ForeignKey(
                        name: "FK_ChangelogBatchContact_ChangelogBatches_ChangelogBatchesId",
                        column: x => x.ChangelogBatchesId,
                        principalTable: "ChangelogBatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChangelogBatchContact_Contacts_ContactsContactId",
                        column: x => x.ContactsContactId,
                        principalTable: "Contacts",
                        principalColumn: "ContactId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChangelogBatchContact_ContactsContactId",
                table: "ChangelogBatchContact",
                column: "ContactsContactId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChangelogBatches_Campaigns_CampaignId",
                table: "ChangelogBatches",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "CampaignId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChangelogBatches_Campaigns_CampaignId",
                table: "ChangelogBatches");

            migrationBuilder.DropTable(
                name: "ChangelogBatchContact");

            migrationBuilder.AlterColumn<int>(
                name: "CampaignId",
                table: "ChangelogBatches",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContactId",
                table: "ChangelogBatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
    }
}
