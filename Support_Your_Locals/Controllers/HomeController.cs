using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Support_Your_Locals.Models;
using Support_Your_Locals.Models.Repositories;
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

        public ViewResult Index([FromQuery] SearchResponse searchResponse, string product, int page = 1)
        {
            searchResponse.SetWeekdaySelected();
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

    }
}
