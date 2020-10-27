﻿using System;
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
        public ActionResult SignUp(string name, string surname, DateTime birthDate, string email)
        {
            int count = userRepository.Users.Count(b => b.Email == email);
                if (count == 0)
                {
                    userRepository.Add(new User {Name = name, Surname = surname, BirthDate = birthDate, Email = email});
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
                else
                {
                    login.NotFound = true;
                    return View(login);
                }
            }
            else return View();
        }
    }
}
