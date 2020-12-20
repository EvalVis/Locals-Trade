using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Support_Your_Locals.Models;
using Support_Your_Locals.Models.Repositories;
using Support_Your_Locals.Models.ViewModels.BusinessBoard;

namespace Support_Your_Locals.Controllers
{
    public class HomeController : Controller
    {

        private IServiceRepository repository;
        public int PageSize;

        public HomeController(IServiceRepository repo, IConfiguration configuration)
        {
            repository = repo;
            PageSize = int.Parse(configuration["Pages:pagesSize"]);
        }

        [Route("Home/Index/page{page:int}/{product}")]
        [Route("Home/Index/{product}/page{page:int}")]
        [Route("Home/Index/{product}")]
        [Route("Home/Index/page{page:int}")]
        [Route("Home/Index")]
        [Route("/")]
        public ViewResult Index(SearchResponse searchResponse, string product, int page = 1)
        {
            IEnumerable<Business> businesses = repository.Business
                .Where(b => product == null || b.Products.Any(p => p.Name == product)).
                Include(b => b.User).Include(b => b.Workdays).Include(b => b.Products);
            IEnumerable<Business> filteredBusinesses = searchResponse.FilterBusinesses(businesses).
                OrderBy(b => b.BusinessID).
                Skip((page - 1) * PageSize).
                Take(PageSize);
            return View(new BusinessListViewModel
            {
                Businesses = filteredBusinesses,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = searchResponse.FilterBusinesses(businesses).Count()
                },
                CurrentProduct = product,
                SearchResponse = searchResponse
            });
        }

        public ViewResult Error(string code)
        {
            switch (code)
            {
                case "403":
                    return View("Errors/Forbidden");
                case "404":
                    return View("Errors/NotFound");
            }
            return View("Errors/InternalError");
        }
    }
}
