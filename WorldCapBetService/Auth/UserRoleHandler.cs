using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using static WorldCapBetService.Helpers.Constants.Strings;

namespace WorldCapBetService.Auth
{
    public class UserRoleHandler : AuthorizationHandler<UserRoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserRoleRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == JwtClaimIdentifiers.Rol))
            {
                return Task.CompletedTask;
            }

            var role = context.User.FindFirst(c => c.Type == JwtClaimIdentifiers.Rol);

            if (requirement.Role == role.Value || JwtClaims.AdminAccess == role.Value)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
