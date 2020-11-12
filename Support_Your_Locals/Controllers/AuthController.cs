using System;
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
using Support_Your_Locals.Cryptography;
using Support_Your_Locals.Models;
using Support_Your_Locals.Models.Repositories;
using Support_Your_Locals.Models.ViewModels;

namespace Support_Your_Locals.Controllers
{
    public class AuthController : Controller
    {

        private IServiceRepository userRepository;
        private HashCalculator hashCalculator;
        public byte[] salt = new byte[16];

        public AuthController(IServiceRepository repo, HashCalculator hashCalc)
        {
            userRepository = repo;
            hashCalculator = hashCalc;
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
                        Passhash = hashCalculator.PassHash(register.Passhash)
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

        [HttpPost] // TODO: Include.
        public async Task<ActionResult> SignIn(UserLoginModel login)
        {
            if (ModelState.IsValid)
            {
                User user = await userRepository.Users.FirstOrDefaultAsync(b => b.Email == login.Email);
                if (user != null)
                {
                    if (hashCalculator.IsGoodPass(user.Passhash, login.Passhash, salt))
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Name),
                            new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString())
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
