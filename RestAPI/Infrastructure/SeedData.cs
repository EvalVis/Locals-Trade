using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestAPI.Cryptography;
using RestAPI.Models;

namespace RestAPI.Infrastructure
{
    public class SeedData
    {

        public static void EnsurePopulated(IApplicationBuilder app, HashCalculator hashCalculator)
        {
            ServiceDbContext context = app.ApplicationServices.CreateScope().ServiceProvider
                .GetRequiredService<ServiceDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            if (!context.Users.Any())
            {
                context.Users.AddRange(CreateTestMaterial(context, hashCalculator));
                context.SaveChanges();
            }
        }

        private static User CreateUser(string name, string surname, int year, int month, int day, string email, string passhash)
        {
            return new User { Name = name, Surname = surname, BirthDate = new DateTime(year, month, day), Email = email, Passhash = passhash };
        }

        private static Business CreateBusiness(string product, string description, string longitude, string latitude, string phone, string header)
        {
            return new Business
            {
                Description = description,
                Longitude = longitude,
                Latitude = latitude,
                PhoneNumber = phone,
                Header = header
            };
        }

        private static Product CreateProduct(string name, decimal unitPrice, string unit, string comment)
        {
            return new Product { Name = name, PricePerUnit = unitPrice, Unit = unit, Comment = comment };
        }

        private static Order CreateOrder(int amount, string address, string comment, DateTime dateAdded, long userId, long productId)
        {
            return new Order { Amount = amount, Address = address, Comment = comment, DateAdded = dateAdded, UserId = userId, ProductId = productId };
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

        private static User[] CreateTestMaterial(ServiceDbContext context, HashCalculator hashCalculator)
        {
            User u1 = CreateUser("Evaldas", "Visockas", 1999, 12, 06, "vievaldas@gmail.com", hashCalculator.PassHash("lydeka"));
            User u2 = CreateUser("Šarūnas", "Teisutis", 1996, 04, 02, "testinis1422@gmail.com", hashCalculator.PassHash("tigras"));
            User u3 = CreateUser("Kazys", "Bruolė", 1950, 01, 02, "senovinis5478@gmail.com", hashCalculator.PassHash("katinas"));
            User u4 = CreateUser("Birutė", "Išdykėlė", 2001, 01, 15, "atsitiktinis3456@gmail.com", hashCalculator.PassHash("miau"));
            User u5 = CreateUser("Lapinas", "Baronas", 2005, 10, 15, "lapinas3456@gmail.com", hashCalculator.PassHash("uosis"));
            User u6 = CreateUser("Konradas", "Rado", 2018, 06, 05, "konradas5423@gmail.com", hashCalculator.PassHash("cimbaliukas"));
            User u7 = CreateUser("Vaiva", "Vaivorykštė", 1961, 07, 02, "vaiva7423@gmail.com", hashCalculator.PassHash("paprika"));
            User u8 = CreateUser("Vaidas", "Grinius", 2004, 03, 06, "vaidas12354672@gmail.com", hashCalculator.PassHash("lapinas"));
            User u9 = CreateUser("Laputė", "Laisvuolė", 1930, 02, 15, "lapute422068@gmail.com", hashCalculator.PassHash("kaziukas"));
            User u10 = CreateUser("Kazys", "Keistuolis", 1994, 04, 12, "keistuolis4122315@gmail.com", hashCalculator.PassHash("greitkelis"));
            User u11 = CreateUser("Rasa", "Išplaukusi", 2000, 01, 03, "isplaukusi423456@gmail.com", hashCalculator.PassHash("popkornas"));

            Business b1 = CreateBusiness("Traktoriai", "Traktorių parduotuvė", "25.274633", "54.699603", "+37064575620", "10% nuolaida žieminėms padangoms");
            Business b2 = CreateBusiness("Automobiliai", "Daužti automobiliai už prieinamą kainą", "25.268811", "54.693882", "+37064215675", "Įsigykite automobilius be rėmo, greit neliks");
            Business b3 = CreateBusiness("Saldainiai", "Įvairūs", "25.269173", "54.688853", "+37064215201", "Saldainių parduotuvė");
            Business b4 = CreateBusiness("Išdaigos", "Smagūs niekučiai", "25.281194", "54.681308", "+37064512012", "Išpardavimas išdaigininkams");
            Business b5 = CreateBusiness("Automobilių remontas", "Nuo 7:00 iki 8:00 ryto remontuoju už dyką", "25.214340", "54.379052", "+37064512456", "Automobilių remontas");
            Business b6 = CreateBusiness("Nusirašinėjimas", "Rašau referatus, rašinius ir t.t.", "25.391008", "54.290225", "+37064752660", "Nenorit daryt mokyklinio projekto? Susisiekit, padarysiu už jus!");
            Business b7 = CreateBusiness("Piešiniai", "Galiu daryti portretus, natiurmortą, peisažą ir t.t.", "25.359637", "54.306365", "+37064233450", "Piešiniai už (beveik) dyką!");
            Business b8 = CreateBusiness("Dėvėtos kojinės", "Paprastai labai kvepia", "25.304485", "54.630629", "+37064512478", "Yra ir smirdančių");
            Business b9 = CreateBusiness("Labdara seneliams", "Paaukokite senelių namuose gyvenantiems senjorams", "24.489809", "55.565052", "+37062199630", "Padarykite gerą darbą vieno mygtuko paspaudimu!");

            u1.Businesses.Add(b1);
            u1.Businesses.Add(b2);
            u2.Businesses.Add(b3);
            u2.Businesses.Add(b4);
            u3.Businesses.Add(b5);
            u3.Businesses.Add(b6);
            u4.Businesses.Add(b7);
            u5.Businesses.Add(b8);
            u6.Businesses.Add(b9);

            Product p1 = CreateProduct("Traktorius", 10000, "vienetas", "Liko tik trys.");
            Product p2 = CreateProduct("Daužtas BMW", 20000, "vienetas", "Jei netinka, galim dar padaužti prieš parduodant.");
            Product p3 = CreateProduct("Nissan", 10000, "vienetas", "Sena gera mašina.");
            Product p4 = CreateProduct("Saldainiai arbūzai", 3, "pakelis", "Tik močiutės pyragai skanesni.");
            Product p5 = CreateProduct("Sproginėjantys čiulpinukai", 2, "čiulpinukas", "Suvalgius keisti garsai pilve girdisi.");
            Product p6 = CreateProduct("Remonto paslauga", 350, "sutaisytas automobilis", "Paprastai sutaisai per valandą dvi.");
            Product p7 = CreateProduct("Kontrolinių atsakymai", 20, "lapas", "Pametus garantija neteikiama.");
            Product p8 = CreateProduct("Peisažas", 200, "piešinys",
                "Perkant du ar daugiau, taikoma 20% nuolaida bendrai sumai");
            Product p9 = CreateProduct("Natiurmortas", 500, "piešinys",
                "Perkant du ar daugiau, mokate 20% brangiau bendros sumos");
            Product p10 = CreateProduct("Kvepiančios kojinės", 25.73M, "viena kojinė", "Išnešiojo srities specialistas");
            Product p11 = CreateProduct("Pinigų aukojimas", 5, "kartas", "Jei norite, galite paaukoti ir daugiau.");

            b1.Products.Add(p1);
            b1.Products.Add(p2);
            b2.Products.Add(p3);
            b3.Products.Add(p4);
            b4.Products.Add(p5);
            b5.Products.Add(p6);
            b6.Products.Add(p7);
            b7.Products.Add(p8);
            b7.Products.Add(p9);
            b8.Products.Add(p10);
            b9.Products.Add(p11);

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

            return new User[] { u1, u2, u3, u4, u5, u6, u7, u8, u9, u10, u11 };
        }


        private static void AddOrders(ServiceDbContext context)
        {
            Order o1 = CreateOrder(10, "Aguonų g. 24", "Urgent", DateTime.Now.AddDays(-1), 1, 1);
            Order o2 = CreateOrder(5, "Lydos g. 5", "Please apply green colour", DateTime.Now, 1, 10);
            Order o3 = CreateOrder(7, "M. K. Čiurlionio g. 4-2", "It's for my aunt, so make it good", DateTime.Now.AddMinutes(-5), 1, 9);
            Order o4 = CreateOrder(1, "Tilto g. 27", "Please make the delivery price 50 euros or less", DateTime.Now.AddSeconds(35), 1, 1);
            Order o5 = CreateOrder(2, "A. Juozapavičiaus g. 2", "Not interested in 1 year old cars or older", DateTime.Now.AddSeconds(17), 1, 10);
            context.Orders.AddRange(new Order[] { o1, o2, o3, o4, o5 });
        }

    }
}
