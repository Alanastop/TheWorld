using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TheWorld.Migrations
{
    public partial class AddingIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstTrip",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstTrip",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "DateTime",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }
    }
}
