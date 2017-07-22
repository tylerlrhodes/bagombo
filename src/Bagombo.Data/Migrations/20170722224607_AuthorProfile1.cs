using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bagombo.Data.Migrations
{
    public partial class AuthorProfile1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Blurb",
                table: "Author",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageLink",
                table: "Author",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Blurb",
                table: "Author");

            migrationBuilder.DropColumn(
                name: "ImageLink",
                table: "Author");
        }
    }
}
