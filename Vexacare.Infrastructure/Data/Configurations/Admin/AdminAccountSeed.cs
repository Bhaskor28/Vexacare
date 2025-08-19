using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Domain.Entities.PatientEntities;

namespace Vexacare.Infrastructure.Data.Configurations.Admin
{
    public static class DataSeeder
    {
        public static async Task SeedAdminUserAndRoleAsync(UserManager<Patient> userManager,
                                                     RoleManager<IdentityRole> roleManager)
        {
            // 1. Seed Admin Role
            var adminRole = new IdentityRole("Admin")
            {
                NormalizedName = "ADMIN",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            if (!await roleManager.RoleExistsAsync(adminRole.Name))
            {
                await roleManager.CreateAsync(adminRole);
            }

            // 2. Seed Admin User using your Patient class
            var adminUser = new Patient
            {
                UserName = "admin@vexacare.com",
                NormalizedUserName = "ADMIN@VEXACARE.COM",
                Email = "admin@vexacare.com",
                NormalizedEmail = "ADMIN@VEXACARE.COM",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = true,
                Role = "Admin",
                AccessFailedCount = 0,
                FirstName = "System",
                LastName = "Administrator"
            };

            var existingUser = await userManager.FindByEmailAsync(adminUser.Email);
            if (existingUser == null)
            {
                // Create with secure password
                var createResult = await userManager.CreateAsync(adminUser, "Admin@123");

                if (createResult.Succeeded)
                {
                    // 3. Assign Admin role
                    await userManager.AddToRoleAsync(adminUser, adminRole.Name);
                }
                else
                {
                    // Handle errors (log them)
                    var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    throw new Exception($"Admin user creation failed: {errors}");
                }
            }
        }
    }
}
