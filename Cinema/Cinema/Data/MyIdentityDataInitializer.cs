using Cinema.Areas.Identity.User;
using Data.Domain.Entities;
using Data.Persistence;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;

namespace Cinema.Data
{
    public static class MyIdentityDataInitializer
    {
        public static void SeedData(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, DatabaseContext dbContext)
        {
            SeedRoles(roleManager);

            SeedUsers(userManager);

            SeedDatabase(dbContext);
        }

        public static void SeedDatabase(DatabaseContext dbContext)
        {
            Guid locationId = Guid.NewGuid();
            Location newLocation = new Location()
            {
                Id = locationId,
                Street = "Álfabakki 8",
                State = "Reykjavík",
                PostalCode = "109"
            };
            CinemaInfo newCinema = new CinemaInfo()
            {
                Id = Guid.NewGuid(),
                LocationId = locationId,
                Email = "alfabakki@samfilm.is",
                PhoneNumber = "575 8900"
            };

            if (dbContext.Locations.Where(x => x.Street == newLocation.Street).ToList().Count == 0)
            {
                dbContext.Locations.Add(newLocation);
            }
            if (dbContext.Cinemas.Where(x => x.Email == newCinema.Email).ToList().Count == 0)
            {
                dbContext.Cinemas.Add(newCinema);
            }

            dbContext.SaveChanges();

        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            roleManager.CreateRole("Admin"); // Administrators/Moderators
            roleManager.CreateRole("Worker");  // ex: cashier
            roleManager.CreateRole("Customer");
        }

        public static void CreateRole(this RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (roleManager.RoleExistsAsync(roleName).Result)
            {
                return;
            }

            var role = new IdentityRole
            {
                Name = roleName
            };

            var roleResult = roleManager.CreateAsync(role).Result;

            if (!roleResult.Succeeded)
            {
                throw new Exception("Error creating role");
            }
        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            //Admin
            string mail = "admin@gmail.com";
            if (userManager.FindByNameAsync(mail).Result == null)
            {
                var user = new ApplicationUser()
                {
                    Kennitala = "1700969999",
                    UserName = mail,
                    FirstName = "Gado",
                    LastName = "Admin",
                    BirthDate = new DateTime(1996, 09, 17),
                    Email = mail,
                    PhoneNumber = "7772181",
                    CreatedDate = DateTime.Now
                };

                IdentityResult result = userManager.CreateAsync(user, "Admin007!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }

            //Curstomers
            mail = "georgecosminmorosanu@gmail.com";
            if (userManager.FindByNameAsync(mail).Result == null)
            {
                var user = new ApplicationUser()
                {
                    Kennitala = "1700965269",
                    UserName = mail,
                    FirstName = "George",
                    LastName = "Morosanu",
                    BirthDate = new DateTime(1996, 09, 17),
                    Email = mail,
                    PhoneNumber = "0750000000",
                    CreatedDate = DateTime.Now
                };

                IdentityResult result = userManager.CreateAsync(user, "Customer007!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Customer").Wait();
                }
            }

        }
    }
}
