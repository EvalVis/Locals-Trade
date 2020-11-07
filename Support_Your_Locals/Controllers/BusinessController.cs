using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Support_Your_Locals.Models;
using Support_Your_Locals.Models.Repositories;
using Support_Your_Locals.Models.ViewModels;

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
            Business business = repository.Business.Include(b => b.Workdays).FirstOrDefault(b => b.BusinessID == businessId);
            User user = repository.Users.FirstOrDefault(u => u.UserID == business.UserID);
            IEnumerable<TimeSheet> timeSheets = business.Workdays;
            UserBusinessTimeSheets userBusinessTimeSheets = new UserBusinessTimeSheets
            {
                User = user,
                Business = business,
                TimeSheets = timeSheets
            };
            return View(userBusinessTimeSheets);
        }

        [HttpGet]
        public ActionResult AddAdvertisement()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddAdvertisement(BusinessRegisterModel businessRegisterModel)
        {
            //TODO: validation
            Business business = new Business
            {
                // Exception here
                Header = businessRegisterModel.Header,
                Description = businessRegisterModel.Description,
                UserID = 1, // TODO: fix
                Product = businessRegisterModel.Product,
                PhoneNumber = businessRegisterModel.PhoneNumber,
                Latitude = businessRegisterModel.Latitude,
                Longitude = businessRegisterModel.Longitude,
            };
            for (int i = 0; i < 7; i++)
            {
                TimeSheetRegisterViewModel day = businessRegisterModel.Workdays[i];
                DateTime from = day.From;
                DateTime to = day.To;
                if (TimeSheetRegisterViewModel.Invalid(from, to)) continue;
                TimeSheet workday = new TimeSheet { From = day.From, To = day.To, Weekday = day.Weekday, Business = business};
                business.Workdays.Add(workday);
            }
            repository.AddBusiness(business);
            return Redirect("/");
        }
    }
}
