using Hospital.Models;
using Hospital.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Hospital.Utilities
{

    public class DbInitializer : IDbIntializer
    {
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private ApplicationDbContext _context;

        public DbInitializer(UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task IntializeAsync()
        {
            try
            {
                if (!_context.Database.GetPendingMigrations().Any())
                {
                    _context.Database.Migrate();
                }
                if (!await _roleManager.RoleExistsAsync(WebsiteRoles.WebSite_Admin))
                {
                    await _roleManager.CreateAsync(new IdentityRole(WebsiteRoles.WebSite_Admin));
                    await _roleManager.CreateAsync(new IdentityRole(WebsiteRoles.WebSite_Patient));
                    await _roleManager.CreateAsync(new IdentityRole(WebsiteRoles.WebSite_Doctor));

                    var adminUser = new ApplicationUser
                    {
                        UserName = "mehdi",
                        Email = "mehdi@gmail.com"
                    };

                    var userExist = await _userManager.FindByEmailAsync(adminUser.Email);
                    if (userExist == null)
                    {
                        await _userManager.CreateAsync(adminUser, "Mehdi300863#");
                        userExist = await _userManager.FindByEmailAsync(adminUser.Email);
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Enter during database initialization: {ex.Message}");
            }
        }
    }
}
