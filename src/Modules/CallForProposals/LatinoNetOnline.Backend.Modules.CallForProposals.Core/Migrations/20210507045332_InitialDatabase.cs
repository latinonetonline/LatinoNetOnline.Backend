using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LatinoNetOnline.Backend.Modules.CallForProposals.Core.Migrations
{
    public partial class InitialDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "cfs");

            migrationBuilder.CreateTable(
                name: "Proposals",
                schema: "cfs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    AudienceAnswer = table.Column<string>(type: "text", nullable: true),
                    KnowledgeAnswer = table.Column<string>(type: "text", nullable: true),
                    UseCaseAnswer = table.Column<string>(type: "text", nullable: true),
                    EventDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proposals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Speakers",
                schema: "cfs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Twitter = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Image = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Speakers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProposalSpeaker",
                schema: "cfs",
                columns: table => new
                {
                    ProposalsId = table.Column<Guid>(type: "uuid", nullable: false),
                    SpeakersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProposalSpeaker", x => new { x.ProposalsId, x.SpeakersId });
                    table.ForeignKey(
                        name: "FK_ProposalSpeaker_Proposals_ProposalsId",
                        column: x => x.ProposalsId,
                        principalSchema: "cfs",
                        principalTable: "Proposals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProposalSpeaker_Speakers_SpeakersId",
                        column: x => x.SpeakersId,
                        principalSchema: "cfs",
                        principalTable: "Speakers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProposalSpeaker_SpeakersId",
                schema: "cfs",
                table: "ProposalSpeaker",
                column: "SpeakersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProposalSpeaker",
                schema: "cfs");

            migrationBuilder.DropTable(
                name: "Proposals",
                schema: "cfs");

            migrationBuilder.DropTable(
                name: "Speakers",
                schema: "cfs");
        }
    }
}
