using GymManagementSystem.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagementSystem.DAL.Data.DataSeeding;

public static class IdentityDataSeeding
{
    public const string SuperAdminRole = "SuperAdmin";
    public const string AdminRole = "Admin";

    private const string DemoPassword = "Gym@12345";

    public static async Task SeedAsync(IServiceProvider services)
    {
        try
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            foreach (var role in new[] { SuperAdminRole, AdminRole })
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            await EnsureUserAsync(userManager, "Super", "Admin", "superadmin", "superadmin@gym.com", "01012345678", SuperAdminRole);
            await EnsureUserAsync(userManager, "Gym", "Admin", "admin", "admin@gym.com", "01112345678", AdminRole);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Identity seeding failed: {ex.Message}");
        }
    }

    private static async Task EnsureUserAsync(UserManager<ApplicationUser> userManager, string firstName,
        string lastName, string userName, string email, string phone, string role)
    {
        if (await userManager.FindByEmailAsync(email) is not null)
            return;

        var user = new ApplicationUser
        {
            FirstName = firstName,
            LastName = lastName,
            UserName = userName,
            Email = email,
            PhoneNumber = phone,
            EmailConfirmed = true
        };

        var created = await userManager.CreateAsync(user, DemoPassword);
        if (created.Succeeded)
            await userManager.AddToRoleAsync(user, role);
    }
}
