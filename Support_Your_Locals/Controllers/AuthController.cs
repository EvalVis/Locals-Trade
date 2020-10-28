using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Support_Your_Locals.Infrastructure.Extensions;
using Support_Your_Locals.Models;
using Support_Your_Locals.Models.Repositories;
using Support_Your_Locals.Models.ViewModels;

namespace Support_Your_Locals.Controllers
{
    public class AuthController : Controller
    {
        public byte[] salt = new byte[16];

        private IServiceRepository repository;
        private ServiceDbContext context;
        public AuthController(IServiceRepository repo, ServiceDbContext serviceDbContext)
        {
            repository = repo;
            context = serviceDbContext;

        }

        [HttpGet]
        public ViewResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(UserRegisterModel user)
        {
            if (!ModelState.IsValid)
            {
                if (user.Email == null) ViewBag.mail = "true";
                if (user.Name == null) ViewBag.name = "true";
                if (user.Surname == null) ViewBag.surname = "true";
                if (user.BirthDate.Equals(DateTime.Parse("01/01/0001 12:00:00 AM"))) ViewBag.date = "true";
                if (user.Passhash == null) ViewBag.pass = "true";
                return View();
            }
            int count = repository.Users.Count(b => b.Email == user.Email);
                if (count == 0)
                {
                
                new RNGCryptoServiceProvider().GetBytes(salt);
                var pbkdf2 = new Rfc2898DeriveBytes(user.Passhash, salt, 100000);
                byte[] hash = pbkdf2.GetBytes(20);
                byte[] hashBytes = new byte[36];
                Array.Copy(salt, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);
                string savedPasswordHash = Convert.ToBase64String(hashBytes);


                context.Users.Add(new User {Name = user.Name, Surname = user.Surname, BirthDate = user.BirthDate, Email = user.Email, Passhash = savedPasswordHash });
                    context.SaveChanges();
                    ViewBag.email = "true";
                    return View();
                }
                else
                {
                    ViewBag.email = "false";
                    return View();
                }

        }

        [HttpGet]
        public ViewResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(UserLoginModel useris)
        {
            if (!ModelState.IsValid)
            {
                if (useris.Email == null) ViewBag.mail = "true";
                if (useris.Passhash == null) ViewBag.pass = "true";
                return View();
            }
            bool goodpass = false;
            User user = repository.Users.FirstOrDefault(b => b.Email == useris.Email);
            string savedPasswordHash = user.Passhash;
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(useris.Passhash, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            for (int i = 0; i < 20; i++) {
                if (hashBytes[i + 16] != hash[i]) goodpass = true;
            }
               
                    
            if (user.Email == useris.Email && goodpass)
                {
                    ViewBag.email = "true";
                    HttpContext.Session.SetJson("user", user);
                    return View();
                }
                else
                {
                    ViewBag.email = "false";
                    return View();
                }
        }
    }
}
