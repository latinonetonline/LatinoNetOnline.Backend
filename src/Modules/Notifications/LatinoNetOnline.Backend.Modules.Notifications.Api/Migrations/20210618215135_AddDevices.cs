using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LatinoNetOnline.Backend.Modules.Notifications.Api.Migrations
{
    public partial class AddDevices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "notifications");

            migrationBuilder.CreateTable(
                name: "Devices",
                schema: "notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PushEndpoint = table.Column<string>(type: "text", nullable: false),
                    PushP256DH = table.Column<string>(type: "text", nullable: false),
                    PushAuth = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Devices",
                schema: "notifications");
        }
    }
}
