using Microsoft.EntityFrameworkCore.Migrations;

using System;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Migrations
{
    public partial class AddWebinars : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Webinars",
                schema: "cfs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProposalId = table.Column<Guid>(type: "uuid", nullable: false),
                    YoutubeLink = table.Column<string>(type: "text", nullable: false),
                    MeetupLink = table.Column<string>(type: "text", nullable: false),
                    FlyerLink = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Webinars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Webinars_Proposals_ProposalId",
                        column: x => x.ProposalId,
                        principalSchema: "cfs",
                        principalTable: "Proposals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Webinars_ProposalId",
                schema: "cfs",
                table: "Webinars",
                column: "ProposalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Webinars",
                schema: "cfs");
        }
    }
}
