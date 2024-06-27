using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class AddDonationReviewTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DonationReview",
                columns: table => new
                {
                    DonationReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: false),
                    ApprovedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DonationStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserCreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserApprovedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonationReview", x => x.DonationReviewId);
                    table.ForeignKey(
                        name: "FK_DonationReview_AspNetUsers_UserApprovedById",
                        column: x => x.UserApprovedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DonationReview_AspNetUsers_UserCreatedById",
                        column: x => x.UserCreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DonationReview_UserApprovedById",
                table: "DonationReview",
                column: "UserApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_DonationReview_UserCreatedById",
                table: "DonationReview",
                column: "UserCreatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DonationReview");
        }
    }
}
