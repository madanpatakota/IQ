using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Misard.IQs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class userotphone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "UserOtps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "UserOtps");
        }
    }
}
