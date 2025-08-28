using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiveHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AuthInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Auth0UserId",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "Users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Auth0UserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Users");
        }
    }
}
