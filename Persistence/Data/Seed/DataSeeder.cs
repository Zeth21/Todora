using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence.Seed
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            string adminEmail = "admin@todora.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdminUser = new User
                {
                    UserName = "admin",
                    Email = adminEmail,
                    EmailConfirmed = true,
                    UserPhotoPath = "default.png"
                };

                var result = await userManager.CreateAsync(newAdminUser, "Admin123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdminUser, "Admin");
                }
            }


            string regularUserEmail = "user@todora.com";
            var regularUser = await userManager.FindByEmailAsync(regularUserEmail);

            if (regularUser == null)
            {
                var newRegularUser = new User
                {
                    UserName = "user",
                    Email = regularUserEmail,
                    EmailConfirmed = true,
                    UserPhotoPath = "default.png"
                };

                var result = await userManager.CreateAsync(newRegularUser, "User123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newRegularUser, "User");
                }
            }
        }
    }
}