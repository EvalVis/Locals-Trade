using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Support_Your_Locals.Models;
using Support_Your_Locals.Models.Repositories;
using Support_Your_Locals.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;



namespace Support_Your_Locals.Controllers
{
    public class AdminController : Controller
    {

        private IServiceRepository repository;
        private IConfiguration configuration;
        public AdminController(IServiceRepository repo, IConfiguration config)
        {
            repository = repo;
            configuration = config;
        }
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            return View(new AdminViewModel()
            {
                Businesses = repository.Business.Include(b => b.User).Include(b => b.Products),
                Users = repository.Users.Where(notAdmin),
                TotalBusiness = repository.Business.Count(),
                TotalProducts = repository.Products.Count(),
                TotalUsers = repository.Users.Where(notAdmin).Count()
            });
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteBusiness(long id)
        {
            repository.DeleteBusiness(new Models.Business { BusinessID = id });
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteUser(long id)
        {
            repository.DeleteUser(new Models.User { UserID = id });
            return RedirectToAction("Index");
        }

        private bool notAdmin(User user)
        {
            return !configuration.GetSection("Admin:AdminEmails").Get<List<string>>().Contains(user.Email);
        } 
    }
}
