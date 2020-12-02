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
using Microsoft.AspNetCore.Http;
using Support_Your_Locals.Infrastructure;

namespace Support_Your_Locals.Controllers
{
    public class BusinessController : Controller
    {
        //public delegate void FeedbackHandler(Feedback feedback);
        //public static event FeedbackHandler FeedbackEvent;
        public static event EventHandler<FeedbackEventArgs> FeedbackEvent;

        private IServiceRepository repository;
        private long userID;

        public BusinessController(IServiceRepository repo, IHttpContextAccessor accessor)
        {
            repository = repo;
            long.TryParse(accessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value, out userID);
        }

        [HttpGet]
        public async Task<ActionResult> Index(long businessId)
        {
            if (businessId <= 0) Redirect("/");
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
            FeedbackEvent(this, new FeedbackEventArgs(feedback));
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
                { // TODO: Redirect replace with error.
                    if (business.UserID != userID) return Redirect("/");
                    BusinessRegisterModel businessRegisterModel = new BusinessRegisterModel(business);
                    return View(businessRegisterModel);
                }
                return Redirect("/");
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddAdvertisement(BusinessRegisterModel businessRegisterModel)
        {
            if (!ModelState.IsValid)
            {
              return View();
            }
            Business business = new Business
            {
                Header = businessRegisterModel.Header,
                Description = businessRegisterModel.Description,
                UserID = Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value),
                PhoneNumber = businessRegisterModel.PhoneNumber,
                Latitude = businessRegisterModel.Latitude,
                Longitude = businessRegisterModel.Longitude,
                Picture = businessRegisterModel.Picture
            };
            Business dbBusiness = await repository.Business
                .Include(b => b.Workdays).Include(b => b.Products)
                .FirstOrDefaultAsync(b => b.BusinessID == businessRegisterModel.BusinessId);
            if (dbBusiness.UserID == userID)
            {
                dbBusiness.UpdateBusiness(businessRegisterModel);
                repository.SaveBusiness(dbBusiness);
            }
            return Redirect("/");
        }

    }
}
