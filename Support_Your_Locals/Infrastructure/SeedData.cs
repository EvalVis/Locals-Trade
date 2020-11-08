using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Support_Your_Locals.Models;

namespace Support_Your_Locals.Infrastructure
{
    public class SeedData
    {

        public static void EnsurePopulated(IApplicationBuilder app)
        {
            ServiceDbContext context = app.ApplicationServices.CreateScope().ServiceProvider
                .GetRequiredService<ServiceDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            if (!context.Users.Any())
            {
                context.Users.AddRange(CreateTestMaterial(context));
                context.SaveChanges();
            }
        }

        private static User CreateUser(string name, string surname, int year, int month, int day, string email)
        {
            return new User {Name = name, Surname = surname, BirthDate = new DateTime(year, month, day), Email = email};
        }

        private static Business CreateBusiness(string product, string description, string longitude, string latitude, string phone, string header)
        {
            return new Business
            {
                Product = product,
                Description = description,
                Longitude = longitude,
                Latitude = latitude,
                PhoneNumber = phone,
                Header = header
            };
        }

        private static TimeSheet CreateWorkday(int fh, int fm, int th, int tm, int day)
        {
            return new TimeSheet
            {
                From = new DateTime(1999, 12, 06, fh, fm, 00),
                To = new DateTime(1999, 12, 06, th, tm, 00),
                Weekday = day
            };
        }

        private static void AddBusinessOwner(User owner, Business business)
        {
            owner.Businesses.Add(business);
        }

        private static void AddBusinessWorkday(Business business, TimeSheet workday)
        {
            business.Workdays.Add(workday);
        }

        private static User[] CreateTestMaterial(ServiceDbContext context)
        {
            User u1 = CreateUser("Evaldas", "Visockas", 1999, 12, 06, "vievaldas@gmail.com");
            User u2 = CreateUser("Šarūnas", "Teisutis", 1996, 04, 02, "teisutis@gmail.com");
            User u3 = CreateUser("Kazys", "Bruolė", 1950, 01, 02, "bruoleparpuole@gmail.com");
            User u4 = CreateUser("Birutė", "Išdykėlė", 2001, 01, 15, "birute@gmail.com");
            User u5 = CreateUser("Lapinas", "Baronas", 2005, 10, 15, "gandras@gmail.com");
            User u6 = CreateUser("Konradas", "Rado", 2018, 06, 05, "neturiu@gmail.com");
            User u7 = CreateUser("Vaiva", "Vaivorykštė", 1961, 07, 02, "vaivaesu@gmail.com");
            User u8 = CreateUser("Vaidas", "Grinius", 2004, 03, 06, "vaidas2004@gmail.com");
            User u9 = CreateUser("Laputė", "Laisvuolė", 1930, 02, 15, "laisvoji.lape@gmail.com");
            User u10 = CreateUser("Kazys", "Keistuolis", 1994, 04, 12, "1994.kazys.04@gmail.com");
            User u11 = CreateUser("Rasa", "Išplaukusi", 2000, 01, 03, "rasa.ryto@gmail.com");

            Business b1 = CreateBusiness("Traktoriai", "Traktorių parduotuvė", "25.274633", "54.699603", "+37064575620","10% nuolaida žieminėms padangoms");
            Business b2 = CreateBusiness("Automobiliai", "Daužti automobiliai už prieinamą kainą", "25.268811", "54.693882", "+37064215675", "Įsigykite automobilius be rėmo, greit neliks");
            Business b3 = CreateBusiness("Saldainiai", "Įvairūs", "25.269173", "54.688853", "+37064215201", "Saldainių parduotuvė");
            Business b4 = CreateBusiness("Išdaigos", "Smagūs niekučiai", "25.281194", "54.681308", "+37064512012", "Išpardavimas išdaigininkams");
            Business b5 = CreateBusiness("Automobilių remontas", "Nuo 7:00 iki 8:00 ryto remontuoju už dyką", "25.214340", "54.379052", "+37064512456", "Automobilių remontas");
            Business b6 = CreateBusiness("Nusirašinėjimas", "Rašau referatus, rašinius ir t.t.", "25.391008", "54.290225", "+37064752660", "Nenorit daryt mokyklinio projekto? Susisiekit, padarysiu už jus!");
            Business b7 = CreateBusiness("Piešiniai", "Galiu daryti portretus, natiurmortą, peisažą ir t.t.", "25.359637", "54.306365", "+37064233450","Piešiniai už (beveik) dyką!");
            Business b8 = CreateBusiness("Dėvėtos kojinės", "Paprastai labai kvepia", "25.304485", "54.630629", "+37064512478", "Yra ir smirdančių");
            Business b9 = CreateBusiness("Labdara seneliams", "Paaukokite senelių namuose gyvenantiems senjorams", "24.489809", "55.565052", "+37062199630", "Padarykite gerą darbą vieno mygtuko paspaudimu!");

            TimeSheet t1 = CreateWorkday(10, 0, 11, 0, 1);
            TimeSheet t2 = CreateWorkday(10, 0, 16, 0, 3);
            TimeSheet t3 = CreateWorkday(10, 0, 12, 0, 2);
            TimeSheet t4 = CreateWorkday(8, 0, 12, 0, 1);
            TimeSheet t5 = CreateWorkday(8, 0, 11, 0, 2);
            TimeSheet t6 = CreateWorkday(8, 0, 11, 0, 6);
            TimeSheet t7 = CreateWorkday(7, 0, 9, 0, 4);
            TimeSheet t8 = CreateWorkday(7, 0, 16, 30, 1);
            TimeSheet t9 = CreateWorkday(7, 0, 16, 30, 2);
            TimeSheet t10 = CreateWorkday(7, 0, 16, 30, 3);
            TimeSheet t11 = CreateWorkday(7, 0, 16, 30, 4);
            TimeSheet t12 = CreateWorkday(7, 0, 16, 30, 5);
            TimeSheet t13 = CreateWorkday(1, 0, 23, 30, 7);

            b1.Workdays.Add(t1);
            b1.Workdays.Add(t2);
            b2.Workdays.Add(t3);
            b2.Workdays.Add(t4);
            b3.Workdays.Add(t5);
            b4.Workdays.Add(t6);
            b5.Workdays.Add(t7);
            b5.Workdays.Add(t8);
            b8.Workdays.Add(t9);
            b8.Workdays.Add(t10);
            b8.Workdays.Add(t11);
            b8.Workdays.Add(t12);
            b8.Workdays.Add(t13);

            u1.Businesses.Add(b1);
            u1.Businesses.Add(b2);
            u2.Businesses.Add(b3);
            u2.Businesses.Add(b4);
            u3.Businesses.Add(b5);
            u3.Businesses.Add(b6);
            u4.Businesses.Add(b7);
            u5.Businesses.Add(b8);
            u6.Businesses.Add(b9);
            return new User[] {u1, u2, u3, u4, u5, u6, u7, u8, u9, u10, u11};
        }

    }
}
