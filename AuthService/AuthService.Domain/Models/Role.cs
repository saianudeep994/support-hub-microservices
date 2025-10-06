using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string RoleDescription { get; set; } = string.Empty;

        [System.Text.Json.Serialization.JsonIgnore]
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    }
}
