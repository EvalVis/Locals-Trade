using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Support_Your_Locals.Models.Repositories;

namespace Support_Your_Locals.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {

        private IServiceRepository repository;

        public NavigationMenuViewComponent(IServiceRepository repo)
        {
            repository = repo;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(repository.Business.Select(x => x.Product).Distinct().OrderBy(x => x));
        }
    }
}
