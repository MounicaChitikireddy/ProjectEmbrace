using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class FixAllDonationRelatedNavProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonationReview_AspNetUsers_UserApprovedById",
                table: "DonationReview");

            migrationBuilder.DropForeignKey(
                name: "FK_DonationReview_AspNetUsers_UserCreatedById",
                table: "DonationReview");

            migrationBuilder.DropIndex(
                name: "IX_DonationReview_UserApprovedById",
                table: "DonationReview");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "DonationReview");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "DonationReview");

            migrationBuilder.DropColumn(
                name: "UserApprovedById",
                table: "DonationReview");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserCreatedById",
                table: "DonationReview",
                newName: "ApprovedById");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "DonationReview",
                newName: "CreatedOn");

            migrationBuilder.RenameIndex(
                name: "IX_DonationReview_UserCreatedById",
                table: "DonationReview",
                newName: "IX_DonationReview_ApprovedById");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "DonationReview",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DonationId",
                table: "DonationReview",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "AspNetUsers",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "AspNetUsers",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DonationReview_CreatedById",
                table: "DonationReview",
                column: "CreatedById");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_DonationReview_CreatedById",
                table: "DonationReview");

            migrationBuilder.DropIndex(
                name: "IX_DonationReview_DonationId",
                table: "DonationReview");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "DonationReview");

            migrationBuilder.DropColumn(
                name: "DonationId",
                table: "DonationReview");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "DonationReview",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "ApprovedById",
                table: "DonationReview",
                newName: "UserCreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_DonationReview_ApprovedById",
                table: "DonationReview",
                newName: "IX_DonationReview_UserCreatedById");

            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "DonationReview",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "DonationReview",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserApprovedById",
                table: "DonationReview",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DonationReview_UserApprovedById",
                table: "DonationReview",
                column: "UserApprovedById");

            migrationBuilder.AddForeignKey(
                name: "FK_DonationReview_AspNetUsers_UserApprovedById",
                table: "DonationReview",
                column: "UserApprovedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DonationReview_AspNetUsers_UserCreatedById",
                table: "DonationReview",
                column: "UserCreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
