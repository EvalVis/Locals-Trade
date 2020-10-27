using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Support_Your_Locals.Infrastructure.Extensions;
using Support_Your_Locals.Models;
using Support_Your_Locals.Models.Repositories;
using Support_Your_Locals.Models.ViewModels;

namespace Support_Your_Locals.Controllers
{
    public class AuthController : Controller
    {

        private IUserRepository userRepository;

        public AuthController(IUserRepository repo)
        {
            userRepository = repo;
        }

        [HttpGet]
        public ViewResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(UserRegisterModel register)
        {
            if (ModelState.IsValid)
            {
                bool exists = userRepository.Users.Any(b => b.Email == register.Email);
                register.AlreadyExists = exists;
                if (!exists)
                {
                    userRepository.Add(new User
                    {
                        Name = register.Name,
                        Surname = register.Surname,
                        BirthDate = register.BirthDate,
                        Email = register.Email
                    });
                    return Redirect("/");
                }
                return View(register);
            }
            return View();
        }

        [HttpGet]
        public ViewResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(UserLoginModel login)
        {
            if (ModelState.IsValid)
            {
                User user = userRepository.Users.FirstOrDefault(b => b.Email == login.Email);
                if (user != null)
                {
                    login.NotFound = false;
                    HttpContext.Session.SetJson("user", user);
                    return Redirect("/");
                }
                login.NotFound = true;
                return View(login);
            }
            return View();
        }
    }
}
