using Microsoft.EntityFrameworkCore.Migrations;

namespace Wechat.Backend.Migrations
{
    public partial class addRequestBody : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RequestBody",
                table: "Logs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestBody",
                table: "Logs");
        }
    }
}
