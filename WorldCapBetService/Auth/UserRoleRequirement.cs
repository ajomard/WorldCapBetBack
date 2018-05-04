using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldCapBetService.Auth
{
    public class UserRoleRequirement : IAuthorizationRequirement
    {
        public string Role { get; set; }

        public UserRoleRequirement(string role)
        {
            this.Role = role;
        }
    }
}
