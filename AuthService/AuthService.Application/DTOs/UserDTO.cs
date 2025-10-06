﻿using AuthService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.DTOs
{
    public class UserDTO
    {
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public RoleDTO[] Roles { get; set; }
        public string Token { get; set; }
    }
}
