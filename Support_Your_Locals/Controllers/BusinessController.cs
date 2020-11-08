using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Support_Your_Locals.Models;
using Support_Your_Locals.Models.Repositories;
using Support_Your_Locals.Models.ViewModels;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
        public ActionResult Index(string business)
        {
            Business b = JsonSerializer.Deserialize<Business>(business);
            if (b == null) return NotFound();
            return View(b);
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
