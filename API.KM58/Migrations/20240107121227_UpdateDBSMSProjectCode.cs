using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.KM58.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDBSMSProjectCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectCode",
                table: "SMS",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectCode",
                table: "SMS");
        }
    }
}
