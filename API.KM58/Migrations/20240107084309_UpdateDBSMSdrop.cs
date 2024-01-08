using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.KM58.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDBSMSdrop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EditTime",
                table: "SMS");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "SMS");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EditTime",
                table: "SMS",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdateTime",
                table: "SMS",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
