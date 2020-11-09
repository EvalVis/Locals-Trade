using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Support_Your_Locals.Infrastructure.Extensions;
using Support_Your_Locals.Models;
using Support_Your_Locals.Models.Repositories;
using Support_Your_Locals.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Support_Your_Locals.Controllers
{
    public class BusinessController : Controller
    {

        private IServiceRepository repository;

        public BusinessController(IServiceRepository repo)
        {
            repository = repo;
        }

        [HttpGet]
        public ViewResult Index(long businessId)
        {
            //TODO: Exception handling (ect. businessId = 0).
            if (businessId == 0) businessId = 1; //This kind of solves it.
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

        [Authorize]
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
                    UserID = 1,
                    Product = businessRegisterModel.Product,
                    PhoneNumber = businessRegisterModel.PhoneNumber,
                    Latitude = businessRegisterModel.Latitude,
                    Longitude = businessRegisterModel.Longitude,
                    Pictures = businessRegisterModel.Pictures.Where(item => item != null).ToList(),
                };
                repository.AddBusiness(business);
                return View();
            }
            return View();
        }
    }
}
