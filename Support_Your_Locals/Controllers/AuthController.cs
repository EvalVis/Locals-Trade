using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Support_Your_Locals.Infrastructure.Extensions;
using Support_Your_Locals.Models;
using Support_Your_Locals.Models.Repositories;
using Support_Your_Locals.Models.ViewModels;

namespace Support_Your_Locals.Controllers
{
    public class AuthController : Controller
    {
        public byte[] salt = new byte[16];

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
        public ActionResult SignUp(UserRegisterModel register)
        {
            if (ModelState.IsValid)
            {
                bool exists = userRepository.Users.Any(b => b.Email == register.Email);
                register.AlreadyExists = exists;
                if (!exists)
                {
                    new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
                    var pbkdf2 = new Rfc2898DeriveBytes(register.Passhash, salt, 100000);
                    byte[] hash = pbkdf2.GetBytes(20);
                    byte[] hashBytes = new byte[36];
                    Array.Copy(salt, 0, hashBytes, 0, 16);
                    Array.Copy(hash, 0, hashBytes, 16, 20);
                    string savedPasswordHash = Convert.ToBase64String(hashBytes);
                    userRepository.AddUser(new User
                    {
                        Name = register.Name,
                        Surname = register.Surname,
                        BirthDate = register.BirthDate,
                        Email = register.Email,
                        Passhash = savedPasswordHash
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
        public async Task<ActionResult> SignIn(UserLoginModel login)
        {
            if (ModelState.IsValid)
            {
                User user = userRepository.Users.FirstOrDefault(b => b.Email == login.Email);
                if (user != null)
                {
                    bool goodpass = false;
                    string savedPasswordHash = user.Passhash;
                    byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
                    Array.Copy(hashBytes, 0, salt, 0, 16);
                    var pbkdf2 = new Rfc2898DeriveBytes(login.Passhash, salt, 100000);
                    byte[] hash = pbkdf2.GetBytes(20);
                    for (int i = 0; i < 20; i++)
                    {
                        if (hashBytes[i + 16] == hash[i]) goodpass = true;
                        else
                        {
                            goodpass = false;
                            break;
                        }
                    }
                    if (goodpass)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Name)
                        };
                        var identity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        var props = new AuthenticationProperties();
                        HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme, principal, props).Wait();

                        login.NotFound = false;
                        HttpContext.Session.SetJson("user", user);
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
