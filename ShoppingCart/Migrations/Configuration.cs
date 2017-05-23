namespace ShoppingCart.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ShoppingCart.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ShoppingCart.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(

            //TODO: Add code to allow us to create a new role
            //Step 1. Spin up an instance of the role manager
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            //Step 2. Look for an existing role wiht the name "Admin" and if one is not found, create one.
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            //TODO: Add code to create new user
            //Step 1. Spin up an instance of the user manager class
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            //TODO: Add code to assign a user to a role
            //Step 2. Look for an existing user with the email address equal to your email address
            if(!context.Users.Any(u => u.Email == "jtisdale1977@gmail.com"))
            {
                userManager.Create(
                    new ApplicationUser
                    {
                        //  This method will be called after migrating to the latest version.

                        //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
                        //  to avoid creating duplicate seed data. E.g.
                        //
                        //    context.People.AddOrUpdate

                        UserName = "jtisdale1977@gmail.com",
                        Email = "jtisdale1977@gmail.com",
                        FirstName = "Justin",
                        LastName = "Tisdale",
                        DisplayName = "Tiz",
                    }
                    , "Abc123!");
            }
            //Goes out to the AplicationUser table
            //Finds the record with email = jtosdale1977@gmail.com 
            //assigns that record's ID to the UserId variable
            var userId = userManager.FindByEmail("jtisdale1977@gmail.com").Id;

            //assigns the user identified by the user variable (jtisdale1977@gmail.com) to the admin role that was just created above
            userManager.AddToRole(userId, "Admin");

        }
    }
}
