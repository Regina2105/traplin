using Microsoft.AspNetCore.Identity;
using Traplin.Core.Entities;

namespace Traplin.Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task Initialize(RoleManager<IdentityRole<Guid>> roleManager, UserManager<AppUser> userManager)
        {
            // Создание ролей
            string[] roleNames = { "applicant", "employer", "curator" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                }
            }

            // Создание администратора (куратора)
            var adminEmail = "admin@traplin.ru";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    DisplayName = "Администратор",
                    IsVerified = true,
                    PrivacyLevel = "private"
                };
                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "curator");
                    await userManager.AddToRoleAsync(adminUser, "applicant"); // может иметь обе роли
                }
            }
        }
    }
}