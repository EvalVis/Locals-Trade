﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Support_Your_Locals.Models;
using Support_Your_Locals.Models.Repositories;
using Support_Your_Locals.Models.ViewModels;

namespace Support_Your_Locals.Controllers
{
    public class AuthController : Controller
    {

        private IServiceRepository userRepository;

        public AuthController(IServiceRepository repo)
        {
            userRepository = repo;
        }

        [HttpGet]
        public ViewResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SignUp(UserRegisterModel register)
        {
            if (ModelState.IsValid)
            {
                bool exists = await userRepository.Users.AnyAsync(b => b.Email == register.Email);
                register.AlreadyExists = exists;
                if (!exists)
                {
                    
                    userRepository.AddUser(new User
                    {
                        Name = register.Name,
                        Surname = register.Surname,
                        BirthDate = register.BirthDate,
                        Email = register.Email,
                        Passhash = PassHash(register.Passhash)
                    });
                    return Redirect("/");
                }
                return View(register);
            }
            return View();
        }
        public byte[] salt = new byte[16];

        public bool IsGoodPass (string userpass, string loginpass)
        {
            bool goodpass = false;
            string savedPasswordHash = userpass;
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(loginpass, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            for (int i = 0; i< 20; i++){
            if (hashBytes[i + 16] == hash[i]) goodpass = true;
            else{
            goodpass = false;
            break;
            }
            }
            return goodpass;
        }

        private string PassHash(string pass)
        {
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var pbkdf2 = new Rfc2898DeriveBytes(pass, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            return Convert.ToBase64String(hashBytes);
        }

        [HttpGet]
        public ViewResult SignIn()
        {
            return View();
        }

        [HttpPost] // TODO: Include.
        public async Task<ActionResult> SignIn(UserLoginModel login)
        {
            if (ModelState.IsValid)
            {
                User user = await userRepository.Users.FirstOrDefaultAsync(b => b.Email == login.Email);
                if (user != null)
                {
                    if (IsGoodPass(user.Passhash, login.Passhash))
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.UserID.ToString())
                        };
                        var identity = new ClaimsIdentity(claims, "SignIn");
                        var principal = new ClaimsPrincipal(identity);
                        var props = new AuthenticationProperties();
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);

                        login.NotFound = false;
                        return Redirect("/");
                    }

                }
                login.NotFound = true;
                return View(login);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
        
    }
}
