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

namespace Support_Your_Locals.Controllers
{
    public class BusinessController : Controller
    {
        public delegate void FeedbackHandler(Feedback feedback);
        public static event FeedbackHandler FeedbackEvent;

        private IServiceRepository repository;

        public BusinessController(IServiceRepository repo)
        {
            repository = repo;
        }

        [HttpGet]
        public async Task<ActionResult> Index(long businessId)
        {
            if (businessId == 0) businessId = 1;
            Business business = await repository.Business.Include(b => b.User).
                Include(b => b.Workdays).Include(b => b.Products)
                .Include(b => b.Feedbacks)
                .FirstOrDefaultAsync(b => b.BusinessID == businessId);
            if (business == null) return NotFound();
            return View(business);
        }

        [HttpPost]
        public void AddFeedback(string senderName, string text, long businessId)
        {
            Feedback feedback = new Feedback {SenderName = senderName, Text = text, BusinessID = businessId};
            repository.AddFeedback(feedback);
            FeedbackEvent?.Invoke(feedback);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> AddAdvertisement(long businessId)
        {
            if (businessId > 0)
            {
                Business business = await repository.Business.Include(b => b.Workdays)
                    .Include(b => b.Products)
                    .FirstOrDefaultAsync(b => b.BusinessID == businessId);
                if (business != null)
                {
                    BusinessRegisterModel businessRegisterModel = new BusinessRegisterModel()
                    {
                        Description = business.Description,
                        PhoneNumber = business.PhoneNumber,
                        Header = business.Header,
                        Longitude = business.Longitude,
                        Latitude = business.Latitude,
                        Picture = business.Picture,
                        Products = business.Products
                    };
                    foreach (TimeSheet workday in business.Workdays)
                    {
                        businessRegisterModel.Workdays[workday.Weekday - 1] = workday;
                    }
                    return View(businessRegisterModel);
                }
                return Redirect("/");
            }
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
                UserID = long.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value),
                PhoneNumber = businessRegisterModel.PhoneNumber,
                Latitude = businessRegisterModel.Latitude,
                Longitude = businessRegisterModel.Longitude,
                Picture = businessRegisterModel.Picture,
                Workdays = businessRegisterModel.Workdays.ToList(),
                Products = businessRegisterModel.Products
            };
            repository.AddBusiness(business);
            return Redirect("/");
        }
    }
}
