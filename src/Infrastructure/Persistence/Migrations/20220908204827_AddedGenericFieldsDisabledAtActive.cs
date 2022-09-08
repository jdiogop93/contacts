using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contacts.Infrastructure.Persistence.Migrations
{
    public partial class AddedGenericFieldsDisabledAtActive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "TodoLists",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DisabledAt",
                table: "TodoLists",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "TodoItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DisabledAt",
                table: "TodoItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DisabledAt",
                table: "Messages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Contacts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DisabledAt",
                table: "Contacts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "ContactNumbers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DisabledAt",
                table: "ContactNumbers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "ContactGroups",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DisabledAt",
                table: "ContactGroups",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "TodoLists");

            migrationBuilder.DropColumn(
                name: "DisabledAt",
                table: "TodoLists");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "DisabledAt",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "DisabledAt",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "DisabledAt",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "ContactNumbers");

            migrationBuilder.DropColumn(
                name: "DisabledAt",
                table: "ContactNumbers");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "ContactGroups");

            migrationBuilder.DropColumn(
                name: "DisabledAt",
                table: "ContactGroups");
        }
    }
}
