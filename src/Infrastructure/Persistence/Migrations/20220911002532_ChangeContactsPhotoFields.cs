using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contacts.Infrastructure.Persistence.Migrations
{
    public partial class ChangeContactsPhotoFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileExtension",
                table: "Contacts",
                newName: "MimeType");

            migrationBuilder.AlterColumn<long>(
                name: "Size",
                table: "Contacts",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MimeType",
                table: "Contacts",
                newName: "FileExtension");

            migrationBuilder.AlterColumn<int>(
                name: "Size",
                table: "Contacts",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
