using Microsoft.AspNetCore.Identity;
using TwitterClone.Data.Entities;
namespace TwitterClone.Infrastructure.DataSeeding
{
    public static class RoleSeedService
    {
        public static async Task SeedRolesAndUsersAsync(
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            /// List of roles to seed
            var roles = new[] { "ADMIN", "USER", "MODERATOR" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            /// Seed admin user
            var adminEmail = "admin@example.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var user = new User
                {
                    UserName = "Admin",
                    Email = adminEmail,
                    DateJoined = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(user, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "ADMIN");
                }
            }
        }
    }
}
