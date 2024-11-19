using Microsoft.AspNetCore.Identity;

namespace LibraryMangementSystem
{
    public class RoleSeeder : IRoleSeeder
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public RoleSeeder(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task SeedRolesAndAdminUserAsync()
        {
            string[] roleNames = { "Admin", "Librarian", "Member" };
            foreach (var roleName in roleNames)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var adminEmail = "adminZ@admin.com";
            // var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASS");
            
            var adminUser = await _userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var newAdmin = new IdentityUser { UserName = adminEmail, Email = adminEmail };
                var result = await _userManager.CreateAsync(newAdmin, adminPassword);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
        }
    }
}
