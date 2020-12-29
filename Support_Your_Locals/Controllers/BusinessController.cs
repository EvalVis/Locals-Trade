using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Support_Your_Locals.Models;
using Support_Your_Locals.Models.Repositories;
using Support_Your_Locals.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Support_Your_Locals.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Support_Your_Locals.Controllers
{
    public class BusinessController : Controller
    {

        private IServiceRepository repository;
        private IConfiguration configuration;
        private long userID;

        public BusinessController(IServiceRepository repo, IHttpContextAccessor accessor, IConfiguration config)
        {
            repository = repo;
            configuration = config;
            long.TryParse(accessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value, out userID);
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
            ViewBag.userID = userID;
            return View(business);
        }

        [HttpPost]
        public void AddFeedback(string senderName, string text, long businessId)
        {
            Feedback feedback = new Feedback { SenderName = senderName, Text = text, BusinessID = businessId };
            repository.AddFeedback(feedback);
            new Mailer(repository, configuration).SendMail(feedback);
        }

        [HttpPost]
        public ActionResult AddOrder(int amount, string address, string comment, long productId)
        {
            if(userID < 1)
            {
                return Unauthorized();
            }
            Order order = new Order { Amount = amount, Address = address, Comment = comment, UserId = userID, ProductId = productId, DateAdded = DateTime.Now };
            repository.AddOrder(order);
            return Ok();
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
            if (ModelState.IsValid)
            {
                Business business = new Business(businessRegisterModel, userID);
                repository.AddBusiness(business);
                return Redirect("/user/businesses");
            }
            return View();
        }

    }
}
