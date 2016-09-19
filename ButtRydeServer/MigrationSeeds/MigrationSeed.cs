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
            roleList.Add("SuperAdmin");
            roleList.Add("Sales");
            roleList.Add("Admin");
            roleList.Add("ContentProvider");
            roleList.Add("User");
            roleList.Add("ProductManager");
            roleList.Add("SalesManager");
            foreach (string role in roleList)
            {
                if (!roleManager.RoleExists(role)) {
                    roleManager.Create(new IdentityRole { Name = role });
                }
                
            }

            
            
            //if (roleManager.Roles.Count() == 0)
            //{
            //    roleManager.Create(new IdentityRole { Name = "SuperAdmin" });
            //    roleManager.Create(new IdentityRole { Name = "Sales" });
            //    roleManager.Create(new IdentityRole { Name = "Admin" });
            //    roleManager.Create(new IdentityRole { Name = "ContentProvider" });
            //    roleManager.Create(new IdentityRole { Name = "User" });
            //}

            // create SuperAdmin
            var superAdmin = new ApplicationUser()
            {
                UserName = "SuperAdmin",
                FirstName = "Eric",
                LastName = "Shih",
                Email = "eric.shih@advantech.com",
                EmailConfirmed = true,
                IsActive = true,
                RegisterDate = DateTime.Now.AddYears(-3)
            };

            userManager.Create(superAdmin, "MySuperP@ss1");

            superAdmin = userManager.FindByName("SuperAdmin");

            userManager.AddToRoles(superAdmin.Id, new string[] { "SuperAdmin", "Admin" });

            // create Admin
            var admin = new ApplicationUser()
            {
                UserName = "Admin",
                FirstName = "Eric",
                LastName = "Shih",
                Email = "ericshihtwn@hotmail.com",
                EmailConfirmed = true,
                IsActive = true,
                RegisterDate = DateTime.Now.AddYears(-3)
            };

            userManager.Create(admin, "MySuperP@ss1");

            admin = userManager.FindByName("Admin");

            userManager.AddToRoles(admin.Id, new string[] { "Admin" });


            // create ProductManager
            var alexLin = new ApplicationUser()
            {
                UserName = "alex.lin",
                FirstName = "Alex",
                LastName = "Lin",
                Email = "alex.lin@advantech.com",
                EmailConfirmed = true,
                IsActive = true,
                RegisterDate = DateTime.Now.AddYears(-3)
            };
            userManager.Create(alexLin, "MySuperP@ss1");
            alexLin = userManager.FindByName("alex.lin");
            userManager.AddToRoles(alexLin.Id, new string[] { "ProductManager" });

            // create Sales
            var first = "Fei";
            var last = "Khong";
            var appUser = new ApplicationUser()
            {
                UserName = first+"."+last,
                FirstName = first,
                LastName = last,
                Email = first+"."+last+"@advantech.com",
                EmailConfirmed = true,
                IsActive = true,
                RegisterDate = DateTime.Now.AddYears(-3)
            };
            userManager.Create(appUser, "MySuperP@ss1");
            appUser = userManager.FindByName(first+"."+last);
            userManager.AddToRoles(appUser.Id, new string[] { "SalesManager" });

            // create User
            first = "User";
            last = "Nobody";
            appUser = new ApplicationUser()
            {
                UserName = first + "." + last,
                FirstName = first,
                LastName = last,
                Email = first + "." + last + "@advantech.com",
                EmailConfirmed = true,
                IsActive = true,
                RegisterDate = DateTime.Now.AddYears(-3)
            };
            userManager.Create(appUser, "MySuperP@ss1");
            appUser = userManager.FindByName(first + "." + last);

            
            // create ContentProvider
            var contentProvider = new ApplicationUser()
            {
                UserName = "ContentProvider",
                FirstName = "Eric",
                LastName = "Shih",
                Email = "shih.eric@hotmail.com",
                EmailConfirmed = true,
                IsActive = false,
                RegisterDate = DateTime.Now.AddYears(-3)
            };

            userManager.Create(contentProvider, "MySuperP@ss1");

            contentProvider = userManager.FindByName("ContentProvider");

            userManager.AddToRoles(contentProvider.Id, new string[] { "ContentProvider" });
        }
    }
}