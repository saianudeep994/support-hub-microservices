using AuthService.Application.DTOs;
using AuthService.Domain.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Mapper
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            // Role → RoleDTO
            CreateMap<Role, RoleDTO>().ReverseMap();
            //CreateMap<User, UserDTO>()
            //   .ForMember(d => d.Name, m => m.MapFrom(s => s.UserName))
            //   .ForMember(d => d.Roles, m => m.MapFrom(s => s.UserRoles.Select(ur => ur.Role)));

            CreateMap<User, UserDTO>()
            .ForMember(d => d.Name, m => m.MapFrom(s => s.UserName))
            .ForMember(d => d.Roles, m => m.MapFrom(s =>
                s.UserRoles != null
                    ? s.UserRoles.Where(ur => ur.Role != null)
                                 .Select(ur => ur.Role)
                    : new List<Role>()));

            // User → UserDTO
            //CreateMap<User, UserDTO>()
            //.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserName))
            //.ForMember(dest => dest.Roles, opt => opt.MapFrom(src =>
            //    src.UserRoles != null
            //        ? src.UserRoles
            //            .Where(ur => ur != null && ur.Role != null)
            //            .Select(ur => new RoleDTO
            //            {
            //                RoleId = ur.Role.RoleId,
            //                RoleName = ur.Role.RoleName,
            //                RoleDescription = ur.Role.RoleDescription
            //            })
            //            .ToArray()
            //        : Array.Empty<RoleDTO>()))
            //.ForMember(dest => dest.Token, opt => opt.Ignore()); // Set manually








            //CreateMap<User, UserDTO>()
            //    .ForMember(d => d.UserId, o => o.MapFrom(s => (long)s.UserId))   // int -> long
            //    .ForMember(d => d.Name, o => o.MapFrom(s => s.UserName))       // name mismatch
            //    .ForMember(d => d.Roles, o => o.MapFrom(s => s.UserRoles.Select(ur => ur.Role)));

        }

    }
}
