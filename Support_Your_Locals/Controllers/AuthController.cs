using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private IConfiguration configuration;

        public AuthController(IServiceRepository repo, HashCalculator hashCalc, IConfiguration config)
        {
            userRepository = repo;
            hashCalculator = hashCalc;
            configuration = config;
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
                        Passhash = hashCalculator.PassHash(register.Password)
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
                    if (hashCalculator.IsGoodPass(user.Passhash, login.Passhash))
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Name),
                            new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                            new Claim(ClaimTypes.Role, isAdmin(login.Email) ? "Administrator": "User"),
                        };
                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
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

        
        private bool isAdmin(string email)
        {
            var adminEmails = configuration.GetSection("Admin:AdminEmails").Get<List<string>>();
            return adminEmails.Contains(email);
        }

        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

    }
}
