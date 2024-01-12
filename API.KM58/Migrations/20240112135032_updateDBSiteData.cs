using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.KM58.Migrations
{
    /// <inheritdoc />
    public partial class updateDBSiteData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CheckAccountAPI",
                table: "Sites",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ecremarks",
                table: "Sites",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Point",
                table: "Sites",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PointClientAPI",
                table: "Sites",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "Sites",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Round",
                table: "Sites",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckAccountAPI",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "Ecremarks",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "Point",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "PointClientAPI",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "Round",
                table: "Sites");
        }
    }
}
