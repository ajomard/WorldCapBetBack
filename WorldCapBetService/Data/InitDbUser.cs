using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using WorldCapBetService.Models.Entities;
using System.Security.Claims;

namespace WorldCapBetService.Data
{
    public static class InitDbUser
    {
        public static IApplicationBuilder InitDb(this IApplicationBuilder app)
        {
            IServiceScopeFactory scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();

            using (IServiceScope scope = scopeFactory.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();




                if (userManager.Users.Count() == 0)
                {
                    Task.Run(() => InitRoles(roleManager)).Wait();
                    Task.Run(() => InitUsers(userManager)).Wait();
                }
            }

            return app;
        }

        private static async Task InitRoles(RoleManager<IdentityRole> roleManager)
        {
            var role = new IdentityRole("User");
            await roleManager.CreateAsync(role);

            role = new IdentityRole("Admin");
            await roleManager.CreateAsync(role);
            await roleManager.AddClaimAsync(role, new Claim("Admin", "true"));
        }

        private static async Task InitUsers(UserManager<User> userManager)
        {
            var user = new User() { UserName = "akubler", Email = "arnaud.kubler@capgemini.com" };
            await userManager.CreateAsync(user, "123456");
            await userManager.AddToRoleAsync(user, "Admin");
            await userManager.AddToRoleAsync(user, "User");

            user = new User() { UserName = "toto", Email = "toto@capgemini.com" };
            await userManager.CreateAsync(user, "123456");
            await userManager.AddToRoleAsync(user, "User");

        }
    }
}
