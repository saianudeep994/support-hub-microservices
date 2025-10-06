using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Domain.Models;

namespace AuthService.Application.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        //Task<bool> ValidatePasswordAsync(string email, string password);
        Task<List<string>> GetUserRolesAsync(int userId);
        Task<User> GetUserWithRolesAsync(int userId);
        Task<List<User>> GetAllUserWithRolesAsync();
        Task<bool> UserExistsAsync(string email);
    }
}
