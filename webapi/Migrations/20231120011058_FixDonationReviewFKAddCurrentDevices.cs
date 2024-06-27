using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class FixDonationReviewFKAddCurrentDevices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonationReview_AspNetUsers_ApprovedById",
                table: "DonationReview");

            migrationBuilder.DropForeignKey(
                name: "FK_DonationReview_AspNetUsers_CreatedById",
                table: "DonationReview");

            migrationBuilder.DropForeignKey(
                name: "FK_DonationReview_Donations_DonationId",
                table: "DonationReview");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DonationReview",
                table: "DonationReview");

            migrationBuilder.DropIndex(
                name: "IX_DonationReview_DonationId",
                table: "DonationReview");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "DonationReview",
                newName: "DonationReviews");

            migrationBuilder.RenameIndex(
                name: "IX_DonationReview_CreatedById",
                table: "DonationReviews",
                newName: "IX_DonationReviews_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_DonationReview_ApprovedById",
                table: "DonationReviews",
                newName: "IX_DonationReviews_ApprovedById");

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_DonationReviews",
                table: "DonationReviews",
                column: "DonationReviewId");

            migrationBuilder.CreateTable(
                name: "CurrentDevices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Grade = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentDevices", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DonationReviews_DonationId",
                table: "DonationReviews",
                column: "DonationId",
                unique: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_DonationReviews_Donations_DonationId",
                table: "DonationReviews",
                column: "DonationId",
                principalTable: "Donations",
                principalColumn: "DonationId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonationReviews_AspNetUsers_ApprovedById",
                table: "DonationReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_DonationReviews_AspNetUsers_CreatedById",
                table: "DonationReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_DonationReviews_Donations_DonationId",
                table: "DonationReviews");

            migrationBuilder.DropTable(
                name: "CurrentDevices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DonationReviews",
                table: "DonationReviews");

            migrationBuilder.DropIndex(
                name: "IX_DonationReviews_DonationId",
                table: "DonationReviews");

            migrationBuilder.RenameTable(
                name: "DonationReviews",
                newName: "DonationReview");

            migrationBuilder.RenameIndex(
                name: "IX_DonationReviews_CreatedById",
                table: "DonationReview",
                newName: "IX_DonationReview_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_DonationReviews_ApprovedById",
                table: "DonationReview",
                newName: "IX_DonationReview_ApprovedById");

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

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DonationReview",
                table: "DonationReview",
                column: "DonationReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_DonationReview_DonationId",
                table: "DonationReview",
                column: "DonationId");

            migrationBuilder.AddForeignKey(
                name: "FK_DonationReview_AspNetUsers_ApprovedById",
                table: "DonationReview",
                column: "ApprovedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DonationReview_AspNetUsers_CreatedById",
                table: "DonationReview",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DonationReview_Donations_DonationId",
                table: "DonationReview",
                column: "DonationId",
                principalTable: "Donations",
                principalColumn: "DonationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
