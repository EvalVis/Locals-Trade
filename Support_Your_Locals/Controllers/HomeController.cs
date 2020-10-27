using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Support_Your_Locals.Infrastructure.Extensions;
using Support_Your_Locals.Models;
using Support_Your_Locals.Models.Repositories;
using Support_Your_Locals.Models.ViewModels;
using Support_Your_Locals.Models.ViewModels.BusinessBoard;

namespace Support_Your_Locals.Controllers
{
    public class HomeController : Controller
    {

        private IServiceRepository repository;
        private IUserRepository userRepository;
        public int PageSize = 4;

        public HomeController(IServiceRepository repo, IUserRepository userRepo)
        {
            repository = repo;
            userRepository = userRepo;
        }

        public ViewResult Index(SearchResponse searchResponse, string category, int productPage = 1)
        {
            IEnumerable<Business> businesses = repository.Business
                .Where(b => category == null || b.Product == category);
            IEnumerable<UserBusinessTimeSheets> userBusinessTimeSheets = searchResponse.FilterBusinesses(businesses, repository, userRepository).
                OrderBy(ubts => ubts.Business.BusinessID).
                Skip((productPage - 1) * PageSize).
                Take(PageSize);
            /*Join(repository.Users, business => business.UserID, user => user.UserID,
            (business, user) => new UserBusiness
            {
                User = user,
                Business = business
            });*/
            return View(new BusinessListViewModel
            {
                UserBusinessTimeSheets = userBusinessTimeSheets,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = searchResponse.FilterBusinesses(businesses, repository, userRepository).Count()
                },
                CurrentCategory = category,
                SearchResponse = searchResponse
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
