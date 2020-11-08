using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Support_Your_Locals.Models;
using Support_Your_Locals.Models.Repositories;
using Support_Your_Locals.Models.ViewModels;
using Support_Your_Locals.Models.ViewModels.BusinessBoard;

namespace Support_Your_Locals.Controllers
{
    public class HomeController : Controller
    {

        private IServiceRepository repository;
        public int PageSize = 4;

        public HomeController(IServiceRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index(SearchResponse searchResponse, string category, int productPage = 1)
        {
            IEnumerable<Business> businesses = repository.Business
                .Where(b => category == null || b.Product == category).Include(b => b.User).Include(b => b.Workdays);
            IEnumerable<Business> filteredBusinesses = searchResponse.FilterBusinesses(businesses).
                OrderBy(b => b.BusinessID).
                Skip((productPage - 1) * PageSize).
                Take(PageSize);
            return View(new BusinessListViewModel
            {
                Businesses = filteredBusinesses,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = searchResponse.FilterBusinesses(businesses).Count()
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
