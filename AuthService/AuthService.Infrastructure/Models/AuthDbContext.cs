using AuthService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Models
{
    public class AuthDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        { 
        }
        public virtual DbSet<User> users { get; set; }
        public virtual DbSet<Role> roles { get; set; }
        public virtual DbSet<UserRole> userRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // --- Configure Many-to-Many Key ---
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            // --- Seed Roles ---
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Admin", RoleDescription = "Admin user" },
                new Role { RoleId = 2, RoleName = "Agent", RoleDescription = "Support agent" },
                new Role { RoleId = 3, RoleName = "Customer", RoleDescription = "End customer" }
            );

            // --- Seed Users ---
            var staticCreatedDate = new DateTime(2025, 10, 5, 20, 56, 22, DateTimeKind.Utc);
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    UserName = "admin",
                    Email = "admin@supporthub.com",
                    PasswordHash = "$2b$10$f2dXi5mshOoIvA68eCbaKOREF4BtWy7u4T.uZt1pXjaXZwHrbgKoS", //admin123
                    CreatedAt = staticCreatedDate
                },
                new User
                {
                    UserId = 2,
                    UserName = "agent",
                    Email = "agent@supporthub.com",
                    PasswordHash = "$2b$10$PKA31qEzgPtkeEDriinSN.X23wzgvc2UbsyOf6eCSIIwVK4XGZj2y", //agent123
                    CreatedAt = staticCreatedDate
                },
                new User
                {
                    UserId = 3,
                    UserName = "customer",
                    Email = "customer@supporthub.com",
                    PasswordHash = "$2b$10$MfKOL5BAldw0QORTaipoaeS0ZpHrUbfxqsGYMznVIL7IW0mLvBW3y", //cust123
                    CreatedAt = staticCreatedDate
                }
            );

            // --- Seed UserRoles (many-to-many mapping) ---
            modelBuilder.Entity<UserRole>().HasData(
                new UserRole { UserId = 1, RoleId = 1 }, // admin -> Admin
                new UserRole { UserId = 2, RoleId = 2 }, // agent1 -> Agent
                new UserRole { UserId = 3, RoleId = 3 }  // customer1 -> Customer
            );
        }

    } 
}
