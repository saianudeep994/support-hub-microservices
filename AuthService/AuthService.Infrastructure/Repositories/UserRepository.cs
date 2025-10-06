using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Application.Repositories;
using AuthService.Domain.Models;
using AuthService.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext _context;

        public UserRepository(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.users
                    .Include(u => u.UserRoles)
        .ThenInclude(ur => ur.Role)
    .FirstOrDefaultAsync(u => u.Email == email);

            //return await _context.users
            //    .Include(u => u.UserRoles)
            //        .ThenInclude(ur => ur.Role)
            //    .FirstOrDefaultAsync(u => u.Email == email);
            //return await _context.users.FirstOrDefaultAsync(u => u.Email == email);
        }

        //public async Task<bool> ValidatePasswordAsync(string email, string password)
        //{
        //    var user = await GetUserByEmailAsync(email);
        //    if (user == null) return false;

        //    var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        //    return result == PasswordVerificationResult.Success;
        //}

        public async Task<List<string>> GetUserRolesAsync(int userId)
        {
            return await _context.userRoles
                .Where(ur => ur.UserId == userId)
                .Join(_context.roles,
                      ur => ur.RoleId,
                      r => r.RoleId,
                      (ur, r) => r.RoleName)
                .ToListAsync();
        }

        public async Task<User> GetUserWithRolesAsync(int userId)
        {
            return await _context.users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }
        public async Task<List<User>> GetAllUserWithRolesAsync()
        {
            return await _context.users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role).ToListAsync();
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _context.users.AnyAsync(u => u.Email == email);
        }
    }
}
