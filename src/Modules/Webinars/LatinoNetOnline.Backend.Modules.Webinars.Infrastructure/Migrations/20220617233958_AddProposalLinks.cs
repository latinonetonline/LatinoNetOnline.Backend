using Microsoft.EntityFrameworkCore.Migrations;

namespace LatinoNetOnline.Backend.Modules.Webinars.Core.Migrations
{
    public partial class AddProposalLinks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Flyer",
                schema: "cfs",
                table: "Proposals",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LiveStreaming",
                schema: "cfs",
                table: "Proposals",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Meetup",
                schema: "cfs",
                table: "Proposals",
                type: "text",
                nullable: true);


            migrationBuilder.AddColumn<string>(
                name: "Streamyard",
                schema: "cfs",
                table: "Proposals",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WebinarNumber",
                schema: "cfs",
                table: "Proposals",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Flyer",
                schema: "cfs",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "LiveStreaming",
                schema: "cfs",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "Meetup",
                schema: "cfs",
                table: "Proposals");


            migrationBuilder.DropColumn(
                name: "Streamyard",
                schema: "cfs",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "WebinarNumber",
                schema: "cfs",
                table: "Proposals");
        }
    }
}
