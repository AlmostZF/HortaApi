using Microsoft.AspNetCore.Identity;

namespace HortaGestao.Infrastructure.Identity;

public static class IdentitySeeder
{
    public static async Task SeedRolesAsync(RoleManager<ApplicationRole> roleManager)
    {
        string[] roles = {"Customer", "Seller"};

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new ApplicationRole
                {
                    Name = role,
                });
            }
        }
    }
}