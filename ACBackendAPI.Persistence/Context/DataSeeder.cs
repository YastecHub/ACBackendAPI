using ACBackendAPI.Domain.Entities;
using ACBackendAPI.Domain.Enum;
using Microsoft.AspNetCore.Identity;

namespace ACBackendAPI.Persistence.Context
{
    public class DataSeeder
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public DataSeeder(AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAdminAsync()
        {
            var adminEmail = "adamsadebola@gmail.com";
            var existingAdmin = await _userManager.FindByEmailAsync(adminEmail);

            if (existingAdmin == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "Adams",
                    Email = adminEmail,
                    Surname = "Adebola",
                    LastName = "Ade",
                    Gender = Gender.Male,
                    DateOfBirth = new DateOnly(1980, 1, 1),
                    Nationality = "Nigeria",
                    Address = "Admin HQ"
                };

                var result = await _userManager.CreateAsync(adminUser, "Admin@123!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "Admin");

                    var admin = new Admin
                    {
                        Name = "Adams",
                        Email = adminEmail,
                        PhoneNumber = "09068913009",
                        Address = "Admin HQ",
                        ApplicationUserId = adminUser.Id
                    };

                    _context.Admins.Add(admin);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
