using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using LoanApp.Models;

namespace LoanApp.Services
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = { "GlobalAdmin", "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Seed GlobalAdmin user
            var globalAdminEmail = "globaladmin@loanapp.com";
            var globalAdminUser = await userManager.FindByEmailAsync(globalAdminEmail);
            if (globalAdminUser == null)
            {
                globalAdminUser = new ApplicationUser
                {
                    UserName = globalAdminEmail,
                    Email = globalAdminEmail,
                    EmailConfirmed = true,
                    FullName = "Global Admin",
                    Address = "Admin Address"
                };
                var result = await userManager.CreateAsync(globalAdminUser, "GlobalAdmin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(globalAdminUser, "GlobalAdmin");
                }
            }

            // Seed admin user
            var adminEmail = "admin@loanapp.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FullName = "Admin User",
                    Address = "Admin Address"
                };
                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
