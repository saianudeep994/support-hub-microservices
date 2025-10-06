using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AuthService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IntialMigrationAndSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "userRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_userRoles_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userRoles_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "RoleId", "RoleDescription", "RoleName" },
                values: new object[,]
                {
                    { 1, "Admin user", "Admin" },
                    { 2, "Support agent", "Agent" },
                    { 3, "End customer", "Customer" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "UserId", "CreatedAt", "Email", "PasswordHash", "UserName" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 10, 5, 20, 56, 22, 0, DateTimeKind.Utc), "admin@supporthub.com", "$2b$10$f2dXi5mshOoIvA68eCbaKOREF4BtWy7u4T.uZt1pXjaXZwHrbgKoS", "admin" },
                    { 2, new DateTime(2025, 10, 5, 20, 56, 22, 0, DateTimeKind.Utc), "agent1@supporthub.com", "$2b$10$PKA31qEzgPtkeEDriinSN.X23wzgvc2UbsyOf6eCSIIwVK4XGZj2y", "agent1" },
                    { 3, new DateTime(2025, 10, 5, 20, 56, 22, 0, DateTimeKind.Utc), "customer1@supporthub.com", "$2b$10$MfKOL5BAldw0QORTaipoaeS0ZpHrUbfxqsGYMznVIL7IW0mLvBW3y", "customer1" }
                });

            migrationBuilder.InsertData(
                table: "userRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_userRoles_RoleId",
                table: "userRoles",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "userRoles");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
