using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.DataSeed
{
    public static class IdentityDbContextSeeding
    {

        public static bool SeedData(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            try
            {

                var hasUsers = userManager.Users.Any();
                var hasRoles = roleManager.Roles.Any();

                if (hasUsers && hasRoles) return false;

                if (!hasRoles)
                {
                    var roles = new List<IdentityRole>()
                {
                    new (){Name="SuperAdmin" },
                    new (){Name="Admin" }
                };

                    foreach (var role in roles)
                    {
                        if (!roleManager.RoleExistsAsync(role.Name!).Result)
                        {
                            roleManager.CreateAsync(role).Wait();
                        }
                    }
                }

                if (!hasUsers)
                {
                    var seedPassword = Environment.GetEnvironmentVariable("GYM_SEED_ADMIN_PASSWORD");
                    if (string.IsNullOrWhiteSpace(seedPassword))
                        throw new InvalidOperationException("Set GYM_SEED_ADMIN_PASSWORD to initialize demo accounts.");

                    var mainAdmin = new ApplicationUser()
                    {
                        FirstName = "Mohamed",
                        LastName = "Elbarbary",
                        UserName = "Mohamed_Elbarbary",
                        Email = "superadmin@example.com",
                        PhoneNumber = "00000000000"
                    };

                    userManager.CreateAsync(mainAdmin, seedPassword).Wait();
                    userManager.AddToRoleAsync(mainAdmin, "SuperAdmin").Wait();

                    var admin = new ApplicationUser()
                    {
                        FirstName = "Mohamed",
                        LastName = "Ali",
                        UserName = "Mohamed_Ali",
                        Email = "admin@example.com",
                        PhoneNumber = "00000000001"
                    };

                    userManager.CreateAsync(admin, seedPassword).Wait();
                    userManager.AddToRoleAsync(admin, "Admin").Wait();

                }
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed To Seed Data : {ex}");
                return false;
            }
        }

    }
}

