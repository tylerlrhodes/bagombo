using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace blog.Migrations
{
    public partial class test1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Author_ApplicationUserId",
                table: "Author");

            migrationBuilder.CreateIndex(
                name: "IX_Author_ApplicationUserId",
                table: "Author",
                column: "ApplicationUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Author_ApplicationUserId",
                table: "Author");

            migrationBuilder.CreateIndex(
                name: "IX_Author_ApplicationUserId",
                table: "Author",
                column: "ApplicationUserId",
                unique: true);
        }
    }
}
