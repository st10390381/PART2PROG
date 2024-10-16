using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PART2PROG.Migrations
{
    /// <inheritdoc />
    public partial class Main : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Claim",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "Claim",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Doc",
                columns: table => new
                {
                    SupportingDocumentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClaimId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doc", x => x.SupportingDocumentId);
                    table.ForeignKey(
                        name: "FK_Doc_Claim_ClaimId",
                        column: x => x.ClaimId,
                        principalTable: "Claim",
                        principalColumn: "ClaimId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Claim_ApplicationUserId",
                table: "Claim",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Doc_ClaimId",
                table: "Doc",
                column: "ClaimId");

            migrationBuilder.AddForeignKey(
                name: "FK_Claim_AspNetUsers_ApplicationUserId",
                table: "Claim",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claim_AspNetUsers_ApplicationUserId",
                table: "Claim");

            migrationBuilder.DropTable(
                name: "Doc");

            migrationBuilder.DropIndex(
                name: "IX_Claim_ApplicationUserId",
                table: "Claim");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Claim");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "Claim");
        }
    }
}
