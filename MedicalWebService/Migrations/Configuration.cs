namespace MedicalWebService.Migrations
{
    using Enums;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Threading.Tasks;

    internal sealed class Configuration : DbMigrationsConfiguration<MedicalWebService.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled =true;
        }

        protected override void Seed(MedicalWebService.Models.ApplicationDbContext context)
        {

            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);


            //Roles
            string[] roles = new string[] { RoleTypes.DOCTOR, RoleTypes.NURSE};
            foreach (string role in roles)
            {
            
                if (!context.Roles.Any(r => r.Name == role))
                {
                    roleManager.Create(new IdentityRole(role));
                }

            }
            //Users 
            List<ApplicationUser> users = new List<ApplicationUser>()
            {
                new ApplicationUser() {
                    UserName = "doctor",
                    Email ="doctor@example.com",
                    EmailConfirmed = true,
                    Password="p@ssw0rd",
                    LockoutEnabled = false,
                    TwoFactorEnabled = false,
                    LastLogin = DateTime.Now,
                    PasswordHash = userManager.PasswordHasher.HashPassword("p@ssw0rd")
                },
                 new ApplicationUser() {
                    UserName = "nurse",
                    Email ="nurse@example.com",
                    EmailConfirmed = true,
                    Password="p@ssw0rd",
                    LockoutEnabled = false,
                    TwoFactorEnabled = false,
                    LastLogin= DateTime.Now,
                    
                    PasswordHash = userManager.PasswordHasher.HashPassword("p@ssw0rd")
                }
            };

          
            foreach (ApplicationUser user in users)
            {

                if (!context.Users.Any(u => u.UserName == user.UserName))
                {
                    userManager.Create(user);
                    userManager.AddToRole(user.Id,char.ToUpper(user.UserName.ToString()[0])+user.UserName.Substring(1));
                }

            }
            Client client = context.Clients.Where(a => a.ClientName == "medical").SingleOrDefault();
            if (client==null)
            {
                context.Clients.Add(new Client
                {
                    ClientName = "medical",
                    ClientSecret="password",
                    ClientRedirectURL= "http://localhost:49769/"
                });
                context.SaveChanges();
            }
        }
    }

}
