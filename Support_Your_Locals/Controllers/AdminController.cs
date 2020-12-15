using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        private IConfiguration configuration;
        private ILegacyServiceRepository repository;

        public AdminController(ILegacyServiceRepository repo, IConfiguration config)
        {
            configuration = config;
            repository = repo;

        }
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            var businesses = repository.GetBusinesses();
            var usersBusinesses = businesses.GroupBy(business => business.User);
            var users = repository.GetUsers().Where(notAdmin);
            return View(new AdminViewModel()
            {
                Businesses = businesses,
                UsersBusinesses = usersBusinesses,
                Users = repository.GetUsers().Where(notAdmin),
                TotalBusiness = businesses.Count(),
                TotalProducts = repository.GetProducts().Count(),
                TotalUsers = users.Count(),
            });
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteBusiness(long id)
        {
            repository.DeleteBusiness(id);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteUser(long id)
        {
            repository.DeleteUser(id);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Reasign(long id, long targetUserId)
        {
            return RedirectToAction("Index");
        }

        private bool notAdmin(User user)
        {
            return !configuration.GetSection("Admin:AdminEmails").Get<List<string>>().Contains(user.Email);
        } 
    }
}
