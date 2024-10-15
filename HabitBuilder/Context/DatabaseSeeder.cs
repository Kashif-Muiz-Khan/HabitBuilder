using HabitBuilder.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;



namespace HabitBuilder.Context
{
    public class DatabaseSeeder
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DatabaseSeeder(DatabaseContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Seed()
        {
            await _context.Database.MigrateAsync();



            if (!_context.Users.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("Customer"));

                var adminEmail = "admin@habitbuilder.com";
                var adminPassword = "Habitbuilder@123";

                var admin = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                };

                await _userManager.CreateAsync(admin, adminPassword);
                await _userManager.AddToRoleAsync(admin, "Admin");
            }

            await ResetAdminUser();
        }

        private async Task ResetAdminUser()
        {
            var admin = _context.Users.FirstOrDefault(user => user.UserName == "admin@habitbuilder.com");
            if (admin is not null && !await _userManager.IsInRoleAsync(admin, "Admin"))
            {
                await _userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}
