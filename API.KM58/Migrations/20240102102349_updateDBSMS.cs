using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.KM58.Migrations
{
    /// <inheritdoc />
    public partial class updateDBSMS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "SMS",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Device",
                table: "SMS",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditTime",
                table: "SMS",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "SMS");

            migrationBuilder.DropColumn(
                name: "Device",
                table: "SMS");

            migrationBuilder.DropColumn(
                name: "EditTime",
                table: "SMS");
        }
    }
}
