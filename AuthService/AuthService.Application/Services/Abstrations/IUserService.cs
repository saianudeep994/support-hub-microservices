using AuthService.Application.DTOs;
using AuthService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Services.Abstrations
{
    public interface IUserService
    {
        UserDTO LoginUser(LoginDTO loginDTO);
        IEnumerable<UserDTO> GetAllUsers();
        UserDTO GetUserByEmail(string email);

        UserDTO RegisterUser(SignUpDTO signUpDTO);
    }
}
