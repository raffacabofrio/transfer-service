using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TransferService.Infra.Data.Migrations
{
    public partial class AddTransferModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TransferId",
                table: "Entries",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Transfers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    UserIdOrigin = table.Column<Guid>(nullable: false),
                    UserIdDestination = table.Column<Guid>(nullable: false),
                    Value = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transfers_Users_UserIdDestination",
                        column: x => x.UserIdDestination,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Transfers_Users_UserIdOrigin",
                        column: x => x.UserIdOrigin,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entries_TransferId",
                table: "Entries",
                column: "TransferId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_UserIdDestination",
                table: "Transfers",
                column: "UserIdDestination");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_UserIdOrigin",
                table: "Transfers",
                column: "UserIdOrigin");

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Transfers_TransferId",
                table: "Entries",
                column: "TransferId",
                principalTable: "Transfers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Transfers_TransferId",
                table: "Entries");

            migrationBuilder.DropTable(
                name: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Entries_TransferId",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "TransferId",
                table: "Entries");
        }
    }
}
