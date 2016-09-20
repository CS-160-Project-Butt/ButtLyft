using AASC.Partner.API.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AASC.Partner.API.MigrationSeeds
{
    public class MigrationSeed
    {
        public static void Seed()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            List<String> roleList = new List<String>();
            roleList.Add("Rider");
            roleList.Add("Driver");
            roleList.Add("Admin");
            foreach (string role in roleList)
            {
                if (!roleManager.RoleExists(role)) {
                    roleManager.Create(new IdentityRole { Name = role });
                }
                
            }

            //create driver
            var first = "FrankButts";
            var last = "ButtlerDriver";
            var appUser = new ApplicationUser()
            {
                UserName = first + "." + last,
                FirstName = first,
                LastName = last,
                Email = first + "." + last + "@sjsu.edu",
                EmailConfirmed = true,
                IsActive = true,
                RegisterDate = DateTime.Now.AddYears(-3)
            };
            userManager.Create(appUser, "frankbutt");
            appUser = userManager.FindByName(first + "." + last);
            userManager.AddToRoles(appUser.Id, new string[] { "Driver" });


            // create Rider
            first = "Frank";
            last = "Butt";
            appUser = new ApplicationUser()
            {
                UserName = first + "." + last,
                FirstName = first,
                LastName = last,
                Email = first + "." + last + "@sjsu.edu",
                EmailConfirmed = true,
                IsActive = true,
                RegisterDate = DateTime.Now.AddYears(-3)
            };
            userManager.Create(appUser, "frankbutt");
            appUser = userManager.FindByName(first + "." + last);
            userManager.AddToRoles(appUser.Id, new string[] { "Rider" });

            
        }
    }
}