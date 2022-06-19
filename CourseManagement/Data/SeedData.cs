using System;
using CourseManagement.Identity;
using Microsoft.AspNetCore.Identity;

namespace CourseManagement.Data
{
    public static class SeedData
    {
        public static void Seed(UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        
        private static void SeedUsers(UserManager<AppUser> userManager)
        {
            if (userManager.FindByEmailAsync("ahmad.s.sawad.99@icloud.com").Result == null)
            {
                var user = new AppUser
                {
                    UserName = "ahmadsawaed",
                    FirstName = "Ahmad",
                    LastName = "Sawaed",
                    Email = "ahmad.s.sawaed.99@icloud.com",
                    Adress = "sallama"
                };
                var result = userManager.CreateAsync(user, "Sawaed2299*").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait(TimeSpan.FromSeconds(5));
                    userManager.AddToRoleAsync(user, "Prof").Wait(TimeSpan.FromSeconds(5));
                }
            }
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Admin"
                };

                var res = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Prof").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Prof"
                };

                var res = roleManager.CreateAsync(role);
            }
        }
    }
}
