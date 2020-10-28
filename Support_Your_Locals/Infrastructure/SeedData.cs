using System;
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
                context.Users.AddRange(
                    new User {Name = "Evaldas", Surname = "Visockas", BirthDate = new DateTime(1999,12,06,00,00,00), Email = "vievaldas@gmail.com"},
                    new User {Name = "Šarūnas", Surname = "Teisutis", BirthDate = new DateTime(1996,04,02,00,00,00), Email = "teisutis@gmail.com"},
                    new User {Name = "Kazys", Surname = "Bruolė", BirthDate = new DateTime(1950,01,02,00,00,00), Email = "bruoleparpuole@gmail.com"},
                    new User {Name = "Birutė", Surname = "Išdykėlė", BirthDate = new DateTime(2001,01,15,00,00,00), Email = "birute@gmail.com"},
                    new User {Name = "Lapinas", Surname = "Baronas", BirthDate = new DateTime(2005,10,15,00,00,00), Email = "gandras@gmail.com"},
                    new User {Name = "Konradas", Surname = "Rado", BirthDate = new DateTime(2018,06,05,00,00,00), Email = "neturiu@gmail.com"},
                    new User {Name = "Vaiva", Surname = "Vaivorykštė", BirthDate = new DateTime(1961,07,02,00,00,00), Email = "vaivaesu@gmail.com"},
                    new User {Name = "Vaidas", Surname = "Grinius", BirthDate = new DateTime(2004,03,06,00,00,00), Email = "vaidas2004@gmail.com"},
                    new User {Name = "Laputė", Surname = "Laisvuolė", BirthDate = new DateTime(1930,02,15,00,00,00), Email = "laisvoji.lape@gmail.com"},
                    new User {Name = "Kazys", Surname = "Keistuolis", BirthDate = new DateTime(1994,04,12,00,00,00), Email = "1994.kazys.04@gmail.com"},
                    new User {Name = "Rasa", Surname = "Išplaukusi", BirthDate = new DateTime(2000,01,03,00,00,00), Email = "rasa.ryto@gmail.com"}
                );
                context.SaveChanges();
            }

            if (!context.Business.Any())
            {
                context.Business.AddRange(
                    new Business {UserID = 1, Product = "Traktoriai", Description = "Traktorių parduotuvė", Header = "10% nuolaida žieminėms padangoms"},
                    new Business {UserID = 1, Product = "Automobiliai", Description = "Daužti automobiliai už prieinamą kainą", Header = "Įsigykite automobilius be rėmo, greit neliks"},
                    new Business {UserID = 1, Product = "Saldainiai", Description = "Įvairūs", Header = "Saldainių parduotuvė"},
                    new Business {UserID = 4, Product = "Išdaigos", Header = "Išpardavimas išdaigininkams"},
                    new Business {UserID = 8, Product = "Automobilių remontas", Description = "Nuo 7:00 iki 8:00 ryto remontuoju už dyką", Header = "Automobilių remontas"},
                    new Business {UserID = 10, Product = "Nusirašinėjimas", Description = "Rašau referatus, rašinius ir t.t.", Header = "Nenorit daryt mokyklinio projekto?" +
                        "Susisiekit, padarysiu už jus!", PhoneNumber = "+37064752660"},
                    new Business {UserID = 7, Product = "Piešiniai", Description = "Galiu daryti portretus, natiurmortą, peisažą ir t.t.", Header = "Piešiniai už (beveik) dyką!",
                        PhoneNumber = "+37064233450"},
                    new Business {UserID = 8, Product = "Dėvėtos kojinės", Description = "Paprastai labai kvepia", Header = "Yra ir smirdančių"},
                    new Business {UserID = 4, Product = "Labdara seneliams", Description = "Paaukokite senelių namuose gyvenantiems senjorams", Header = "Padarykite gerą darbą " +
                        "vieno mygtuko paspaudimu!", PhoneNumber = "+37062199630"}

                        );
                context.SaveChanges();
            }

            if (!context.TimeSheets.Any())
            {
                context.TimeSheets.AddRange(
                    new TimeSheet {BusinessID = 1, From = new DateTime(1999,12,06,10,00, 00), To = new DateTime(1999,12,06,11,00,00), Weekday = 1},
                    new TimeSheet { BusinessID = 1, From = new DateTime(1999, 12, 06, 10, 00, 00), To = new DateTime(1999, 12, 06, 16, 00, 00), Weekday = 3 },
                    new TimeSheet { BusinessID = 2, From = new DateTime(1999, 12, 06, 10, 00, 00), To = new DateTime(1999, 12, 06, 12, 00, 00), Weekday = 2 },
                    new TimeSheet { BusinessID = 3, From = new DateTime(1999, 12, 06, 8, 00, 00), To = new DateTime(1999, 12, 06, 12, 00, 00), Weekday = 1 },
                    new TimeSheet { BusinessID = 3, From = new DateTime(1999, 12, 06, 8, 00, 00), To = new DateTime(1999, 12, 06, 11, 00, 00), Weekday = 2 },
                    new TimeSheet { BusinessID = 4, From = new DateTime(1999, 12, 06, 8, 00, 00), To = new DateTime(1999, 12, 06, 11, 00, 00), Weekday = 6 },
                    new TimeSheet { BusinessID = 5, From = new DateTime(1999, 12, 06, 7, 00, 00), To = new DateTime(1999, 12, 06, 9, 00, 00), Weekday = 4 },
                    new TimeSheet { BusinessID = 6, From = new DateTime(1999, 12, 06, 7, 00, 00), To = new DateTime(1999, 12, 06, 16, 30, 00), Weekday = 1 },
                    new TimeSheet { BusinessID = 6, From = new DateTime(1999, 12, 06, 7, 00, 00), To = new DateTime(1999, 12, 06, 16, 30, 00), Weekday = 2 },
                    new TimeSheet { BusinessID = 6, From = new DateTime(1999, 12, 06, 7, 00, 00), To = new DateTime(1999, 12, 06, 16, 30, 00), Weekday = 3 },
                    new TimeSheet { BusinessID = 6, From = new DateTime(1999, 12, 06, 7, 00, 00), To = new DateTime(1999, 12, 06, 16, 30, 00), Weekday = 4 },
                    new TimeSheet { BusinessID = 6, From = new DateTime(1999, 12, 06, 7, 00, 00), To = new DateTime(1999, 12, 06, 16, 30, 00), Weekday = 5 },
                    new TimeSheet { BusinessID = 9, From = new DateTime(1999, 12, 06, 01, 00, 00), To = new DateTime(1999, 12, 06, 23, 30, 00), Weekday = 7 }
                    );
                context.SaveChanges();
            }
        }
    }
}
