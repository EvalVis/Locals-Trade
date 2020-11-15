using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Support_Your_Locals.Models;
using Support_Your_Locals.Models.Repositories;
using Support_Your_Locals.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Diagnostics;

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
        public async Task<ActionResult> Index(long businessId)
        {
            Business business = await repository.Business.Include(b => b.User).
                Include(b => b.Workdays).Include(b => b.Products)
                .FirstOrDefaultAsync(b => b.BusinessID == businessId);
            if (business == null) return NotFound();
            return View(business);
        }

        [HttpPost]
        public ActionResult AddFeedback(string senderName, string text, long businessId)
        {
            return Redirect("/");
        }

        [Authorize]
        [HttpGet]
        public ViewResult AddAdvertisement()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddAdvertisement(BusinessRegisterModel businessRegisterModel)
        {
            //TODO: validation
            Business business = new Business
            {
                // Exception here
                Header = businessRegisterModel.Header,
                Description = businessRegisterModel.Description,
                UserID = Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value),
                PhoneNumber = businessRegisterModel.PhoneNumber,
                Latitude = businessRegisterModel.Latitude,
                Longitude = businessRegisterModel.Longitude,
                Picture = businessRegisterModel.Picture
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

            foreach (var pr in businessRegisterModel.Products)
            {
                Product product = new Product {Name = pr.Name, PricePerUnit = pr.PricePerUnit, Unit = pr.Unit, Comment = pr.Comment, Picture = pr.Picture};
                business.Products.Add(product);
            }
            repository.AddBusiness(business);
            return Redirect("/");
        }
    }
}
