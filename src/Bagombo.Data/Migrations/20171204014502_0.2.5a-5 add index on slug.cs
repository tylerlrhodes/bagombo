using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Bagombo.Data.Migrations
{
    public partial class _025a5addindexonslug : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "BlogPost",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogPost_Slug",
                table: "BlogPost",
                column: "Slug",
                unique: true,
                filter: "[Slug] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BlogPost_Slug",
                table: "BlogPost");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "BlogPost",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
