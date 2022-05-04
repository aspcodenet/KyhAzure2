using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mvc1VaccinDemo.Data;

namespace SharedThings.Data
{
    public class DataInitializer
    {
        public static void SeedData(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            dbContext.Database.Migrate();

            SeedRoles(dbContext);
            SeedUsers(userManager);

            SeedSuppliers(dbContext);
            SeedVaccins(dbContext); //Color, Län , kommuner
            SeedMyndighet(dbContext);
            SeedVaccinationsFaser(dbContext);
            SeedPersoner(dbContext);
        }


        private static void SeedUsers(UserManager<IdentityUser> userManager)
        {
            AddUserIfNotExists(userManager, "admin@hejhopp.nl", "Hejsan123#", new string[]{"Admin"});
            AddUserIfNotExists(userManager, "nurse1@hejhopp.nl", "Hejsan123#", new string[] { "Nurse" });
            AddUserIfNotExists(userManager, "nurse2@hejhopp.nl", "Hejsan123#", new string[] { "Nurse", 
                "Admin" });
        }

        private static void AddUserIfNotExists(UserManager<IdentityUser> userManager, 
            string userName, string password, string[] roles)
        {
            if (userManager.FindByEmailAsync(userName).Result != null) return;

            var user = new IdentityUser
            {
                UserName = userName,
                Email = userName,
                EmailConfirmed = true
            };
            var result = userManager.CreateAsync(user, password).Result;
            var r = userManager.AddToRolesAsync(user, roles).Result;
        }

        private static void SeedMyndighet(ApplicationDbContext dbContext)
        {
            var role = dbContext.Myndigheter.FirstOrDefault(r => r.Name == "FHM");
            if (role == null)
            {
                dbContext.Myndigheter.Add(new Myndighet() { Name = "FHM" });
            }
            role = dbContext.Myndigheter.FirstOrDefault(r => r.Name == "Socialstyrelsen");
            if (role == null)
            {
                dbContext.Myndigheter.Add(new Myndighet() { Name = "Socialstyrelsen" });
            }
            role = dbContext.Myndigheter.FirstOrDefault(r => r.Name == "Regeringen");
            if (role == null)
            {
                dbContext.Myndigheter.Add(new Myndighet() { Name = "Regeringen" });
            }
        }


        private static void SeedRoles(ApplicationDbContext dbContext)
        {
            var role = dbContext.Roles.FirstOrDefault(r => r.Name == "Admin");
            if (role == null)
            {
                dbContext.Roles.Add(new IdentityRole { Name = "Admin", NormalizedName = "Admin" });
            }
            role = dbContext.Roles.FirstOrDefault(r => r.Name == "Nurse");
            if (role == null)
            {
                dbContext.Roles.Add(new IdentityRole { Name = "Nurse", NormalizedName = "Nurse" });
            }
            dbContext.SaveChanges();

        }

        private static void SeedPersoner(ApplicationDbContext dbContext)
        {
            var person = dbContext.Personer.FirstOrDefault(r => r.Name == "Stefan Holmberg");
            if (person == null)
                dbContext.Personer.Add(new Person
                {
                    Name = "Stefan Holmberg",City="Teststad", PersonalNumber = "19720803-1111",
                    EmailAddress = "stefan@stefan.se",PostalCode = 11122, PreliminaryNextVaccinDate = null,
                    StreetAddress = "Testgatan 12b", VaccineringsFas = dbContext.VaccineringsFaser.First(r=>r.Name == "Fas 4")
                });
            person = dbContext.Personer.FirstOrDefault(r => r.Name == "Oliver Holmberg");
            if (person == null)
                dbContext.Personer.Add(new Person
                {
                    Name = "Oliver Holmberg",
                    City="Teststad", PersonalNumber = "20080528-1111",
                    EmailAddress = "oliver@oliver.se",PostalCode = 11133, PreliminaryNextVaccinDate = null,
                    StreetAddress = "Testgatan 11b", VaccineringsFas = dbContext.VaccineringsFaser.First(r=>r.Name == "Ingen")
                });
            dbContext.SaveChanges();
        }

        private static void SeedVaccinationsFaser(ApplicationDbContext dbContext)
        {
            var riskGrupp = dbContext.VaccineringsFaser.FirstOrDefault(r => r.Name == "Ingen");
            if (riskGrupp == null)
                dbContext.VaccineringsFaser.Add(new VaccineringsFas { Name = "Ingen" });

            riskGrupp = dbContext.VaccineringsFaser.FirstOrDefault(r => r.Name == "Fas 1");
            if(riskGrupp == null)
                dbContext.VaccineringsFaser.Add(new VaccineringsFas { Name = "Fas 1" });
            
            riskGrupp = dbContext.VaccineringsFaser.FirstOrDefault(r => r.Name == "Fas 2");
            if (riskGrupp == null)
                dbContext.VaccineringsFaser.Add(new VaccineringsFas { Name = "Fas 2" });

            riskGrupp = dbContext.VaccineringsFaser.FirstOrDefault(r => r.Name == "Fas 3");
            if (riskGrupp == null)
                dbContext.VaccineringsFaser.Add(new VaccineringsFas { Name = "Fas 3" });

            riskGrupp = dbContext.VaccineringsFaser.FirstOrDefault(r => r.Name == "Fas 4");
            if (riskGrupp == null)
                dbContext.VaccineringsFaser.Add(new VaccineringsFas { Name = "Fas 4" });

            dbContext.SaveChanges();
        }

        private static void SeedSuppliers(ApplicationDbContext dbContext)
        {
            var supplier = dbContext.Suppliers.FirstOrDefault(r => r.Name == "AstraZeneca och Oxford University");
            if (supplier == null)
                dbContext.Suppliers.Add(new Supplier {Name = "AstraZeneca och Oxford University" });

            supplier = dbContext.Suppliers.FirstOrDefault(r => r.Name == "Pfizer/BioNTech");
            if (supplier == null)
                dbContext.Suppliers.Add(new Supplier { Name = "Pfizer/BioNTech" });

            supplier = dbContext.Suppliers.FirstOrDefault(r => r.Name == "Moderna");
            if (supplier == null)
                dbContext.Suppliers.Add(new Supplier { Name = "Moderna" });

            dbContext.SaveChanges();

        }

        private static void SeedVaccins(ApplicationDbContext dbContext)
        {
            var vaccin = dbContext.Vacciner.FirstOrDefault(r => r.Namn == "COVID-19 Vaccine AstraZeneca");
            if (vaccin == null)
                dbContext.Vacciner.Add( new Vaccin
                {
                    Supplier = dbContext.Suppliers.First(r=>r.Name ==  "AstraZeneca och Oxford University"),
                    Namn = "COVID-19 Vaccine AstraZeneca",
                    EuOkStatus = null,
                    Type = Vaccin.VaccinType.Vector
                } );
            else
            {
                vaccin.Supplier = dbContext.Suppliers.First(r => r.Name == "AstraZeneca och Oxford University");
            }

            vaccin = dbContext.Vacciner.FirstOrDefault(r => r.Namn == "Comirnaty");
            if (vaccin == null)
                dbContext.Vacciner.Add(new Vaccin
                {
                    Supplier = dbContext.Suppliers.First(r => r.Name == "Pfizer/BioNTech"),
                    Namn = "Comirnaty",
                    EuOkStatus = new DateTime(2020,12,21),
                    Type = Vaccin.VaccinType.mRNA
                });
            else
            {
                vaccin.Supplier = dbContext.Suppliers.First(r => r.Name == "Pfizer/BioNTech");
            }

            vaccin = dbContext.Vacciner.FirstOrDefault(r => r.Namn == "Covid-19 Vaccine Moderna");
            if (vaccin == null)
                dbContext.Vacciner.Add(new Vaccin
                {
                    Supplier = dbContext.Suppliers.First(r => r.Name == "Moderna"),
                    Namn = "Covid-19 Vaccine Moderna",
                    EuOkStatus = new DateTime(2021, 1, 6),
                    Type = Vaccin.VaccinType.mRNA
                });
            else
            {
                vaccin.Supplier = dbContext.Suppliers.First(r => r.Name == "Moderna");
            }

            dbContext.SaveChanges();
        }
    }
}
