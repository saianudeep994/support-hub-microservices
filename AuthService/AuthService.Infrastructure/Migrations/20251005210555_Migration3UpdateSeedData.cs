using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration3UpdateSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "Email",
                value: "agent@supporthub.com");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: 3,
                column: "Email",
                value: "customer@supporthub.com");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "Email",
                value: "agent1@supporthub.com");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: 3,
                column: "Email",
                value: "customer1@supporthub.com");
        }
    }
}
