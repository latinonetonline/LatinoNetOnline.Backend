using Microsoft.EntityFrameworkCore.Migrations;

namespace LatinoNetOnline.Backend.Modules.Webinars.Core.Migrations
{
    public partial class ProposalViews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LiveAttends",
                schema: "cfs",
                table: "Proposals",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Views",
                schema: "cfs",
                table: "Proposals",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LiveAttends",
                schema: "cfs",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "Views",
                schema: "cfs",
                table: "Proposals");
        }
    }
}
