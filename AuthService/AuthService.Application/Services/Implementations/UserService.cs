using AuthService.Application.DTOs;
using AuthService.Application.Repositories;
using AuthService.Application.Services.Abstrations;
using AuthService.Domain.Models;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AuthService.Application.Services.Implementations
{
    public class UserService:IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        IConfiguration _configuration;
        public UserService(IUserRepository userRepository)//, IMapper mapper, ITokenService tokenService)
        {
            _userRepository = userRepository;
            //_mapper = mapper;
            //_tokenService = tokenService;
        }

        private string GenerateJwtToken(UserDTO user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            int ExpireMinutes = Convert.ToInt32(_configuration["Jwt:ExpireMinutes"]);


            var claims = new[] {
                             new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, user.Name),
                             new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, user.Email),
                             new Claim("Roles", string.Join(",",user.Roles.ToString())),
                             new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                             };


            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                                            _configuration["Jwt:Audience"],
                                            claims,
                                            expires: DateTime.UtcNow.AddMinutes(ExpireMinutes), //token expiry minutes
                                            signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public IEnumerable<UserDTO> GetAllUsers()
        {
            var users = _userRepository.GetAllUserWithRolesAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }


        public UserDTO LoginUser(LoginDTO loginDTO)
        {
            User user = _userRepository.GetUserByEmailAsync(loginDTO.Email).Result;
            if (user != null)
            {
                bool isPasswordMatch = BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.PasswordHash);
                if (isPasswordMatch)
                {
                    //UserDTO data = _mapper.Map<UserDTO>(user);
                    //data.Token = GenerateJwtToken(data);
                    //return data;
                    var roleDtos = user.UserRoles?
                .Where(ur => ur?.Role != null)
                .Select(ur => new RoleDTO
                {
                    RoleId = ur.Role.RoleId,
                    RoleName = ur.Role.RoleName,
                    RoleDescription = ur.Role.RoleDescription
                })
                .ToArray() ?? Array.Empty<RoleDTO>();

                    var userDto = new UserDTO
                    {
                        UserId = user.UserId,
                        Name = user.UserName,
                        Email = user.Email,
                        Token = "",
                        Roles = roleDtos
                    };

                    //userDto.Token = GenerateJwtToken(userDto);

                    return userDto;
                }
            }
            return null;
        }

        public UserDTO GetUserByEmail(string email)
        {
            User user = _userRepository.GetUserByEmailAsync(email).Result;
            if (user != null)
            {
                //return _mapper.Map<UserDTO>(user);
                //return user;
                var roleDtos = user.UserRoles?
                .Where(ur => ur?.Role != null)
                .Select(ur => new RoleDTO
                {
                    RoleId = ur.Role.RoleId,
                    RoleName = ur.Role.RoleName,
                    RoleDescription = ur.Role.RoleDescription
                })
                .ToArray() ?? Array.Empty<RoleDTO>();

                var userDto = new UserDTO
                {
                    UserId = user.UserId,
                    Name = user.UserName,
                    Email = user.Email,
                    Token = "",
                    Roles = roleDtos
                };



                return userDto;

            }
            return null;
        }

    }
}
