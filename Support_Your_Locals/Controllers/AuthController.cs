using System;
using System.Diagnostics;
using System.Linq;
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
        public ActionResult SignUp(string name, string surname, DateTime birthDate, string email)
        {
            int count = repository.Users.Count(b => b.Email == email);
                if (count == 0)
                {
                    context.Users.Add(new User {Name = name, Surname = surname, BirthDate = birthDate, Email = email});
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
        public ActionResult SignIn(string email)
        {
            int count = repository.Users.Count(b => b.Email == email);
            User user = repository.Users.FirstOrDefault(b => b.Email == email);
                if (count == 1)
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
