using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using UserIdentityP19.Models;

namespace UserIdentityP19.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var role = new Role { Name = roleName };
                    await roleManager.CreateAsync(role);
                }
            }

            // Create a default admin user
            var adminEmail = "admin@example.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var newAdmin = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    MobileNumber = "1234567890"
                };

                var result = await userManager.CreateAsync(newAdmin, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
        }
    }
}
