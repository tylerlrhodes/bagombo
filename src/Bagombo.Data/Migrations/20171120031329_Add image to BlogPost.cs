using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Bagombo.Data.Migrations
{
    public partial class AddimagetoBlogPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Topic_Title",
                table: "Topic");

            migrationBuilder.DropIndex(
                name: "IX_Category_Name",
                table: "Category");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "BlogPost",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Topic_Title",
                table: "Topic",
                column: "Title",
                unique: true,
                filter: "[Title] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Category_Name",
                table: "Category",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Topic_Title",
                table: "Topic");

            migrationBuilder.DropIndex(
                name: "IX_Category_Name",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "BlogPost");

            migrationBuilder.CreateIndex(
                name: "IX_Topic_Title",
                table: "Topic",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Category_Name",
                table: "Category",
                column: "Name",
                unique: true);
        }
    }
}
