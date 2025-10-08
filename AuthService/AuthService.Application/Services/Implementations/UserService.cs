using AuthService.Application.DTOs;
using AuthService.Application.Repositories;
using AuthService.Application.Services.Abstrations;
using AuthService.Domain.Models;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace AuthService.Application.Services.Implementations
{
    public class UserService:IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        IConfiguration _configuration;
        public UserService(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        private string GenerateJwtToken(UserDTO user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            int ExpireMinutes = Convert.ToInt32(_configuration["Jwt:ExpireMinutes"]);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };


            if (user.Roles != null && user.Roles.Any())
            {
                foreach (var role in user.Roles)
                {
                    claims.Add(new Claim("Roles", role.RoleName));
                }
            }


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

                    userDto.Token = GenerateJwtToken(userDto);

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
                //UserDTO userDTO = _mapper.Map<UserDTO>(user);
                //return userDTO;
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

        public UserDTO RegisterUser(SignUpDTO signUpDTO)
        {
            // Check if user with the same email already exists
            var existingUser = _userRepository.GetUserByEmailAsync(signUpDTO.Email).Result;
            if (existingUser != null)
            {
                // User with the same email already exists
                return null;
            }
            // Map UserDTO to User entity
            User user = new User
            {
                UserName = signUpDTO.Name,
                Email = signUpDTO.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(signUpDTO.Password),
                CreatedAt = DateTime.UtcNow
            };
            var roleIds = _userRepository.GetRoleIdByNameAsync(signUpDTO.Roles).Result;

            foreach (var role in roleIds)
            {
                user.UserRoles.Add(new UserRole
                {
                    UserId = user.UserId,
                    RoleId = role
                });
            }
            // Assign default role (e.g., "Customer") to the new user
            //var defaultRole = _userRepository.GetRoleByNameAsync(signUpDTO.Roles).Result;
            //if (defaultRole != null)
            //{
            //    user.UserRoles = new List<UserRole>
            //    {
            //        new UserRole { RoleId = defaultRole.RoleId, User = user }
            //    };
            //}
            // Save the new user to the database
            User createdUser = _userRepository.AddUserAsync(user).Result;
            // Map back to UserDTO
            //var createdUserDto = new UserDTO
            //{
            //    UserId = createdUser.UserId,
            //    Name = createdUser.UserName,
            //    Email = createdUser.Email,
            //    Roles = createdUser.UserRoles?
            //        .Where(ur => ur?.Role != null)
            //        .Select(ur => new RoleDTO
            //        {
            //            RoleId = ur.Role.RoleId,
            //            RoleName = ur.Role.RoleName,
            //            RoleDescription = ur.Role.RoleDescription
            //        })
            //        .ToArray() ?? Array.Empty<RoleDTO>()
            //};
            //return createdUserDto;
            if (createdUser != null)
            {
                //UserDTO userDTO = _mapper.Map<UserDTO>(user);
                //return userDTO;
                //return user;
                var roleDtos = createdUser.UserRoles?
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
