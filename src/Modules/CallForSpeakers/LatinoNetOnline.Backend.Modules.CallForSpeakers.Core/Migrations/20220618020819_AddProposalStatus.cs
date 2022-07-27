using Microsoft.EntityFrameworkCore.Migrations;

namespace LatinoNetOnline.Backend.Modules.Webinars.Core.Migrations
{
    public partial class AddProposalStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "cfs",
                table: "Proposals",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                schema: "cfs",
                table: "Proposals");
        }
    }
}
