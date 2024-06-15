//using ForumApp2.Models;
//using Microsoft.AspNetCore.Identity;

//namespace ForumApp2
//{
//    public class SeedData
//    {
//        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
//        {
//            var roleNames = new[] { "Admin", "User" };
//            foreach (var roleName in roleNames)
//            {
//                if (!await roleManager.RoleExistsAsync(roleName))
//                {
//                    await roleManager.CreateAsync(new IdentityRole(roleName));
//                }
//            }

//            // Optionally, create a default admin user
//            var adminUser = new ApplicationUser
//            {
//                UserName = "admin",
//                Email = "admin@example.com"
//            };

//            if (userManager.Users.All(u => u.Id != adminUser.Id))
//            {
//                var user = await userManager.FindByEmailAsync(adminUser.Email);
//                if (user == null)
//                {
//                    await userManager.CreateAsync(adminUser, "AdminPassword123!");
//                    await userManager.AddToRoleAsync(adminUser, "Admin");
//                }
//            }
//        }
//    }
//}
