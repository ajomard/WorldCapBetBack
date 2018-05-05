using Microsoft.AspNetCore.Authorization;

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
