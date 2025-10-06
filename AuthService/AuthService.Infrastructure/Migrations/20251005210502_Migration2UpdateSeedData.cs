using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration2UpdateSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "UserName",
                value: "agent");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: 3,
                column: "UserName",
                value: "customer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "UserName",
                value: "agent1");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: 3,
                column: "UserName",
                value: "customer1");
        }
    }
}
