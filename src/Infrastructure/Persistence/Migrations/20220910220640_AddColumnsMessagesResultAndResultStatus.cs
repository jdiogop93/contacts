using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contacts.Infrastructure.Persistence.Migrations
{
    public partial class AddColumnsMessagesResultAndResultStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Result",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ResultStatus",
                table: "Messages",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Result",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ResultStatus",
                table: "Messages");
        }
    }
}
