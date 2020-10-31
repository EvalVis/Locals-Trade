using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
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

        public ViewResult Index(SearchResponse searchResponse, string product, int page = 1)
        {
            IEnumerable<Business> businesses = repository.Business
                .Where(b => product == null || b.Product == product);
            List<UserBusinessTimeSheets> filterBusiness = searchResponse.FilterBusinesses(businesses, repository).ToList();
            IEnumerable<UserBusinessTimeSheets> userBusinessTimeSheets = filterBusiness.
                OrderBy(ubts => ubts.Business.BusinessID).
                Skip((page - 1) * PageSize).
                Take(PageSize);
            return View(new BusinessListViewModel
            {
                UserBusinessTimeSheets = userBusinessTimeSheets,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = filterBusiness.Count
                },
                CurrentProduct = product,
                SearchResponse = searchResponse
            });
        }

    }
}
