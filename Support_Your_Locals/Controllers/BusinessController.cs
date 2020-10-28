using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Support_Your_Locals.Infrastructure.Extensions;
using Support_Your_Locals.Models;
using Support_Your_Locals.Models.Repositories;
using Support_Your_Locals.Models.ViewModels;

namespace Support_Your_Locals.Controllers
{
    public class BusinessController : Controller
    {

        private IServiceRepository repository;
        private ServiceDbContext context;

        public BusinessController(IServiceRepository repo, ServiceDbContext ctx)
        {
            repository = repo;
            context = ctx;
        }

        [HttpPost]
        public ViewResult Index(long businessId)
        {
            Business business = repository.Business.FirstOrDefault(b => b.BusinessID == businessId);
            User user = repository.Users.FirstOrDefault(u => u.UserID == business.UserID);
            IEnumerable<TimeSheet> timeSheets = repository.TimeSheets.Where(t => t.BusinessID == business.BusinessID);
            UserBusinessTimeSheets userBusinessTimeSheets = new UserBusinessTimeSheets
            {
                User = user,
                Business = business,
                TimeSheets = timeSheets
            };
            return View(userBusinessTimeSheets);
        }

        [HttpGet]
        public ViewResult AddAdvertisement()
        {
            return View();
        }

        [HttpPost]
        public ViewResult AddAdvertisement(BusinessRegisterModel businessRegisterModel)
        {
            if (ModelState.IsValid)
            {
                Business business = new Business
                {
                    // Exception here
                    Header = businessRegisterModel.Header,
                    Description = businessRegisterModel.Description,
                    UserID = HttpContext.Session.GetJson<User>("user").UserID,
                    Product = businessRegisterModel.Product,
                    PhoneNumber = businessRegisterModel.PhoneNumber
                };
                context.Add(business);
                context.SaveChanges();
                return View();
            }
            else
            {
                return View();
            }
        }
    }
}
