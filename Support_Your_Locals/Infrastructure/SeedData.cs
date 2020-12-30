using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Support_Your_Locals.Cryptography;
using Support_Your_Locals.Models;

namespace Support_Your_Locals.Infrastructure
{
    public class SeedData
    {

        public static void EnsurePopulated(IApplicationBuilder app, HashCalculator hashCalculator, Imager imager)
        {
            ServiceDbContext context = app.ApplicationServices.CreateScope().ServiceProvider
                .GetRequiredService<ServiceDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            if (!context.Users.Any())
            {
                context.Users.AddRange(CreateTestMaterial(hashCalculator, imager));
                context.SaveChanges();
            }
        }

        private static User CreateUser(string name, string surname, int year, int month, int day, string email, string passhash)
        {
            return new User { Name = name, Surname = surname, BirthDate = new DateTime(year, month, day), Email = email, Passhash = passhash };
        }

        private static Business CreateBusiness(string description, string longitude, string latitude, string phone, string header, byte[] image)
        {
            return new Business
            {
                Description = description,
                Longitude = longitude,
                Latitude = latitude,
                PhoneNumber = phone,
                Header = header,
                PictureData = image
            };
        }

        private static Product CreateProduct(string name, decimal unitPrice, string unit, string comment)
        {
            return new Product { Name = name, PricePerUnit = unitPrice, Unit = unit, Comment = comment };
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

        private static User[] CreateTestMaterial(HashCalculator hashCalculator, Imager imager)
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

            Business b1 = CreateBusiness("Traktorių parduotuvė", "25.274633", "54.699603", "+37064575620", "10% nuolaida žieminėms padangoms", null);
            Business b2 = CreateBusiness("Daužti automobiliai už prieinamą kainą", "25.268811", "54.693882", "+37064215675", "Įsigykite automobilius be rėmo, greit neliks", null);
            Business b3 = CreateBusiness("Įvairūs", "25.269173", "54.688853", "+37064215201", "Saldainių parduotuvė", null);
            Business b4 = CreateBusiness("Smagūs niekučiai", "25.281194", "54.681308", "+37064512012", "Išpardavimas išdaigininkams", null);
            Business b5 = CreateBusiness("Nuo 7:00 iki 8:00 ryto remontuoju už dyką", "25.214340", "54.379052", "+37064512456", "Automobilių remontas", null);
            Business b6 = CreateBusiness("Rašau referatus, rašinius ir t.t.", "25.391008", "54.290225", "+37064752660", "Nenorit daryt mokyklinio projekto? Susisiekit, padarysiu už jus!", null);
            Business b7 = CreateBusiness("Galiu daryti portretus, natiurmortą, peisažą ir t.t.", "25.359637", "54.306365", "+37064233450", "Piešiniai už (beveik) dyką!", null);
            Business b8 = CreateBusiness("Paprastai labai kvepia", "25.304485", "54.630629", "+37064512478", "Yra ir smirdančių", null);
            Business b9 = CreateBusiness("Paaukokite senelių namuose gyvenantiems senjorams", "24.489809", "55.565052", "+37062199630", "Padarykite gerą darbą vieno mygtuko paspaudimu!", null);

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

            Business b10 = CreateBusiness("Selling some kind of wild berries", "25.235131", "54.758748", "+37062199630", "Wild berries", imager.ByteMaker("10.jpg"));
            Product p10_0 = CreateProduct("Blackberry", 15, "1 Kilo", "Fresh forest blackberries");
            Product p10_1 = CreateProduct("Fragaria", 8, "1 Kilo", "Sweet, medium sized fragarias");
            TimeSheet t10_0 = CreateWorkday(8, 0, 18, 0, 3);

            u6.Businesses.Add(b10);
            b10.Products.Add(p10_0);
            b10.Products.Add(p10_1);
            b10.Workdays.Add(t10_0);

            Business b11 = CreateBusiness("Selling berries from my garden", "25.266962", "54.792729", "+37062199630", "Berries", imager.ByteMaker("11.jpg"));
            Product p11_0 = CreateProduct("Strawberry", 4, "1 Kilo", "Frozen strawberries");
            Product p11_1 = CreateProduct("Raspberry", 7, "1 Kilo", "Juicy raspberries");
            TimeSheet t11_0 = CreateWorkday(10, 30, 17, 0, 2);
            TimeSheet t11_1 = CreateWorkday(8, 45, 18, 30, 4);


            u6.Businesses.Add(b11);
            b11.Products.Add(p11_0);
            b11.Products.Add(p11_1);
            b11.Workdays.Add(t11_0);
            b11.Workdays.Add(t11_1);

            Business b12 = CreateBusiness("I can weld, electrical work", "25.298652", "54.783504", "+37062199111", "Services", imager.ByteMaker("12.jpg"));
            Product p12_0 = CreateProduct("Welding", 20, "1 hour", "Can weld metal things");
            Product p12_1 = CreateProduct("Electrical work", 17, "1 hour", "All that connected to electrical work in home");
            TimeSheet t12_0 = CreateWorkday(8, 30, 22, 0, 1);

            u10.Businesses.Add(b12);
            b12.Products.Add(p12_0);
            b12.Products.Add(p12_1);
            b12.Workdays.Add(t12_0);

            Business b13 = CreateBusiness("I can plow", "25.298652", "54.783504", "+37062199111", "Farming work", imager.ByteMaker("13.jpg"));
            Product p13_0 = CreateProduct("Plowing", 30, "1 ar", "Plow your ground");
            TimeSheet t13_0 = CreateWorkday(8, 30, 22, 0, 1);

            u10.Businesses.Add(b13);
            b13.Products.Add(p13_0);
            b13.Workdays.Add(t13_0);

            Business b14 = CreateBusiness("You can order clothes to sew", "25.277352", "54.737149", "+37062199222", "Sewing", imager.ByteMaker("14.jpg"));
            Product p14_0 = CreateProduct("Sewing", 18, "1 cm2", "Order unique clothes");
            TimeSheet t14_0 = CreateWorkday(8, 30, 22, 0, 1);

            u9.Businesses.Add(b14);
            b14.Products.Add(p14_0);
            b14.Workdays.Add(t14_0);

            Business b15 = CreateBusiness("You can order knited clothes", "25.277352", "54.737149", "+37062199222", "Knitting", imager.ByteMaker("15.jpg"));
            Product p15_0 = CreateProduct("Knitting", 28, "1 cm2", "Order unique clothes");
            TimeSheet t15_0 = CreateWorkday(8, 30, 22, 0, 1);

            u9.Businesses.Add(b15);
            b15.Products.Add(p15_0);
            b15.Workdays.Add(t15_0);

            Business b16 = CreateBusiness("Selling used clothes for symbolic price", "25.226498", "54.708795", "+37062199222", "Clothes", imager.ByteMaker("16.jpg"));
            Product p16_0 = CreateProduct("Clothes", 1, "1 Clothe", "I can give some discount if you buy more");
            TimeSheet t16_0 = CreateWorkday(8, 30, 22, 0, 1);

            u8.Businesses.Add(b16);
            b16.Products.Add(p16_0);
            b16.Workdays.Add(t16_0);

            Business b17 = CreateBusiness("Selling home made dairy products", "25.506973", "54.676054", "+37062199333", "Dairy", imager.ByteMaker("17.jpg"));
            Product p17_0 = CreateProduct("Milk", 1, "1 Liter", "Fresh cow milk");
            Product p17_1 = CreateProduct("Curd", 1, "200 Grams", "Loose curd");
            TimeSheet t17_0 = CreateWorkday(8, 30, 12, 0, 1);
            TimeSheet t17_1 = CreateWorkday(8, 30, 12, 0, 2);
            TimeSheet t17_2 = CreateWorkday(8, 30, 12, 0, 3);
            TimeSheet t17_3 = CreateWorkday(8, 30, 12, 0, 4);
            TimeSheet t17_4 = CreateWorkday(8, 30, 12, 0, 5);
            TimeSheet t17_5 = CreateWorkday(8, 30, 12, 0, 6);
            TimeSheet t17_6 = CreateWorkday(8, 30, 12, 0, 7);


            u7.Businesses.Add(b17);
            b17.Products.Add(p17_0);
            b17.Products.Add(p17_1);
            b17.Workdays.Add(t17_0);
            b17.Workdays.Add(t17_1);
            b17.Workdays.Add(t17_2);
            b17.Workdays.Add(t17_3);
            b17.Workdays.Add(t17_4);
            b17.Workdays.Add(t17_5);
            b17.Workdays.Add(t17_6);

            Business b18 = CreateBusiness("Selling chicken eggs", "25.326122", "54.638121", "+37062199222", "Eggs", imager.ByteMaker("18.jpg"));
            Product p18_0 = CreateProduct("Chicken eggs", 3, "10 eggs", "Chickens were feeded without compound feedingstuffs");
            TimeSheet t18_0 = CreateWorkday(8, 30, 22, 0, 1);

            u8.Businesses.Add(b18);
            b18.Products.Add(p18_0);
            b18.Workdays.Add(t18_0);

            Business b19 = CreateBusiness("Selling home made goat dairy products", "25.506973", "54.676054", "+37062199333", "Dairy", imager.ByteMaker("19.jpg"));
            Product p19_0 = CreateProduct("Milk", 4, "1 Liter", "Fresh goat milk");
            Product p19_1 = CreateProduct("Cheese", 5, "200 Grams", "Goat cheese. Good with bread");
            TimeSheet t19_0 = CreateWorkday(9, 30, 12, 0, 1);
            TimeSheet t19_1 = CreateWorkday(9, 30, 12, 0, 2);
            TimeSheet t19_2 = CreateWorkday(9, 30, 12, 0, 3);
            TimeSheet t19_3 = CreateWorkday(9, 30, 12, 0, 4);
            TimeSheet t19_4 = CreateWorkday(9, 30, 12, 0, 5);
            TimeSheet t19_5 = CreateWorkday(9, 30, 12, 0, 6);
            TimeSheet t19_6 = CreateWorkday(9, 30, 12, 0, 7);


            u8.Businesses.Add(b19);
            b19.Products.Add(p19_0);
            b19.Products.Add(p19_1);
            b19.Workdays.Add(t19_0);
            b19.Workdays.Add(t19_1);
            b19.Workdays.Add(t19_2);
            b19.Workdays.Add(t19_3);
            b19.Workdays.Add(t19_4);
            b19.Workdays.Add(t19_5);
            b19.Workdays.Add(t19_6);

            Business b20 = CreateBusiness("Selling home made bread from my own wheat", "25.301730", "54.636929", "+37062199333", "Bread", imager.ByteMaker("12.jpg"));
            Product p20_0 = CreateProduct("Bread", 3, "500 Grams", "Rectangle shaped bread");
            TimeSheet t20_0 = CreateWorkday(8, 30, 22, 0, 1);

            u8.Businesses.Add(b20);
            b20.Products.Add(p20_0);
            b20.Workdays.Add(t20_0);

            Business b21 = CreateBusiness("Selling 1 year old goat", "25.301730", "54.636929", "+37062199333", "Goat", imager.ByteMaker("21.jfif"));
            Product p21_0 = CreateProduct("Goat", 50, "1 animal", "Gives 3liters of milk");
            TimeSheet t21_0 = CreateWorkday(8, 30, 22, 0, 1);

            u8.Businesses.Add(b21);
            b21.Products.Add(p21_0);
            b21.Workdays.Add(t21_0);

            Business b22 = CreateBusiness("Giving cat for symbolic price", "25.262359", "54.639473", "+37062199331", "Cat", null);
            Product p22 = CreateProduct("Cat", 1, "1 animal", "Small cat");
            TimeSheet t22 = CreateWorkday(8, 30, 22, 0, 1);

            u1.Businesses.Add(b22);
            b22.Products.Add(p22);
            b22.Workdays.Add(t22);

            Business b23 = CreateBusiness("Selling 1 year German Shepherd", "25.205261", "54.665214", "+37062199332", "Dog", null);
            Product p23 = CreateProduct("German Shepherd", 150, "1 animal", "Needs new home");
            TimeSheet t23 = CreateWorkday(8, 30, 22, 0, 1);

            u2.Businesses.Add(b23);
            b23.Products.Add(p23);
            b23.Workdays.Add(t23);

            Business b24 = CreateBusiness("Selling 1.5 year old green lizard", "25.249167", "54.708795", "+37062199330", "Lizard", null);
            Product p24_0 = CreateProduct("Lizard", 150, "1 animal", "It is 1.23 meters long");
            TimeSheet t24_0 = CreateWorkday(8, 30, 22, 0, 1);

            u3.Businesses.Add(b24);
            b24.Products.Add(p24_0);
            b24.Workdays.Add(t24_0);

            Business b25 = CreateBusiness("Selling newborn white guinea pigs", "25.301730", "54.636929", "+37062199334", "Guinea pig", null);
            Product p25_0 = CreateProduct("Guinea pig", 50, "1 animal", "Newborn Guinea pigs");

            u4.Businesses.Add(b25);
            b25.Products.Add(p25_0);
            b25.Workdays.Add(t21_0);

            Business b26 = CreateBusiness("Fixing your phones", "25.309975", "54.706812", "+37062199355", "Repair job", null);
            Product p26_0 = CreateProduct("Broken glass", 100, "1 screen", "Price depends on selected phone");

            u4.Businesses.Add(b26);
            b26.Products.Add(p26_0);
            b26.Workdays.Add(t21_0);


            Business b27 = CreateBusiness("Fixing your watches", "25.309975", "54.706812", "+37062199355", "Repair job", null);
            Product p27_0 = CreateProduct("Fix watch", 100, "unit", "Price depends by work");

            u4.Businesses.Add(b27);
            b27.Products.Add(p27_0);
            b27.Workdays.Add(t21_0);


            Business b28 = CreateBusiness("Fixing your laptops", "25.300356", "54.678635", "+37062199356", "Repair job", null);
            Product p28_0 = CreateProduct("Laptop", 200, "unit", "Price depends on work dificulty");

            u5.Businesses.Add(b28);
            b28.Products.Add(p28_0);
            b28.Workdays.Add(t21_0);


            Business b29 = CreateBusiness("Fast applying screen protecting glass", "25.257756", "54.679627", "+37062199356", "Repair job", null);
            Product p29_0 = CreateProduct("Apply job", 5, "1 glass", "Applying protective glass on your phone in 2 minutes. Dont forget to bring your glass.");

            u5.Businesses.Add(b29);
            b29.Products.Add(p29_0);
            b29.Workdays.Add(t21_0);


            Business b30 = CreateBusiness("Fast changing car tires", "25.253633", "54.711374", "+37062199374", "Repair job", null);
            Product p30_0 = CreateProduct("Tire installation", 40, "1 tire", "Price depends on selected tire");

            u5.Businesses.Add(b30);
            b30.Products.Add(p30_0);
            b30.Workdays.Add(t21_0);


            Business b31 = CreateBusiness("Fast changing engine oil", "25.228210", "54.702249", "+37062199355", "Repair job", null);
            Product p31_0 = CreateProduct("Oil change", 30, "one service", "Price includes only work");

            u6.Businesses.Add(b31);
            b31.Products.Add(p31_0);
            b31.Workdays.Add(t21_0);


            Business b32 = CreateBusiness("Fixing your phones", "25.309975", "54.706812", "+37062199358", "Eggs", null);
            Product p32_0 = CreateProduct("Quail eggs", 5, "60 eggs", "Small ones");

            u7.Businesses.Add(b32);
            b32.Products.Add(p32_0);
            b32.Workdays.Add(t21_0);


            Business b33 = CreateBusiness("Fresh bread kvass", "25.258030", "54.698995", "+37062199359", "Drink", null);
            Product p33_0 = CreateProduct("Kvass", 4, "1 liter", "Refreshing taste");

            u8.Businesses.Add(b33);
            b33.Products.Add(p33_0);
            b33.Workdays.Add(t21_0);


            Business b34 = CreateBusiness("Home made vine from apples", "25.180800", "54.725810", "+37062199388", "Drink", null);
            Product p34_0 = CreateProduct("Apple vine", 30, "1 bottle", "It is hold for 10 years");

            u9.Businesses.Add(b34);
            b34.Products.Add(p34_0);
            b34.Workdays.Add(t21_0);


            Business b35 = CreateBusiness("Amazing apple juice", "25.180800", "54.725810", "+37062199388", "Drink", null);
            Product p35_0 = CreateProduct("Bag of apple juice", 8, "5 liters", "Can hold 1 year if not opened");

            u9.Businesses.Add(b35);
            b35.Products.Add(p35_0);
            b35.Workdays.Add(t21_0);


            Business b36 = CreateBusiness("This season walnuts", "25.088591", "54.778514", "+37062199301", "Nuts", null);
            Product p36_0 = CreateProduct("Walnuts", 10, "1 kilo", "Nuts from lithuanian garden");

            u11.Businesses.Add(b36);
            b36.Products.Add(p36_0);
            b36.Workdays.Add(t21_0);


            Business b37 = CreateBusiness("Teaching math to pass exams", "25.249243", "54.692726", "+37062199222", "Tutor", null);
            Product p37_0 = CreateProduct("Lesson", 20, "1 hour", "For highschool graders");

            u1.Businesses.Add(b37);
            b37.Products.Add(p37_0);
            b37.Workdays.Add(t21_0);


            Business b38 = CreateBusiness("Profesional help for good results on exam", "25.262642", "54.680421", "+37062199374", "Tutor", null);
            Product p38_0 = CreateProduct("Lesson", 30, "1 hour", "For highschool graders");

            u2.Businesses.Add(b38);
            b38.Products.Add(p38_0);
            b38.Workdays.Add(t21_0);


            Business b39 = CreateBusiness("Try to pass exams and help to understand topics better", "25.276590", "54.750784", "+37062199365", "Tutor", null);
            Product p39_0 = CreateProduct("Lesson", 30, "1 hour", "For highschool graders");

            u3.Businesses.Add(b39);
            b39.Products.Add(p39_0);
            b39.Workdays.Add(t21_0);

            Business b40 = CreateBusiness("Help to prepare for exams", "25.241432", "54.807413", "+37062199332", "Tutor", null);
            Product p40_0 = CreateProduct("Lesson", 25, "1 hour", "For highschool graders");

            u4.Businesses.Add(b40);
            b40.Products.Add(p40_0);
            b40.Workdays.Add(t21_0);

            return new User[] { u1, u2, u3, u4, u5, u6, u7, u8, u9, u10, u11 };
        }

    }
}
