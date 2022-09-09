using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LatinoNetOnline.Backend.Modules.Webinars.Core.Migrations
{
    public partial class AddUserIdToSpeakers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "cfs",
                table: "Speakers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "cfs",
                table: "Speakers");
        }
    }
}
